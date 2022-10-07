using Microsoft.EntityFrameworkCore;
using RMS.Currencies;
using RMS.Customers;
using RMS.Status;
using RMS.Remittances;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace RMS.EntityFrameworkCore
{

    [ReplaceDbContext(typeof(IIdentityDbContext))]
    [ReplaceDbContext(typeof(ITenantManagementDbContext))]
    [ConnectionStringName("Default")]
    public class RMSDbContext :
        AbpDbContext<RMSDbContext>,
        IIdentityDbContext,
        ITenantManagementDbContext
    {
        /* Add DbSet properties for your Aggregate Roots / Entities here. */
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Remittance> Remittances { get; set; }
        public DbSet<RemittanceStatus> RemittanceStatus { get; set; }

        #region Entities from the modules

        /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
         * and replaced them for this DbContext. This allows you to perform JOIN
         * queries for the entities of these modules over the repositories easily. You
         * typically don't need that for other modules. But, if you need, you can
         * implement the DbContext interface of the needed module and use ReplaceDbContext
         * attribute just like IIdentityDbContext and ITenantManagementDbContext.
         *
         * More info: Replacing a DbContext of a module ensures that the related module
         * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
         */

        //Identity
        public DbSet<IdentityUser> Users { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }
        public DbSet<IdentityClaimType> ClaimTypes { get; set; }
        public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
        public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
        public DbSet<IdentityLinkUser> LinkUsers { get; set; }

        // Tenant Management
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

        #endregion

        public RMSDbContext(DbContextOptions<RMSDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */

            builder.ConfigurePermissionManagement();
            builder.ConfigureSettingManagement();
            builder.ConfigureBackgroundJobs();
            builder.ConfigureAuditLogging();
            builder.ConfigureIdentity();
            builder.ConfigureIdentityServer();
            builder.ConfigureFeatureManagement();
            builder.ConfigureTenantManagement();

            /* Configure your own tables/entities inside here */
            builder.Entity<Remittance>(b => {
                b.ToTable(RMSConsts.DbTablePrefix + "Remittances" + RMSConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasKey(x => new { x.Id });
                b.Property(x => x.Amount).IsRequired();
                b.Property(x => x.Type).IsRequired();
                b.Property(x => x.ReceiverFullName).IsRequired();

                //one-to-many relationship with IdentityUser table
                b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.CreatorId).IsRequired();
                b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.ApprovedBy);
                b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.ReleasedBy);

                //one-to-many relationship with Customer table

                b.HasOne<Customer>().WithMany().HasForeignKey(x => x.SenderBy).IsRequired();
                b.HasOne<Customer>().WithMany().HasForeignKey(x => x.ReceiverBy);

                //one-to-many relationship with Currency table

                b.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId);
                //many-to-many relationship with RemittanceStatus table 
                b.HasMany(x => x.Status).WithOne().HasForeignKey(x => x.RemittanceId);


                b.HasIndex(e => new { e.SerialNumber }).IsUnique();
            });


            builder.Entity<RemittanceStatus>(b => {
                b.ToTable(RMSConsts.DbTablePrefix + "RemittanceStatus" + RMSConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasKey(x => new { x.Id });

                b.Property(x => x.State).IsRequired();

                //one-to-many relationship with Remittance table
                b.HasOne<Remittance>().WithMany(x => x.Status).HasForeignKey(x => x.RemittanceId).IsRequired();
                //one-to-many relationship with IdentityUser table
                //b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.CreatorId).IsRequired();

            });

            builder.Entity<Customer>(b => {

                b.ToTable(RMSConsts.DbTablePrefix + "Customers" + RMSConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasKey(x => new { x.Id });
                b.Property(x => x.FirstName).IsRequired();
                b.Property(x => x.LastName).IsRequired();
                b.Property(x => x.FatherName).IsRequired();
                b.Property(x => x.MotherName).IsRequired();
                b.Property(x => x.BirthDate).IsRequired();
                b.Property(x => x.Phone).IsRequired();
                b.Property(x => x.Gender).IsRequired();

                b.HasIndex(e => new { e.FirstName, e.LastName, e.FatherName, e.MotherName }).IsUnique();

            });

            builder.Entity<Currency>(b => {

                b.ToTable(RMSConsts.DbTablePrefix + "Currencies" + RMSConsts.DbSchema);
                b.ConfigureByConvention();

                b.HasKey(x => new { x.Id });

                b.Property(x => x.Name).IsRequired();
                b.Property(x => x.Symbol).IsRequired();
                b.HasIndex(e => new { e.Name, e.Symbol }).IsUnique();


            });

            //builder.Entity<Book>(b => {
            //    b.ToTable(BookStoreConsts.DbTablePrefix + "Books" + BookStoreConsts.DbSchema);
            //    b.ConfigureByConvention(); b.Property(x => x.Name).HasMaxLength(BookConsts.MaxNameLength).IsRequired();

            //one-to-many relationship with Author table
            //b.HasOne<Author>().WithMany().HasForeignKey(x=>x.AuthorId).IsRequired();

            //many-to-many relationship with Category table => BookCategories
            //b.HasMany(x=>x.Categories).WithOne().HasForeignKey(x=>x.BookId).IsRequired();});
        }
    }
}
Remittance Management System  (نظام إدارة الحوالات) 

For Run Application:
 1- first change connection string in appsettings.json in RMS.Blazor and RMS.DbMigrator app.
 2- put RMS.Blazor startup Project and RMS.EntityFrameworkcore in default project in package manager console to Update-Database .
 3- put RMS.DbMigrator startup Project and run it.
 4- remove package-lock.json folder and yarn.Lock folder from RMS Folder
 5- in RMS folder turn on Terminall power shell and run the command 
 6- first command  :  npm install yarn
 7- second command:   dotnet tool install -g Volo.Abp.Cli --version 5.3.4
 8- third command :   abp install-libs --version 5.3.4
 9- fourth command :  dotnet build /graphBuild
 10- put RMS.Blazor startup Project And Run Application

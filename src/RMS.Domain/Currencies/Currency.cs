using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace RMS.Currencies
{
   public class Currency : AggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string Symbol { get; set; }
        public string Code { get; set; }
        private Currency() { }
        public Currency(Guid id,[NotNull] string name, [NotNull] string symbol, string code) : base(id) {
            SetName(name, symbol);
            Code = code;
            }
        internal Currency ChangeName([NotNull] string name, [NotNull] string symbol)
        {
            SetName(name,symbol);
            return this;
        }
        public Currency SetName(string name,string symbol) {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Symbol = Check.NotNullOrWhiteSpace(symbol, nameof(symbol));
            return this; 
        }
    }
}

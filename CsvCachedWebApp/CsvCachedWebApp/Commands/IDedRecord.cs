using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsvCachedWebApp.Commands
{
    public abstract class IDedRecord
    {
        public string Id { get; set; }
    }

    public abstract class IDedRecordClassMap<T> : CsvClassMap<T> where T : IDedRecord
    {
        public override void CreateMap()
        {
            Map(m => m.Id).Name("ID");
        }
    }

}

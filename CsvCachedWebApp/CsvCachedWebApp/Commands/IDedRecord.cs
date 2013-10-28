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



    public class SimpleRecord : IDedRecord
    {
        public string other { get; set; }

        public SimpleRecord()
        {

        }
        
        public SimpleRecord(string id, string other)
        {
            this.Id = id;
            this.other = other;
        }

    }

    public class SimpleRecordClassMap : IDedRecordClassMap<SimpleRecord>
    {
        public override void CreateMap()
        {
            base.CreateMap();
            Map(m => m.other).Name("OTHER");
        }

    }



    public class AnotherSimpleRecord : IDedRecord
    {
        public string another { get; set; }

        public AnotherSimpleRecord()
        {

        }
        
        public AnotherSimpleRecord(string id, string other)
        {
            this.Id = id;
            this.another = another;
        }

    }

    public class AnotherSimpleRecordClassMap : IDedRecordClassMap<AnotherSimpleRecord>
    {
        public override void CreateMap()
        {
            base.CreateMap();
            Map(m => m.another).Name("ANOTHER");
        }

    }

}

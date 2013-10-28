using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AsyncCommander;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsvCachedWebApp.Commands
{


    public class UpdateCsvCommand<T, U> : AbstractCommand
        where T : IDedRecord
        where U : IDedRecordClassMap<T>
    {

        public string path { get; set; }
        public string cacheName { get; set; }
        public List<T> records { get; set; }

        public UpdateCsvCommand(string cacheName, string path, List<T> records)
        {
            this.path = path;
            this.cacheName = cacheName;
            this.records = records;
        }

        public override void execute()
        {

            TextWriter textWriter = new StreamWriter(path);

            CsvWriter writer = new CsvWriter(textWriter);
            writer.Configuration.RegisterClassMap<U>();
            writer.WriteRecords(records);
            textWriter.Close();

        }

        public override void undo()
        {
            throw new NotImplementedException();
        }

    }
}

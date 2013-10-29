using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            try
            {
                using (TextWriter textWriter = new StreamWriter(path))
                using (CsvWriter writer = new CsvWriter(textWriter))
                {
                    writer.Configuration.RegisterClassMap<U>();
                    writer.WriteRecords(records);
                }
            }
            catch (Exception ex)
            {
                if (CsvCachedWebApplication.DEBUG_MODE)
                {
                    Debug.WriteLine("Error executing command: " + ex.StackTrace);
                }
            }

        }

        public override void undo()
        {
            throw new NotImplementedException();
        }

    }
}

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
        where T : IIDedRecord
        where U : IDedRecordClassMap<T>
    {

        public string path { get; set; }
        public string cacheName { get; set; }
        public List<T> records { get; set; }
        public bool writeHeader { get; set; }

        public UpdateCsvCommand(string cacheName, string path, List<T> records, bool writeHeader = true)
        {
            this.path = path;
            this.cacheName = cacheName;
            this.records = records;
            this.writeHeader = writeHeader;
        }

        public override void execute()
        {
            try
            {
                if (records != null)
                {
                    using (TextWriter textWriter = new StreamWriter(path))
                    using (CsvWriter writer = new CsvWriter(textWriter))
                    {
                        writer.Configuration.RegisterClassMap<U>();
                        if (writeHeader)
                        {
                            writer.WriteHeader<T>();
                        }
                        writer.WriteRecords(records);
                    }
                }
                else
                {
                    Debug.WriteLine("records list was null.  Pass an empty list to clear a file. ");
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

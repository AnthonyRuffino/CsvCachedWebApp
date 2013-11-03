using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AsyncCommander;
using CsvCachedWebApp.DbLink;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsvCachedWebApp.Commands
{


    public class UpdateCsvFromDbCommand<T, U> : AbstractCommand
        where T : IIDedRecord
        where U : IDedRecordClassMap<T>
    {

        public string path { get; set; }
        public string cacheName { get; set; }
        public IRecordRetriever<T> recordRetriever { get; set; }
        public bool writeHeader { get; set; }

        public UpdateCsvFromDbCommand(string cacheName, string path, IRecordRetriever<T> recordRetriever, bool writeHeader = true)
        {
            this.path = path;
            this.cacheName = cacheName;
            this.recordRetriever = recordRetriever;
            this.writeHeader = writeHeader;
        }

        public override void execute()
        {
            try
            {
                if (recordRetriever != null)
                {
                    Command updateCsvCommand = new UpdateCsvCommand<T, U>(cacheName, path, recordRetriever.getRecords(), writeHeader);
                    updateCsvCommand.execute();
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

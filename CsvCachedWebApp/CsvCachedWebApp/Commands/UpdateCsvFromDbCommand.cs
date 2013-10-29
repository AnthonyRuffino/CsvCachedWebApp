using System;
using System.Collections.Generic;
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
        where T : IDedRecord
        where U : IDedRecordClassMap<T>
    {

        public string path { get; set; }
        public string cacheName { get; set; }
        public IRecordRetriever<T> recordRetriever { get; set; }

        public UpdateCsvFromDbCommand(string cacheName, string path, IRecordRetriever<T> recordRetriever)
        {
            this.path = path;
            this.cacheName = cacheName;
            this.recordRetriever = recordRetriever;
        }

        public override void execute()
        {
            Command updateCsvCommand = new UpdateCsvCommand<T, U>(cacheName, path, recordRetriever.getRecords());
            updateCsvCommand.execute();
        }

        public override void undo()
        {
            throw new NotImplementedException();
        }

    }
}

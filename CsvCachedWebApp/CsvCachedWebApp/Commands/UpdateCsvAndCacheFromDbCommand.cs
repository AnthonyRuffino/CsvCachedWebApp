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


    public class UpdateCsvAndCacheFromDbCommand<T, U> : AbstractCommand
        where T : IDedRecord
        where U : IDedRecordClassMap<T>
    {

        public string path { get; set; }
        public string cacheName { get; set; }
        public IRecordRetriever<T> recordRetriever { get; set; }
        private HttpApplicationStateBase applicationStateBase { get; set; }

        public UpdateCsvAndCacheFromDbCommand(string cacheName, string path, HttpApplicationStateBase applicationStateBase, IRecordRetriever<T> recordRetriever)
        {
            this.path = path;
            this.cacheName = cacheName;
            this.recordRetriever = recordRetriever;
            this.applicationStateBase = applicationStateBase;
        }

        public override void execute()
        {
            Command updateCsvAndCacheCommand = new UpdateCsvAndCacheCommand<T, U>(cacheName, path, applicationStateBase, recordRetriever.getRecords());
            updateCsvAndCacheCommand.executeAsynchronously();
        }

        public override void undo()
        {
            throw new NotImplementedException();
        }

    }
}

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


    public class UpdateCsvAndCacheFromDbCommand<T, U> : AbstractCommand
        where T : IIDedRecord
        where U : IDedRecordClassMap<T>
    {

        public string path { get; set; }
        public string cacheName { get; set; }
        public IRecordRetriever<T> recordRetriever { get; set; }
        private HttpApplicationStateBase applicationStateBase { get; set; }
        public bool writeHeader { get; set; }
        public bool willThrowOnMissingField { get; set; }

        public UpdateCsvAndCacheFromDbCommand(string cacheName, string path, HttpApplicationStateBase applicationStateBase, IRecordRetriever<T> recordRetriever, bool writeHeader = true, bool willThrowOnMissingField = false)
        {
            this.path = path;
            this.cacheName = cacheName;
            this.recordRetriever = recordRetriever;
            this.applicationStateBase = applicationStateBase;
            this.writeHeader = writeHeader;
            this.willThrowOnMissingField = willThrowOnMissingField;
        }

        public override void execute()
        {
            try
            {
                if (recordRetriever != null)
                {
                    Command updateCsvAndCacheCommand = new UpdateCsvAndCacheCommand<T, U>(cacheName, path, applicationStateBase, recordRetriever.getRecords(), writeHeader, willThrowOnMissingField);
                    updateCsvAndCacheCommand.execute();
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

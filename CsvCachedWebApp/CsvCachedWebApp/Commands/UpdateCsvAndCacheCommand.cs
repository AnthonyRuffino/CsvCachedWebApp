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


    public class UpdateCsvAndCacheCommand<T, U> : AbstractCommand
        where T : IDedRecord
        where U : IDedRecordClassMap<T>
    {

        public string path { get; set; }
        public string cacheName { get; set; }
        public List<T> records { get; set; }

        private HttpApplicationStateBase applicationStateBase { get; set; }

        public UpdateCsvAndCacheCommand(string cacheName, string path, HttpApplicationStateBase applicationStateBase, List<T> records)
        {
            this.path = path;
            this.cacheName = cacheName;
            this.applicationStateBase = applicationStateBase;
            this.records = records;
        }

        public UpdateCsvAndCacheCommand(string cacheName, string path, HttpApplicationState applicationState, List<T> records)
            : this(cacheName, path, new HttpApplicationStateWrapper(applicationState), records)
        {

        }

        public override void execute()
        {
            TextWriter textWriter = new StreamWriter(path);

            CsvWriter writer = new CsvWriter(textWriter);
            writer.Configuration.RegisterClassMap<U>();
            

            Dictionary<string, T> repository = new Dictionary<string, T>();

            foreach (T record in records)
            {
                if (!repository.ContainsKey(record.Id))
                {
                    repository.Add(record.Id, record);
                    writer.WriteRecord(record);
                }
            }

            textWriter.Close();

            applicationStateBase[cacheName] = repository;

        }

        public override void undo()
        {
            applicationStateBase[cacheName] = null;
        }

    }
}

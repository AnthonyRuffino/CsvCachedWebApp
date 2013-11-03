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


    public class UpdateCsvAndCacheCommand<T, U> : AbstractCommand
        where T : IIDedRecord
        where U : IDedRecordClassMap<T>
    {

        public string path { get; set; }
        public string cacheName { get; set; }
        public List<T> records { get; set; }
        public bool writeHeader { get; set; }

        private HttpApplicationStateBase applicationStateBase { get; set; }

        public UpdateCsvAndCacheCommand(string cacheName, string path, HttpApplicationStateBase applicationStateBase, List<T> records, bool writeHeader = true)
        {
            this.path = path;
            this.cacheName = cacheName;
            this.applicationStateBase = applicationStateBase;
            this.records = records;
            this.writeHeader = writeHeader;
        }

        public UpdateCsvAndCacheCommand(string cacheName, string path, HttpApplicationState applicationState, List<T> records)
            : this(cacheName, path, new HttpApplicationStateWrapper(applicationState), records)
        {

        }

        public override void execute()
        {
            try
            {
                if (records != null)
                {
                    Dictionary<string, T> repository = null;

                    using (TextWriter textWriter = new StreamWriter(path))
                    using (CsvWriter writer = new CsvWriter(textWriter))
                    {
                        writer.Configuration.RegisterClassMap<U>();
                        repository = new Dictionary<string, T>();

                        if (writeHeader)
                        {
                            writer.WriteHeader<T>();
                        }

                        foreach (T record in records)
                        {
                            if (record != null && !repository.ContainsKey(record.getId()))
                            {
                                repository.Add(record.getId(), record);
                                writer.WriteRecord(record);
                            }
                        }

                    }

                    if (applicationStateBase != null && cacheName != null)
                    {
                        applicationStateBase[cacheName] = repository;
                    }
                }
                else
                {
                    Debug.WriteLine("records list was null.  Pass an empty list to clear a file and the cache. ");
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
            applicationStateBase[cacheName] = null;
        }

    }
}

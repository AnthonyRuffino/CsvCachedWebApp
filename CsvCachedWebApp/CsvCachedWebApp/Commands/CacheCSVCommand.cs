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


    public class CacheCsvCommand<T, U> : AbstractCommand
        where T : IIDedRecord
        where U : IDedRecordClassMap<T>
    {

        public string path { get; set; }
        public string cacheName { get; set; }

        private HttpApplicationStateBase applicationStateBase { get; set; }

        public CacheCsvCommand(string cacheName, string path, HttpApplicationStateBase applicationStateBase)
        {
            this.path = path;
            this.cacheName = cacheName;
            this.applicationStateBase = applicationStateBase;
        }

        public CacheCsvCommand(string cacheName, string path, HttpApplicationState applicationState)
            : this(cacheName, path, new HttpApplicationStateWrapper(applicationState))
        {

        }

        public override void execute()
        {
            try
            {
                if (cacheName != null && applicationStateBase != null)
                {
                    Dictionary<string, T> repository = null;

                    using (StreamReader streamReader = new StreamReader(path))
                    using (CsvReader csv = new CsvReader(streamReader))
                    {
                        repository = new Dictionary<string, T>();

                        csv.Configuration.RegisterClassMap<U>();

                        while (csv.Read())
                        {
                            T record = csv.GetRecord<T>();

                            if (record != null && !repository.ContainsKey(record.getId()))
                            {
                                repository.Add(record.getId(), record);
                            }

                        }
                    }

                    applicationStateBase[cacheName] = repository;
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

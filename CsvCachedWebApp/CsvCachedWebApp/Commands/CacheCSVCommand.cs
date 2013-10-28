﻿using System;
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


    public class CacheCsvCommand<T, U> : AbstractCommand
        where T : IDedRecord
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
            var csv = new CsvReader(new StreamReader(path));

            csv.Configuration.RegisterClassMap<U>();

            Dictionary<string, T> repository = new Dictionary<string, T>();

            while (csv.Read())
            {
                T record = csv.GetRecord<T>();

                if (!repository.ContainsKey(record.Id))
                {
                    repository.Add(record.Id, record);
                }

            }

            applicationStateBase[cacheName] = repository;

        }

        public override void undo()
        {
            applicationStateBase[cacheName] = null;
        }

    }
}

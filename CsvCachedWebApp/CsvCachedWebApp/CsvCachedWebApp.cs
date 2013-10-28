using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using System.Web.Caching;
using System.Net;
using AsyncCommander;
using System.Configuration;
using CsvCachedWebApp.Scheduler;
using CsvHelper.Configuration;
using CsvCachedWebApp.Commands;
using System.IO;

namespace CsvCachedWebApp
{

    public class CsvCachedWebApp : System.Web.HttpApplication
    {

        public static string CSV_CACHE_DIR_DEFAULT = "CsvCache";
        public static string CSV_CACHE_DIR = ConfigurationManager.AppSettings["CSV_CACHE_DIR"];

        public CachedTaskScheduler CachedTaskScheduler { get; set; }
        public string csvCacheDirectory { get; set; }

        public CsvCachedWebApp() :base()
        {
            this.csvCacheDirectory = CSV_CACHE_DIR != null && CSV_CACHE_DIR.Length > 0 ? CSV_CACHE_DIR : CSV_CACHE_DIR_DEFAULT;
            this.CachedTaskScheduler = new CachedTaskScheduler(this);
        }


        public void StartCachingJob(string cacheJobName, int intervalSeconds, bool executeImmediately, Command command)
        {

            if (executeImmediately)
            {
                command.executeAsynchronously();
            }

            CachedTaskScheduler.AddTask(cacheJobName, intervalSeconds, command);
        }


        public void cacheSimpleRecordsExample()
        {
            string cacheFolder = Server.MapPath("~") + "Content\\" + csvCacheDirectory;

            string rootName = "simpleRecord";
            string fileName = rootName + ".csv";
            string path = cacheFolder + "\\" + fileName + ".csv";
            string cacheName = rootName + "-cache";
            string cacheJobName = rootName + "-cached-job";

            if (File.Exists(path))
            {
                CacheCSVCommand<SimpleRecord, SimpleRecordClassMap> simpleRecordCSVCacheCommand = new CacheCSVCommand<SimpleRecord, SimpleRecordClassMap>(cacheName, path, Application);
                StartCachingJob(cacheJobName, 3600, true, simpleRecordCSVCacheCommand);
            }


            rootName = "anotherSimpleRecord";
            fileName = rootName + ".csv";
            path = cacheFolder + "\\" + fileName + ".csv";
            cacheName = rootName + "-cache";
            cacheJobName = rootName + "-cached-job";

            if (File.Exists(path))
            {
                CacheCSVCommand<AnotherSimpleRecord, AnotherSimpleRecordClassMap> anotherSimpleRecordCSVCacheCommand = new CacheCSVCommand<AnotherSimpleRecord, AnotherSimpleRecordClassMap>(cacheName, path, Application);
                StartCachingJob(cacheJobName, 3600, true, anotherSimpleRecordCSVCacheCommand);
            }
            
        }
        
    }


    
}

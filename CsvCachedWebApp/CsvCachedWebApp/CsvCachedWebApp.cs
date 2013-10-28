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
using CsvHelper;
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
        public string contentDirectory { get; set; }

        public CsvCachedWebApp() :base()
        {
            this.contentDirectory = Server.MapPath("~") + "Content\\";
            this.csvCacheDirectory = CSV_CACHE_DIR != null && CSV_CACHE_DIR.Length > 0 ? CSV_CACHE_DIR : CSV_CACHE_DIR_DEFAULT;
            this.CachedTaskScheduler = new CachedTaskScheduler(this);
        }


        public void StartCachedJob(string cacheJobName, int intervalSeconds, bool executeImmediately, Command command)
        {

            if (executeImmediately)
            {
                command.executeAsynchronously();
            }

            CachedTaskScheduler.AddTask(cacheJobName, intervalSeconds, command);
        }        
        
    }


    
}

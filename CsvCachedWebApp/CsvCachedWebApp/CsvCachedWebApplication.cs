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
using System.Diagnostics;

namespace CsvCachedWebApp
{

    public class CsvCachedWebApplication : System.Web.HttpApplication
    {
        public static string CSV_CACHE_DIR_DEFAULT = "CsvCache";
        public static string CSV_CACHE_DIR = ConfigurationManager.AppSettings["CSV_CACHE_DIR"];

        public static string DEFAULT_CSV_DEBUG_MODE_NAME = "DEBUG_MODE";
        public static string CSV_DEBUG_MODE_NAME = ConfigurationManager.AppSettings["CSV_DEBUG_MODE_NAME"] != null && ConfigurationManager.AppSettings["CSV_DEBUG_MODE_NAME"].ToString().Length > 0 ? ConfigurationManager.AppSettings["CSV_DEBUG_MODE_NAME"].ToString() : DEFAULT_CSV_DEBUG_MODE_NAME;
        public static bool DEBUG_MODE = ConfigurationManager.AppSettings[CSV_DEBUG_MODE_NAME] != null && (ConfigurationManager.AppSettings[CSV_DEBUG_MODE_NAME].ToString().ToLower() == "true" || ConfigurationManager.AppSettings[CSV_DEBUG_MODE_NAME].ToString().ToLower() == "false") ? Boolean.Parse(ConfigurationManager.AppSettings[CSV_DEBUG_MODE_NAME].ToString().ToLower()) : true;

        private CachedTaskScheduler CachedTaskScheduler { get; set; }

        private string CsvCacheDirectory {
            get 
            {
                return GetRootDirectory(Server, "Content") + (CSV_CACHE_DIR != null && CSV_CACHE_DIR.Length > 0 ? CSV_CACHE_DIR : CSV_CACHE_DIR_DEFAULT);
            }
        }

        public CsvCachedWebApplication() :base()
        {
            try
            {
                this.CachedTaskScheduler = new CachedTaskScheduler(this);
            }
            catch (Exception ex)
            {
                if (DEBUG_MODE)
                {
                    Debug.WriteLine("Error initializing CachedTaskScheduler: " + ex.StackTrace);
                }
            }

        }


        public void StartCachedJob(string cacheJobName, int intervalSeconds, bool executeImmediately, Command command)
        {

            if (executeImmediately)
            {
                try
                {
                    command.executeAsynchronously();
                }
                catch(Exception ex)
                {
                    if (DEBUG_MODE)
                    {
                        Debug.WriteLine("Error running Asynchronous command: " + ex.StackTrace);
                    }
                }
            }

            try
            {
                CachedTaskScheduler.AddTask(cacheJobName, intervalSeconds, command);
            }
            catch (Exception ex)
            {
                if (DEBUG_MODE)
                {
                    Debug.WriteLine("Error starting cached job{" + cacheJobName + "}" + ex.StackTrace);
                }
            }
        }

        public static string GetRootDirectory(HttpServerUtility serverUtility, string subDirectory = null)
        {
            return GetRootDirectory(new HttpServerUtilityWrapper(serverUtility), subDirectory);
        }

        public static string GetRootDirectory(HttpServerUtilityBase serverUtilityBase, string subDirectory = null)
        {
            string returnString = null;
            returnString = serverUtilityBase.MapPath("~") + (subDirectory != null ? subDirectory + "\\" : "");
            return returnString;
        }
        
    }


    
}

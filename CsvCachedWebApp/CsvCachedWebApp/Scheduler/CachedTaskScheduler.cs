using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AsyncCommander;

namespace CsvCachedWebApp.Scheduler
{
    public class CachedTaskScheduler : ICachedTaskScheduler
    {

        public static int TASK_SCHEDULED_SUCCESSFULLY = 1;
        public static int TASK_ALREADY_SCHEDULED = 2;
        public static int TASK_SCHEDULE_ERROR = 3;


        protected HttpApplication WebApp { get; set; }
        protected Dictionary<string, Command> scheduledCommands { get; set; }

        public CachedTaskScheduler(HttpApplication WebApp)
        {
            this.WebApp = WebApp;
        }

        public int AddTask(string name, int seconds, Command command, bool overWriteExisting = false)
        {

            int responseCode = TASK_SCHEDULE_ERROR;

            if (command != null)
            {
                if (scheduledCommands == null)
                {
                    scheduledCommands = new Dictionary<string, Command>();
                }

                if (scheduledCommands.ContainsKey(name))
                {
                    if (overWriteExisting)
                    {
                        scheduledCommands[name] = command;
                        responseCode = TASK_SCHEDULED_SUCCESSFULLY;
                    }
                    else
                    {
                        responseCode = TASK_ALREADY_SCHEDULED;
                    }
                }
                else
                {
                    scheduledCommands.Add(name, command);
                    responseCode = TASK_SCHEDULED_SUCCESSFULLY;
                }

                if (responseCode == TASK_SCHEDULED_SUCCESSFULLY)
                {
                    try
                    {
                        scheduleTask(name, seconds);
                    }
                    catch (Exception ex)
                    {
                        if (CsvCachedWebApplication.DEBUG_MODE)
                        {
                            Debug.WriteLine("Error scheduling command {" + name + "}: " + ex.StackTrace);
                            responseCode = TASK_SCHEDULE_ERROR;
                        }
                    }
                }
            }

            return responseCode;
        }


        protected static System.Web.Caching.CacheItemRemovedCallback OnCacheRemove = null;

        protected void scheduleTask(string name, int seconds)
        {

            OnCacheRemove = new System.Web.Caching.CacheItemRemovedCallback(CacheItemRemoved);

            WebApp.Context.Cache.Insert(name, seconds, null,
                DateTime.Now.AddSeconds(seconds), System.Web.Caching.Cache.NoSlidingExpiration,
                System.Web.Caching.CacheItemPriority.NotRemovable, OnCacheRemove);
            
        }


        protected void CacheItemRemoved(string k, object v, System.Web.Caching.CacheItemRemovedReason r)
        {
            if (scheduledCommands != null)
            {
                if (scheduledCommands.ContainsKey(k) && scheduledCommands[k] != null)
                {
                    try
                    {
                        scheduledCommands[k].executeAsynchronously();
                    }
                    finally
                    {
                        try
                        {
                            scheduleTask(k, Convert.ToInt32(v));
                        }
                        catch (Exception ex)
                        {
                            if (CsvCachedWebApplication.DEBUG_MODE)
                            {
                                Debug.WriteLine("Error removing cache item {" + k + "}: " + ex.StackTrace);
                            }
                        }
                    }
                }
            }

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncCommander;

namespace CsvCachedWebApp.Scheduler
{
    public interface ICachedTaskScheduler
    {
        int AddTask(string name, int seconds, Command command, bool overWriteExisting = false);
    }
}

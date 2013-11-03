using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsvCachedWebApp.Commands
{

    public interface IIDedRecord
    {
        string getId();
    }

    public abstract class IDedRecordClassMap<T> : CsvClassMap<T> where T : IIDedRecord
    {

    }

}

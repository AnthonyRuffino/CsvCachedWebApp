using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvCachedWebApp.Commands;

namespace CsvCachedWebApp.DbLink
{
    public interface IRecordRetriever<T>
        where T : IDedRecord
    {
        List<T> getRecords();
    }
}

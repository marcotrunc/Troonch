using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Troonch.Sales.DataAccess.DBMSConfiguration
{
    public enum DBAvailable
    {
        MYSQL,
        SQLSERVER
    }
    public  class DatabaseConfiguration
    {
        public DBAvailable DBSelected { get; set; } = DBAvailable.MYSQL;
    }
}

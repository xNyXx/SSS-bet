using DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public abstract class ServiceBase
    {
        private protected Database _db;
        public ServiceBase(Database db) =>
            _db = db;
    }
}

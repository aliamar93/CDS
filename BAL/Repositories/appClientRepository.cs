using DAL.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Repositories
{
    public class appClientRepository : BaseRepository
    {
        public appClientRepository()
            : base()
        { }

        public appClientRepository(DBEntities contextDB)
            : base(contextDB)
        {
            DBContext = contextDB;
        }

        public tblAppClient authenticateApp(string appId, string appSecret)
        {
            return DBContext.tblAppClients.Where(x => x.AppID == appId && x.AppSecret == appSecret).FirstOrDefault();
        }

    }
}

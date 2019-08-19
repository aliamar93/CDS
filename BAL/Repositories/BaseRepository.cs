using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DBEntities;

namespace BAL.Repositories
{
    public class BaseRepository : IDisposable
    {
        public DBEntities DBContext;
        public BaseRepository()
        {
            DBContext = new DBEntities();
            DBContext.init();
        }

        public BaseRepository(DBEntities ContextDB)
        {
            DBContext = ContextDB;
            DBContext.init();
        }

        public void SaveChanges()
        {
            DBContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await DBContext.SaveChangesAsync();
        }

        //public virtual tblNotification createNotification(int SendTo, int NotificationType, string NotificationUrl, string NotificationText, int createdBy)
        //{
        //    tblNotification noti = new tblNotification();
        //    noti.Created = DateTimeUTC.Now;
        //    noti.CreateBy = createdBy;
        //    noti.SendTo = SendTo;
        //    noti.NotificationDate = DateTimeUTC.Now;
        //    noti.NotificationText = NotificationText;
        //    noti.NotificationType = NotificationType;
        //    noti.Url = NotificationUrl;
        //    DBContext.tblNotifications.Add(noti);
        //    return noti;
        //}

        #region IDisposable Implementation


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DBContext != null)
                {
                    DBContext.Dispose();
                    DBContext = null;

                }
            }

        }

        ~BaseRepository()
        {
            Dispose();
        }


        #endregion
    }
}

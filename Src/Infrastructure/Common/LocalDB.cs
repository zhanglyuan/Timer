using Common.Interfaces;
using Common.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Common
{
    public class LocalDB : ILocalDB
    {
        private SQLiteConnection db;

        private readonly string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Timer.db");
        private const string key = "ScgT4y7GJ9L";
        private const string preKeyAction = "PRAGMA cipher_default_use_hmac = OFF;";
        private const string postKeyAction = "PRAGMA kdf_iter = 128000;";

        public LocalDB()
        {
#if DEBUG
            var options = new SQLiteConnectionString(dbPath, true);
#else
            var options = new SQLiteConnectionString(dbPath, true, key: key,
                   preKeyAction: db => db.Execute(preKeyAction),
                       postKeyAction: db => db.Execute(postKeyAction));
#endif

            db = new SQLiteConnection(options);
            db.CreateTable<TimerInfo>();
        }

        public bool SaveTimer(TimerInfo timer)
        {
            try
            {
                db.InsertOrReplace(timer);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public TimerInfo GetTimer()
        {
            var list = db.Table<TimerInfo>().ToList();

            return list?.FirstOrDefault();
        }
    }
}
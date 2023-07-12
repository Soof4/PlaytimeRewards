using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI.DB;

namespace PlaytimeRewards {
    public class Database {

        public class DatabaseManager {
            private IDbConnection _db;

            public DatabaseManager(IDbConnection db) {
                _db = db;

                var sqlCreator = new SqlTableCreator(db, new SqliteQueryCreator());

                sqlCreator.EnsureTableStructure(new SqlTable("Players",
                    new SqlColumn("Name", MySqlDbType.String) { Primary = true, Unique = true },
                    new SqlColumn("Time", MySqlDbType.Int32)));
            }
           
            /// <exception cref="NullReferenceException"></exception>
            public int GetPlayerTime(string name) {
                using var reader = _db.QueryReader("SELECT * FROM Players WHERE Name = @0", name);
                while (reader.Read()) {
                    return reader.Get<int>("Time");
                }
                throw new NullReferenceException();
            }

            public bool InsertPlayer(string name) {
                return _db.Query("INSERT INTO Players (Name, Time) VALUES (@0, @1)", name, 0) != 0;
            }

            public bool SavePlayer(string name, int time) {
                return _db.Query("UPDATE Players SET Time = @0 WHERE Name = @1", time, name) != 0;
            }
        }
    }
}

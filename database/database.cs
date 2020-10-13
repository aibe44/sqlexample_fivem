using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySqlConnector;

using CitizenFX.Core;

namespace EGRP
{
    class Database : BaseScript
    {
        private static MySqlConnection sqlConnection;
        private static System.Data.ConnectionState sqlState;


        const string sqlServer = "localhost";

        const string sqlDatabase = "db";

        const string sqlUserID = "root";
    
        const string sqlPassword = "root";

        /// <summary>
        /// init connection to db in constructor.
        /// </summary>
        public Database(){ initDb(); }

        async void initDb()
        {
            //Create a new connectionstring
            MySqlConnectionStringBuilder _sb = new MySqlConnectionStringBuilder();
                _sb.Server = sqlServer;
                _sb.Database = sqlDatabase;
                _sb.UserID = sqlUserID;
                _sb.Password = sqlPassword;

            string _cs = _sb.ToString();

            //Open new sql connection
            sqlConnection = new MySqlConnection(_cs);
            await sqlConnection.OpenAsync();

            //Just checking if the connection is valid
            sqlState = sqlConnection.State;
            ConsoleLog.Write(ConsoleState.Warning, $"Database connection -> {sqlState.ToString()}");

            //Mandatory to close connection after each call.
            await sqlConnection.CloseAsync();
        }

        //Example function of how to handle queries
        public static async Task<string> ExecuteQuery (string query)
        {
            string res = string.Empty;

            if (!string.IsNullOrEmpty(query)) {
                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);
                await sqlConnection.OpenAsync();

                try {
                    var v = await cmd.ExecuteScalarAsync();
                    res = (string)v;
                }
                catch { return res; }

                await sqlConnection.CloseAsync();
            }

            await BaseScript.Delay(0); //Mandatory wait
            return res;
        }
    }
}

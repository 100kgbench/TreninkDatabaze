using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;


namespace TreninkDatabaze
{
    class Class1
    {
        static void Main(string[] args)
        {
            using (var connection = new SqliteConnection("Data Source=database.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "CREATE TABLE IF NOT EXISTS highscores (name TEXT, score INT)";
                command.ExecuteNonQuery();
            }
        }
    }
}

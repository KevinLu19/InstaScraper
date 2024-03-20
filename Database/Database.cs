using MySql.Data.MySqlClient;
using MySql.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InstagramWebScrape.Database;
internal class Database
{

    private MySqlConnection _connection = new();

    // ** Remove this conn strign when deploying. **
    private string _conn_string = "";

	public Database()
    {
        // Connect to the mysql database.
        string db_username = Environment.GetEnvironmentVariable("DATABASE_USERNAME");
        string db_password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");

        Console.WriteLine($"{db_username}");
        Console.WriteLine($"{db_password}");

        MySqlConnection conn = new MySqlConnection(_conn_string);
        _connection = conn;

        // Test connect to the database.
        try
        {
            _connection.Open();
            Console.WriteLine("Successfully connect to database");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        _connection.Close();

    }

    // Create insta username if table does not already exist.
    public void CreateUserTable()
    {

        MySqlConnection conn = new(_conn_string);

        try
        {
            conn.Open();

            string sql = "CREATE TABLE IF NOT EXISTS InstaUsername (Id INT NOT NULL AUTO_INCREMENT, Name VARCHAR(255) NOT NULL, PRIMARY KEY (Id))";

            MySqlCommand create_table = new MySqlCommand(sql, conn);
            create_table.ExecuteNonQuery();

            Console.WriteLine("Created InstaUsername table");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public void StoreInstaUsernames(string username)
    {
        List<string> username_list = new();
        username_list.Add(username);

        foreach (var item in username_list)
        {
            Console.WriteLine(item);
        }

        MySqlConnection conn = new(_conn_string);

        string sql = "INSERT IGNORE INTO InstaUsername (Name) SELECT @Name";


        using (MySqlCommand insert_username = new MySqlCommand(sql, conn))
        {
            conn.Open();

			insert_username.Parameters.AddWithValue("@Name", username);
			insert_username.ExecuteNonQuery();
			Console.WriteLine($"Inserted {username} into username table");

		}

        conn.Close();
    }

    public void GetInstaUsernames(string username)
    {
		string sql = "SELECT name FROM InstaUsername WHERE Name= @Name";

        MySqlConnection conn = new(_conn_string);

        using (MySqlCommand fetch_username = new(sql, conn))
        {
            conn.Open();

            fetch_username.Parameters.AddWithValue("@Name", username);
            fetch_username.ExecuteNonQuery();
            Console.WriteLine(username);
        }

    }

    public void PrintUsername()
    {
		string sql = "SELECT * FROM InstaUsername";

        MySqlConnection conn = new(_conn_string);

        MySqlCommand print_user = new(sql, conn);
		conn.Open();


        // Print everything from table.
		using (MySqlDataReader reader = print_user.ExecuteReader())
        { 
			while (reader.Read())
            {
                Console.WriteLine(reader["Name"]);
            }
			
        }
    }
}

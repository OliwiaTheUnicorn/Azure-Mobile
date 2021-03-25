using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Cdv.People
{
    public class DatabaseContext
    {
        private readonly string connectionString;
        private const string Query = "Select * from people";

        public DatabaseContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IEnumerable<Person> GetPeople()
        {
            var people = new List<Person>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(Query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    people.Add(new Person
                    {
                        PersonId = Convert.ToInt32(reader["PersonId"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Phonenumber = reader["Phonenumber"].ToString()
                    });
                }
                reader.Close();
            }

            return people;
        }

        public Person GetPeopleById(int id)
        {
            Person person = new Person();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql =  "Select * from people where PersonId = @param1";
                connection.Open();
                using(SqlCommand cmd = new SqlCommand(sql,connection)) 
                {
                    cmd.Parameters.AddWithValue("@param1", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        person.PersonId = Convert.ToInt32(reader["PersonId"]);
                        person.FirstName = reader["FirstName"].ToString();
                        person.LastName = reader["LastName"].ToString();
                        person.Phonenumber = reader["Phonenumber"].ToString();
                        
                    }
                    reader.Close();
                    if (connection.State == System.Data.ConnectionState.Open) 
                        connection.Close();

                }
            }

            return person;
        }

        public int AddPeople(String firstName, String lastName, String phoneNumber)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql =  "INSERT INTO People(FirstName, LastName, PhoneNumber) output INSERTED.PersonId VALUES(@param1,@param2,@param3)";
                using(SqlCommand cmd = new SqlCommand(sql,connection)) 
                {
                    cmd.Parameters.AddWithValue("@param1", firstName);  
                    cmd.Parameters.AddWithValue("@param2", lastName);
                    cmd.Parameters.AddWithValue("@param3", phoneNumber);
                    //cmd.CommandType = CommandType.Text;
                    int modified =(int)cmd.ExecuteScalar();

                    if (connection.State == System.Data.ConnectionState.Open) 
                        connection.Close();

                    return modified;
                }
            }
        }

        public void DeletePeople(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql =  "DELETE FROM People WHERE PersonId = @param1";
                using(SqlCommand cmd = new SqlCommand(sql,connection)) 
                {
                    cmd.Parameters.AddWithValue("@param1", id);
                    cmd.ExecuteNonQuery();

                    if (connection.State == System.Data.ConnectionState.Open) 
                        connection.Close();
                }
            }
        }


    }
}
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace TestServer.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private string connectingString = "Host=localhost;Username=postgres;Password=12345;Database=testdb";

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Person> people = new List<Person>();
            using var con = new NpgsqlConnection(connectingString);
            con.Open();

            string sql = "SELECT * FROM person";
            using var cmd = new NpgsqlCommand(sql, con);

            using NpgsqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                people.Add(new Person { Name = rdr.GetString(0), Age = rdr.GetInt32(1) });
            }

            return Ok(people);
        }

        [HttpGet("name/{name}")]
        public IActionResult GetByName(string name)
        {
            List<Person> people = new List<Person>();
            using var con = new NpgsqlConnection(connectingString);
            con.Open();

            string sql = "SELECT * FROM person WHERE name = :name";
            using var cmd = new NpgsqlCommand(sql, con);

            cmd.Parameters.AddWithValue("name", name);

            using NpgsqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                people.Add(new Person { Name = rdr.GetString(0), Age = rdr.GetInt32(1) });
            }
            if (people.Count == 0)
            {
                return NotFound();
            }
            return Ok(people);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Person newPerson)
        {
            using var con = new NpgsqlConnection(connectingString);
            con.Open();

            string sql = "INSERT INTO person (name, age) VALUES (@name, @age)";
            using var cmd = new NpgsqlCommand(sql, con);

            cmd.Parameters.AddWithValue("name", newPerson.Name);
            cmd.Parameters.AddWithValue("age", newPerson.Age);

            cmd.ExecuteNonQuery();

            return Ok();
        }

        [HttpDelete("name/{name}")]
        public IActionResult DeleteByName(string name)
        {
            using var con = new NpgsqlConnection(connectingString);
            con.Open();

            string sql = "DELETE FROM person WHERE name = @name";
            using var cmd = new NpgsqlCommand(sql, con);

            cmd.Parameters.AddWithValue("name", name);

            cmd.ExecuteNonQuery();

            return Ok();
        }

        [HttpDelete("age/{age}")]
        public IActionResult DeleteByAge(string age)
        {
            using var con = new NpgsqlConnection(connectingString);
            con.Open();

            string sql = "DELETE FROM person WHERE age = @age";
            using var cmd = new NpgsqlCommand(sql, con);

            cmd.Parameters.AddWithValue("name", age);

            cmd.ExecuteNonQuery();

            return Ok();
        }
    }
}

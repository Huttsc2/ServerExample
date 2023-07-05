using System.Text.Json.Serialization;

namespace TestServer
{
    public class Person
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }
    }
}

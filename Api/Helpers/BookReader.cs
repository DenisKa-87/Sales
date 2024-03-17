using Newtonsoft.Json;
using Api.Entities;
using System.Text.Json.Serialization;

namespace Api.Helpers
{
    public class BookReader
    {
        private readonly string _path;

        public BookReader(string path)
        {
            _path = path;
        }

        public List<Book> ReadBooksfromFile()
        {
            using var reader = new StreamReader(_path);
            var json = reader.ReadToEnd();
            List<Book> books = JsonConvert.DeserializeObject<List<Book>>(json);
            return books;
        }
    }
}

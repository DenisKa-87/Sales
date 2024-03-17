using Microsoft.AspNetCore.Mvc;
using Api.Helpers;
using Api.Interfaces;

namespace Api.Data
{
    public class SeedBooks
    {
        public static async Task<bool> Seed(IUnitOfWork unitOfWork, string path)
        {
            var reader = new BookReader(path);
            var books = reader.ReadBooksfromFile();
            var result = true;
            foreach (var book in books)
            {
                if (!await unitOfWork.BookRepository.AddBook(book))
                {
                    result = false;
                    break;
                } 
            }
            if(result)
            {
                 return await unitOfWork.Complete();
            }
            return result;
        }
    }
}

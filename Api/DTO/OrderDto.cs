using salesAPI.Entities;

namespace Api.DTO
{
    public class OrderDto
    {
        public int Id { get; set; }
        public List<BookDto> Books { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }

        public static OrderDto Create(Order order)
        {
            var orderDto = new OrderDto
            {
                Id = order.Id,
                UserName = order.User.UserName,
                CreatedAt = order.CreatedAt,
                Books = new List<BookDto>()
            };
            if (order.Books != null)
                foreach (var book in order.Books)
                    orderDto.Books.Add(BookDto.Create(book));
            return orderDto;
        }


    }


}

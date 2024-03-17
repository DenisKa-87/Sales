using Api.Entities;

namespace Api.DTO
{
    public class OrderDto
    {
        public int Id { get; set; }
        public List<BookDto> Books { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Placed { get; set; }
        public bool Processed { get; set; }
        public string OrderUrl { get; set; } = null;

        public static OrderDto Create(Order order)
        {
            var orderDto = new OrderDto
            {
                Id = order.Id,
                UserName = order.User.UserName,
                CreatedAt = order.CreatedAt,
                Books = new List<BookDto>(),
                Placed = order.Placed,
                Processed = order.Processed,
                OrderUrl = order.OrderUrl

            };
            if (order.Books != null)
                foreach (var book in order.Books)
                    orderDto.Books.Add(BookDto.Create(book));
            return orderDto;
        }


    }


}

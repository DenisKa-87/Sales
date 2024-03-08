using salesAPI.Entities;

namespace Api.DTO
{
    /// <summary>
    /// An entity to pass data to the client;
    /// </summary>
    public class OrdersResponseDto
    {
        public string Promo { get; set; }
        public List<long> Isbns { get; set; }

        public static OrdersResponseDto Create(Order order)
        {
            var orderDto = new OrdersResponseDto()
            {
                Promo = order.User.UserName,
                Isbns = new List<long>()
            };
            foreach (var item in order.Books)
            {
                orderDto.Isbns.Add(item.Isbn);
            }
            return orderDto;

        }
    }
}

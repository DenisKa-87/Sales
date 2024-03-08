using salesAPI.Entities;

namespace Api.DTO
{
    public class OrdersResponsetDto
    {
        public string Promo { get; set; }
        public List<long> Isbns { get; set; }

        public static OrdersResponsetDto Create(Order order)
        {
            var orderDto = new OrdersResponsetDto()
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

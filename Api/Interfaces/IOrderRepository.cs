using Api.Entities;

namespace Api.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<IEnumerable<Order>> GetActiveOrders(int quantity);
        Task<bool> PlaceCurrentOrder(AppUser user);
        Task<Order> GetOrderById(int id);
        void AddOrder(Order order);
        void DeleteOrder(Order order);
        void UpdateOrder(Order order);
        Task<Order> GetUsersOrder(AppUser appUser);
        Task<Order> GetCurrentOrder(AppUser user);
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Entities;
using Api.Interfaces;

namespace Api.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context)
        {
            _context = context;
        }
        

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);

        }

        public void DeleteOrder(Order order)
        {
            _context.Orders.Remove(order);
        }


        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetUserOrders(AppUser appUser)
        {
            return await _context.Orders.Include(x => x.Books).Where(order =>  
            order.User.NormalizedUserName == appUser.NormalizedUserName).ToListAsync();

        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Attach(order);
            _context.Entry(order).State = EntityState.Modified;
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            return order;
        }

        public async Task<Order> GetCurrentOrder(AppUser user)
        {
            var orders = await GetUserOrders(user);
            var order = orders.FirstOrDefault(order => !order.Placed);
            if(order == null)
            {
                order = Order.Create(user);
                AddOrder(order);
            }
            return order;
        }

        public async Task<IEnumerable<Order>> GetActiveOrders(int quantity)
        {
            var orders = await _context.Orders.Include(x => x.User).Include(x => x.Books).Where(x => x.Placed).OrderBy(x => x.PlacedAt).Take(quantity).ToListAsync();
            return orders;
        }

        public async Task<bool> PlaceCurrentOrder(AppUser user)
        {
            var currentOrder = await GetCurrentOrder(user);
            if (currentOrder.Books.Sum(book => book.Price) < 2000)
                return false;  
            currentOrder.Placed = true;
            currentOrder.PlacedAt = DateTime.Now;
            _context.Entry(currentOrder).State = EntityState.Modified;
            return true;
        }
    }
}

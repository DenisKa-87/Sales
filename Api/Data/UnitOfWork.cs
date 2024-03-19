using Api.Interfaces;
using Api.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Api.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IHubContext<BookHub> _bookHub;
        private readonly IHubContext<UserNotificationHub> _usernotificationHub;

        public UnitOfWork(DataContext context, IHubContext<BookHub> bookHub)
        {
            _context = context;
            _bookHub = bookHub;
        }
        public IOrderRepository OrderRepository => new OrderRepository(_context);

        public IBookRepository BookRepository => new BookRepository(_context, _bookHub);


        public async Task<bool> Complete()
        {
           return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}

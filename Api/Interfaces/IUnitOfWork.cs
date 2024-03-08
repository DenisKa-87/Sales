namespace Api.Interfaces
{
    public interface IUnitOfWork
    {
        public IOrderRepository OrderRepository { get; }
        public IBookRepository BookRepository { get; }

        bool HasChanges();
        //Returns true, if changes have been saved in the database
        Task<bool> Complete();
    }


}

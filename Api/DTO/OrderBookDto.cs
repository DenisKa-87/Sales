namespace Api.DTO
{
    public class OrderBookDto
    {
        /// <summary>
        /// An entity to pass to the main site for order confirmation.
        /// </summary>
        public long Isbn { get; set; }
        public int OrderId { get; set; }
    }
}

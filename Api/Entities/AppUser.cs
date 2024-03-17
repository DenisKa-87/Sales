using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Entities
{
    public class AppUser : IdentityUser
    {
        [ForeignKey("Order")]
        public int OrderId { get; set; }    
        public Order Order { get; set; }

        public AppUser()
        {

            Order = Order.Create(this);
            OrderId = Order.Id;
        }
    }
}

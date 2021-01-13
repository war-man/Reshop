using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Reshop.Domain.Models.User.Cart
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        public bool IsFinally { get; set; }

        public Identity.User User { get; set; }
        public IList<OrderDetail> OrderDetails { get; set; }
    }
}

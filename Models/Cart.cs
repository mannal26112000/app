using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Estorenew.Models
{
    public class Cart
    {
        public Cart()
        {
            items = new List<Items>();
        }
        public int Id { get; set; }
        //[ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        //public Customer customer { get; set; }
        //public int? CustomerId { get; set; }
        public IList<Items> items { get; set; }
    }
}
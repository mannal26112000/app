using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Estorenew.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int ItemId { get; set; }
        public Items Item { get; set; }
    }
}
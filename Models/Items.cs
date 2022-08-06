using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Estorenew.Models
{
    public class Items
    {
        public Items()
        {
            Carts = new List<Cart> ();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int ItemInStock { get; set; }
        public int Price { get; set; }
        public IList<Cart> Carts { get; set; }
    }
}
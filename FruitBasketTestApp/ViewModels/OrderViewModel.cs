using System.Collections.Generic;

namespace FruitBasketTestApp.Models
{
    public class CompletedOrderViewModel
    {

            public List<OrderViewModel> products { get; set; }
            public decimal SubTotal { get; set; }

    }

    public class OrderViewModel
    {
        //o Fruit(s) and quantity
        //o Subtotal
        //o Order Total
        //o Name
        //o Shipping Address

        public string Fruit { get; set; }
        public int Quantity { get; set; }


        public string Name { get; set; }
        public string ShippingAddress { get; set; }

        public decimal SubTotal { get; set; }
        public decimal ItemTotal { get; set; }



    }
}
using System.Collections.Generic;
using FruitBasketTestApp.Models;

namespace FruitBasketTestApp.ViewModels
{
    public class BasketViewModel
    {
       
        public List<Basket> BasketItems { get; set; }
        public decimal BasketTotal { get; set; }

        public int BasketCount { get; set; }
        public int ItemCount { get; set; }
        public string Message { get; set; }
        public int DeleteId { get; set; }
    }
}
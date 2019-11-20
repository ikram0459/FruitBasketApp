using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FruitBasketTestApp.Models
{
    public partial class BasketManager
    {
        private FruitBasketDbEntities _dbContext = new FruitBasketDbEntities();

        string BasketManagerId { get; set; }
        public const string BasketSessionKey = "CartId";
        public const int MaxWeight = 3; //Max weight 3kg

        /// <summary>
        /// GetCart
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static BasketManager GetCart(HttpContextBase context)
        {
            var basket = new BasketManager();
            basket.BasketManagerId = basket.GetCartId(context);
            return basket;
        }


        /// <summary>
        /// AddToCart
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int AddToBasket(Product item)
        {

            // Get the matching item in he basket if already added so we can increment
            Basket basketItem = _dbContext.Baskets.SingleOrDefault(
               c => c.BasketId == BasketManagerId
                && c.ProductId == item.ProductId);

            if (basketItem == null)
            {
                // Create a new basket item if no basket item exists
                basketItem = new Basket
                {
                    ProductId = item.ProductId,
                    BasketId = BasketManagerId, //this will either be the logged in persons email if they have logged in or a random guid
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                _dbContext.Baskets.Add(basketItem);
            }
            else
            {
                // If the item already exists in the basket, 
                // then add one to the quantity
                basketItem.Count++;
            }
            // Save changes
            _dbContext.SaveChanges();

            return basketItem.Count;


        }

        /// <summary>
        /// CheckReachedMaxWeight
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CheckReachedMaxWeight(Product item)
        {
            decimal? totalexistingWeight = 0;
            var productsAndWeights = (from basketItem in _dbContext.Baskets
                              where basketItem.BasketId == BasketManagerId
                              select new
                              {
                                  basketItem.Count,
                                  basketItem.Product.Weight 

                              });

           foreach (var product in productsAndWeights)
            {
                totalexistingWeight += (product.Count * product.Weight);
             
            }
 
            var newtotal = totalexistingWeight + item.Weight;
            return newtotal > MaxWeight ? true : false;
        }


        /// <summary>
        /// RemoveFromCart
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int RemoveFromCart(int id)
        {
            // Get the cart
            var basketItem = _dbContext.Baskets.FirstOrDefault(
                basket => basket.BasketId == BasketManagerId
                && basket.ProductId == id);


            int itemCount = 0;

            if (basketItem != null)
            {
                if (basketItem.Count > 1)
                {
                    basketItem.Count--;
                    itemCount = basketItem.Count;
                }
                else
                {
                    _dbContext.Baskets.Remove(basketItem);
                }
                // Save changes
                _dbContext.SaveChanges();
            }
            return itemCount;
        }

        /// <summary>
        /// EmptyBasket
        /// </summary>
        public void EmptyBasket()
        {
            var basketItems = _dbContext.Baskets.Where(
                basket => basket.BasketId == BasketManagerId);

            foreach (var basketItem in basketItems)
            {
                _dbContext.Baskets.Remove(basketItem);
            }
            // Save changes
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// GetCartItems
        /// </summary>
        /// <returns></returns>
        public List<Basket> GetCartItems()
        {
            return _dbContext.Baskets.Where(
                b => b.BasketId == BasketManagerId).ToList();
        }

        /// <summary>
        /// GetCount
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            // Get the count of each item in the basket and sum them up
            int? count = (from basketItems in _dbContext.Baskets
                          where basketItems.BasketId == BasketManagerId
                          select (int?)basketItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }

        /// <summary>
        /// GetTotal
        /// </summary>
        /// <returns></returns>
        public decimal GetTotal()
        {
            // Multiply item price by count of that item to get 
            // the current price for each of those items in the cart
            // sum all item price totals to get the cart total
            decimal? total = (from basketItem in _dbContext.Baskets
                              where basketItem.BasketId == BasketManagerId
                              select (int?)basketItem.Count *
                              basketItem.Product.Price).Sum();

            return total ?? decimal.Zero;
        }

        /// <summary>
        /// CreateOrder
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public Order CreateOrder(Order order)
        {
            decimal orderTotal = 0;
             // For our list of order details 
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            var basketItem = GetCartItems();

            // Iterate over the items in the basket, 
            // adding the order details for each
            foreach (var item in basketItem)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Product.Price,
                    Quantity = item.Count
                };
                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.Product.Price);
                order.OrderDetails.Add(orderDetail);
                orderDetails.Add(orderDetail);
                

            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;
            //Add order dets to order obj
            order.OrderDetails = orderDetails;
            _dbContext.Orders.Add(order);

            // Save the order
            _dbContext.SaveChanges();

            
            // Empty the basket
            EmptyBasket();

            // Return the OrderId as the confirmation number
            return order;
        }

        /// <summary>
        /// GetCartId
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[BasketSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[BasketSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[BasketSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[BasketSessionKey].ToString();
        }
    }

    
}
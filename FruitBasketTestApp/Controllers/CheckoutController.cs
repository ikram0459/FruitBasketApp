using System;
using System.Linq;
using System.Web.Mvc;
using FruitBasketTestApp.Models;

namespace FruitBasketTestApp.Controllers
{
    public class CheckoutController : Controller
    {
        //instance of context, to do extract to reporitory
        private FruitBasketDbEntities _dbContext = new FruitBasketDbEntities();

        /// <summary>
        /// AddressAndPayment
        /// </summary>
        /// <returns></returns>
        public ActionResult AddressAndPayment()
        {
            var previousOrder = _dbContext.Orders.FirstOrDefault(x => x.Username == User.Identity.Name);

            if (previousOrder != null)
                return View(previousOrder);
            else
                return View();
        }

        /// <summary>
        /// /Checkout/AddressAndPayment
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            if (ModelState.IsValid)
            {

                var order = new Order();
                TryUpdateModel(order);

                order.Username = User.Identity.Name;
                order.Email = User.Identity.Name;
                order.OrderDate = DateTime.Now;

                //Create the order
                var basket = BasketManager.GetCart(this.HttpContext);
                order = basket.CreateOrder(order);

                return RedirectToAction("Complete",
                    new { id = order.OrderId });

            }
            { return View(values); }

        }

        /// <summary>
        /// /Checkout/Complete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            //// Validate customer owns this order
            bool isValid = _dbContext.Orders.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                var orders = (from o in _dbContext.Orders
                              join od in _dbContext.OrderDetails on o.OrderId equals od.OrderId
                              join p in _dbContext.Products on od.ProductId equals p.ProductId
                              where o.OrderId == id
                              select new OrderViewModel
                              {
                                  Fruit = od.Product.Name,
                                  Quantity = od.Quantity,
                                  ShippingAddress = o.Address,
                                  Name = o.FirstName + o.LastName,
                                  ItemTotal = od.Quantity *p.Price
                              }).ToList();

                //Set the overall sub total for the order
                var orderTotal = _dbContext.Orders.Where(o => o.OrderId == id).Select(x => x.Total).FirstOrDefault();
                var results = new CompletedOrderViewModel
                {
                    products = orders,
                    SubTotal = orderTotal


                };


                return View(results);
            }
            else
            {
                return View("Error");
            }


           
        }
    }
}
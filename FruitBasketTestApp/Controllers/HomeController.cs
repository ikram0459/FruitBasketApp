using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FruitBasketTestApp.Models;

namespace FruitBasketTestApp.Controllers
{
    public class HomeController : Controller
    {
        //instance of context, to do extract to reporitory
        private FruitBasketDbEntities _dbContext = new FruitBasketDbEntities();

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        // GET: products
        public ActionResult Index(bool? overweight)
        {
            var products = from i in _dbContext.Products
                           select i;

            products = products.OrderBy(s => s.Name);

            if (overweight == true)
                ModelState.AddModelError("Products.Weight", "Max basket weight is 3kg. Go to Basket Summary to remove products.");
            return View(products);
        }

        /// <summary>
        /// Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product item =  _dbContext.Products.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
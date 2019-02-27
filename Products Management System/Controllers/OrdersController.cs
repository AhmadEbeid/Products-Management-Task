using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Products_Management_System.Models;

namespace Products_Management_System.Controllers
{
    public class OrdersController : Controller
    {

        private Entities db = new Entities();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.SelectAllOrders();
            var products = db.SelectProductsCanBeBought();

            List<SelectListItem> ProductsList = new List<SelectListItem>();

            foreach (var item in products)
            {
                ProductsList.Add(new SelectListItem() { Text = item.ProductName, Value = item.ProductID.ToString() });
            }

            ViewBag.products = ProductsList;
            ViewBag.ModalTogel = -1;
            ViewBag.newProduct = new Order();

            return View(orders.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "CustomerName,UnitsOrdered,ProductID", Prefix = "modelAdd")] Order order)
        {
            var stockUnits = db.GetProductStock(order.ProductID).First().GetValueOrDefault(0);
            if(stockUnits < order.UnitsOrdered)
            {
                ModelState.AddModelError("modelAdd.UnitsOrdered", "The Units ordered should be equal to or less than " + stockUnits.ToString());
            }else if(order.UnitsOrdered <= 0)
            {
                ModelState.AddModelError("modelAdd.UnitsOrdered", "The Units ordered shouldn't be equal or less than zero");
            }
            if (ModelState.IsValid)
            {
                db.MakeOrder(order.ProductID, order.CustomerName, order.UnitsOrdered);
                return RedirectToAction("Index");
            }

            var orders = db.SelectAllOrders();
            var products = db.SelectProductsCanBeBought();

            List<SelectListItem> ProductsList = new List<SelectListItem>();

            foreach (var item in products)
            {
                ProductsList.Add(new SelectListItem() { Text = item.ProductName, Value = item.ProductID.ToString() });
            }

            ViewBag.products = ProductsList;
            ViewBag.ModalTogel = -100;
            ViewBag.newProduct = new Order();

            return View("Index", orders.ToList());
        }
        
    }
}
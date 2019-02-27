using Products_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Products_Management_System.Controllers
{
    public class SearchController : Controller
    {
        private Entities db = new Entities();
        // GET: Search
        public ActionResult Index(string searchParam)
        {

            ViewBag.orders = db.SelectSomeOrders(searchParam).ToList();
            ViewBag.products = db.SelectSomeProducts(searchParam).ToList();
            ViewBag.suppliers = db.SelectSomeSuppliers(searchParam).ToList();

            return View();
        }
    }
}
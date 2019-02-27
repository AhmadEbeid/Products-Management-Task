using Products_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Products_Management_System.Controllers
{
    public class StatisticsController : Controller
    {
        private Entities db = new Entities();

        // GET: Statistics
        public ActionResult Index()
        {

            ViewBag.reOrderProducts = db.SelectAllProductsNeedsReOrder().ToList();
            var largestSupplier = db.SelectLargestSupplier().ToList();
            var minOrderProductName = db.SelectProductWithMinimumOrder().ToList();
            

            if (largestSupplier.Count() != 0)
            {
                ViewBag.largestSupplierName = largestSupplier[0].SupplierName;
            }
            else
            {
                ViewBag.largestSupplierName = "";
            }

            if (minOrderProductName.Count() != 0)
            {
                ViewBag.minOrderProductName = minOrderProductName.ElementAt(0).ProductName;
            }
            else
            {
                ViewBag.minOrderProductName = "";
            }

            
            return View();
        }

        public ActionResult Search(string searchParam, string tableToSearch)
        {
            ViewBag.reOrderProducts = db.SelectAllProductsNeedsReOrder();
            ViewBag.largestSupplierName = db.SelectLargestSupplier().ElementAt(0).SupplierName;
            ViewBag.minOrderProductName = db.SelectProductWithMinimumOrder().ElementAt(0).ProductName;

            return View("Index");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Products_Management_System.Models;

namespace Products_Management_System.Controllers
{
    public class ProductsController : Controller
    {

        string[] list = { "Kilo", "Box", "Can", "Liter", "Bottle" };
        private Entities db = new Entities();
        

        // GET: Products
        public ActionResult Index()
        {
            var products = db.SelectAllProducts();
            var suppliers = db.SelectAllSuppliers();

            List<SelectListItem> SuppliersList = new List<SelectListItem>();

            foreach (var item in suppliers){
                SuppliersList.Add(new SelectListItem() { Text = item.SupplierName, Value = item.SupplierID.ToString() });
            }

            ViewBag.suppliers = SuppliersList;
            ViewBag.ModalTogel = -1;
            ViewBag.newProduct = new Product();
            ViewBag.QuantityList = list;
            
            return View(products.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "ProductID,ProductName,QuantityPerUnit,ReorderLevel,SupplierID,UnitPrice,UnitsInStock,UnitsOnOrder", Prefix = "Item")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.UpdateProduct(product.ProductID, product.ProductName, product.QuantityPerUnit, product.ReorderLevel, product.SupplierID
                    , product.UnitPrice, product.UnitsInStock, product.UnitsOnOrder);
                return RedirectToAction("Index");
            }

            List<SelectListItem> SuppliersList = new List<SelectListItem>();

            foreach (var item in db.SelectAllSuppliers())
            {
                SuppliersList.Add(new SelectListItem() { Text = item.SupplierName, Value = item.SupplierID.ToString() });
            }
            
            ViewBag.suppliers = SuppliersList;
            ViewBag.ModalTogel = int.Parse(Request["l"]);
            ViewBag.QuantityList = list;
            var products = db.SelectAllProducts();

            return View(products.ToList());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "ProductID,ProductName,QuantityPerUnit,ReorderLevel,SupplierID,UnitPrice,UnitsInStock,UnitsOnOrder", Prefix = "modelAdd")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.InsertProduct(product.ProductName, product.QuantityPerUnit, product.ReorderLevel, product.SupplierID,
                    product.UnitPrice, product.UnitsInStock, product.UnitsOnOrder);
                return RedirectToAction("Index");
            }

            List<SelectListItem> SuppliersList = new List<SelectListItem>();

            foreach (var item in db.SelectAllSuppliers())
            {
                SuppliersList.Add(new SelectListItem() { Text = item.SupplierName, Value = item.SupplierID.ToString() });
            }

            ViewBag.suppliers = SuppliersList;
            ViewBag.ModalTogel = -100;

            var products = db.SelectAllProducts();
            ViewBag.QuantityList = list;
            return View("Index", products.ToList());
        }

        public ActionResult Delete(int productID)
        {
            db.DeleteProduct(productID);
            return RedirectToAction("Index");
        }


    }
}
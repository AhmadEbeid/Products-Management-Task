using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Products_Management_System.Models;

namespace Products_Management_System.Controllers
{
    public class SuppliersController : Controller
    {
        private Entities db = new Entities();

        // GET: Suppliers
        public ActionResult Index()
        {
            var suppliers = db.SelectAllSuppliers();
            ViewBag.ModalTogel = -1;

            ViewBag.newSupplier = new SelectAllSuppliers_Result();

            return View(suppliers.ToList());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "SupplierID,SupplierName", Prefix = "Item")] SelectAllSuppliers_Result supplier)
        {
            if (ModelState.IsValid)
            {
                db.UpdateSupplier(supplier.SupplierID, supplier.SupplierName);
                return RedirectToAction("Index");
            }
            
            ViewBag.ModalTogel = int.Parse(Request["l"]);

            var suppliers = db.SelectAllSuppliers();

            return View(suppliers.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "SupplierID,SupplierName", Prefix = "modelAdd")] SelectAllSuppliers_Result supplier)
        {
            if (ModelState.IsValid)
            {
                db.InsertSupplier(supplier.SupplierName);
                return RedirectToAction("Index");
            }
            
            ViewBag.ModalTogel = -100;
            var suppliers = db.SelectAllSuppliers();

            return View("Index", suppliers.ToList());
        }

        public ActionResult Delete(int SupplierID)
        {
            db.DeleteSupplier(SupplierID);
            return RedirectToAction("Index");
        }

    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Products_Management_System.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.Orders = new HashSet<Order>();
        }
        public int ProductID { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required, RegularExpression("(Kilo)|(Box)|(Can)|(Liter)|(Bottle)", ErrorMessage = "This should be Kilo, Box, Can, Kiter or Bottle only")]
        public string QuantityPerUnit { get; set; }
        [Required, Range(1, 1000, ErrorMessage = "Reorder Level must be between 1 and 1000")]
        public int ReorderLevel { get; set; }
        [Required]
        public Nullable<int> SupplierID { get; set; }
        [Required, Range(0.01, 10000.00, ErrorMessage = "Unit Price must be between 0.01 and 10000.00")]
        public decimal UnitPrice { get; set; }
        [Required, Range(0, 10000, ErrorMessage = "Units In Stock must be between 0 and 10000")]
        public int UnitsInStock { get; set; }
        [Required, Range(1, 10000, ErrorMessage = "Units On Order must be between 1 and 10000")]
        public int UnitsOnOrder { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}

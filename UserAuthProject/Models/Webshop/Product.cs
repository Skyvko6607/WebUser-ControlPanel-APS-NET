using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace UserAuthProject.Models.Webshop
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Category { get; set; }
        public int Price { get; set; }
        public int ShippingDays { get; set; }
        public int MainImageIndex { get; set; }
    }
}
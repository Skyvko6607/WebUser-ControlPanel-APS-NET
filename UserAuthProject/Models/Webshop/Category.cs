using System;
using System.ComponentModel.DataAnnotations;

namespace UserAuthProject.Models.Webshop
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ParentCategory { get; set; }
    }
}

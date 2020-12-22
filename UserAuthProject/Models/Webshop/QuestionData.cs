using System;
using System.ComponentModel.DataAnnotations;

namespace UserAuthProject.Models.Webshop
{
    public class QuestionData
    {
        [Key]
        public Guid Id { get; set; }
        public string Question { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public User.User User { get; set; }
        public Product Product { get; set; }
    }
}

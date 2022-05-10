using FPTBook.Areas.Identity.Data;

namespace FPTBook.Models
{
     public class Cart
        {
        public string UId { get; set; }
        public string BookIsbn { get; set; }
        public int Quantity { get; set; }
        public FPTBookUser? User { get; set; }
        public Book? Book { get; set; }
        
    }

}
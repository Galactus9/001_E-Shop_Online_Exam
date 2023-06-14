using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShopOnlineExam.Models
{
    public class Order
    {
       
        public int Id { get; set; }
        [DisplayName("Order Id")]
        public string OrderId { get; set; }     
        public virtual Certificate Certificate { get; set; }        
        public virtual Candidate Candidate { get; set; }
        [DisplayName("Order Date")]
        public DateTime OrderDate { get; set; }
        [DisplayName("Shipping Date")]
        public DateTime ShippingDate { get; set; }
        [DisplayName("Order Total")]
        public double OrderTotal { get; set; }
        [DisplayName("Order Status")]
        public string? OrderStatus { get; set; }
        [DisplayName("Payment Status")]
        public string? PaymentStatus { get; set; }
        [DisplayName("Payment Date")]
        public DateTime PaymentDate { get; set; }
        [DisplayName("Payment Due Date")]
        public DateTime PaymentDueDate { get; set; }
        [DisplayName("Session Id")]
        public string? SessionId { get; set; }
        [DisplayName("Payment Intent Id")]
        public string? PaymentIntentId { get; set; }
    }
}

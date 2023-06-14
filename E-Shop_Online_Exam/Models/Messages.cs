using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EShopOnlineExam.Models
{
    public class Messages
    {
        [DisplayName("Message Id")]
        public int Id { get; set; }

        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [Required]
        [DisplayName("Text")]
        public string Text { get; set; }
       
        public DateTime When { get; set; } = DateTime.Now;

        [DisplayName("User Id")]
        public string UserId { get; set; }
        
        public virtual Candidate Sender { get; set; }
    }
}

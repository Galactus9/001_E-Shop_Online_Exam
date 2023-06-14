using System.ComponentModel;

namespace EShopOnlineExam.Models
{
    public class QcNote
    {
        public int Id { get; set; }
        [DisplayName("User Id")]
        public string UserId { get; set; }
        [DisplayName("Text")]
        public string Text { get; set; }
        [DisplayName("Page Url")]
        public string PageUrl { get; set; }
        [DisplayName("Priority Level")]
        public string PriorityLevel { get; set; }
        [DisplayName("Creation Date")]
        public DateTime DateCreated { get; set; }
        [DisplayName("State")]
        public string State { get; set; } = "Open";
        [DisplayName("Admin")]
        public string Supervisor { get; set; } = "None";
    }
}

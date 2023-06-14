using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EShopOnlineExam.Models
{
    public enum ToDoListStatus
    {
        Invalid = -1,
        Open = 1,
        Closed = 2,
        Close_Request = 3
    }
    public partial class ToDoList
    {
        public int Id { get; set; }
        public virtual Candidate Marker { get; set; }
        public virtual CandidateExamination Exam { get; set; }
        [DisplayName("Task Description")]
        public string TaskDescription { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Due Date")]
        public DateTime? DueDate { get; set; }
        [DisplayName("Status")]
        public ToDoListStatus Status { get; set; }
    }
}

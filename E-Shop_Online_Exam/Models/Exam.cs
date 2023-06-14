using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShopOnlineExam.Models
{
	public class Exam
	{
        
        [DisplayName("Exam Id")]
		public int Id { get; set; }
		public virtual Certificate Certificate { get; set; }
        public virtual List<ExamTopics>? Topic { get; set; }
        public virtual ICollection<ExamQuestion>? ExamQuestions { get; set; }
        [DisplayName("Exam Duration")]
        public int ExamDuration { get; set; }

        [DisplayName("Exam Date")]
        [DataType(DataType.Date)]
        public DateTime? ExamDate { get; set; }

    }
}

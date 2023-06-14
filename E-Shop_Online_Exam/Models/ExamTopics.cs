using System.ComponentModel;

namespace EShopOnlineExam.Models
{
	public class ExamTopics
	{
		[DisplayName ("Exam Topic Id")]
		public int Id { get; set; }
		[DisplayName ("Topic Id")]
		public virtual Topic Topic { get; set; }
		[DisplayName ("Exam Id")]
		public virtual Exam Exam { get; set; }
		[DisplayName ("Subject Weight")]
		public int SubjectWeight { get; set; }

    }
}

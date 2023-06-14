using System.ComponentModel;

namespace EShopOnlineExam.Models
{
    public class CandidateExamination
    {
        public int Id { get; set; }

        public virtual Candidate Candidate { get; set; }
        public virtual Exam Exam { get; set; }
        [DisplayName("Exam Starting Time")]
        public DateTime ExamStartingTime { get; set; }
        [DisplayName("Timestamp")]
        public int timestamp { get; set; }
        [DisplayName("Candidate Examination Result")]
        public int? CandidateExaminationResult { get; set; }
        public ICollection<CandidateResults>? CandidateResults { get; set; }
    }
}

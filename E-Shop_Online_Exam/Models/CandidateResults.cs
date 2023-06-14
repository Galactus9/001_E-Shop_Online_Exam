using System.ComponentModel;

namespace EShopOnlineExam.Models
{
    public class CandidateResults
    {
        public int Id { get; set; }
        public CandidateExamination candidateExamination { get; set; }
        [DisplayName("Candidate Answer to Question")]
        public int? CandidateQuestionAnswer { get; set; }
        public ExamQuestion ExamQuestion { get; set; }
        [DisplayName("Candidate Question Result")]
        public int? QuestionResult { get; set; }
    }
}

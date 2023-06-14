namespace EShopOnlineExam.ViewModels
{
    public class AddExamQuestionsVM
    {
        public QuestionAnswers Question { get; set; }
        public string Status { get; set; }
        public ExamTopics ExamTopic { get; set; }
        public int ExamId { get; set; }
        public int TopicId { get; set; }
        public int ExamTopicId { get; set; }
        public int QuestionId { get; set; }
    }
}

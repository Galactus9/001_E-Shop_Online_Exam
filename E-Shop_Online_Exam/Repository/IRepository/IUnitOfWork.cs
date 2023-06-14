namespace EShopOnlineExam.Repository.IRepository
{
    public interface IUnitOfWork
    {

        ICandidateRepository Candidate { get; }
        ITopicRepository Topic { get; }
        ICertificateRepository Certificate { get; }
        ICandidateExaminationRepository CandidateExamination { get; }
        IExamQuestionRepository ExamQuestion { get; }
        IQuestionAnswersRepository QuestionAnswers { get; }
        IExamRepository Exam { get; }
        IExamTopicRepository ExamTopic { get; }
        ICandidateResultsRepository CandidateResults { get; }

        ICandidateCertificateRepository CandidateCertificate { get; }

        ICandidateCartRepository CandidateCart { get; }
        IToDoListRepository ToDoList { get; }

        IOrderRepository Order { get; }

        IMessagesRepository Messages { get; }
        IQcNoteRepository QcNote { get; }
        void Save();
    }
}

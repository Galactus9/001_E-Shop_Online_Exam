using EShopOnlineExam.Data;
using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;

namespace EShopOnlineExam.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private MyDbContext _context;

        public UnitOfWork(MyDbContext context)
        {
            _context = context;
            Candidate = new CandidateRepository(_context);
            Certificate = new CertificateRepository(_context);
            Topic = new TopicRepository(_context);
            QuestionAnswers = new QuestionAnswersRepository(_context);
            CandidateExamination =  new CandidateExaminationRepository(_context);
            Exam = new ExamRepository(_context);
            ExamTopic = new ExamTopicRepository(_context);
            ExamQuestion = new ExamQuestionRepository(_context);
            CandidateResults = new CandidateResultsRepository(_context);
            CandidateCart = new CandidateCartRepository(_context);
            CandidateCertificate = new CandidateCertificateRepository(_context);
            ToDoList = new ToDoListRepository(_context);
            Order = new OrderRepository(_context);
            Messages = new MessagesRepository(_context);
            QcNote = new QcNoteRepository(_context);
        }

        public ICandidateRepository Candidate { get; private set; }
        public ICertificateRepository Certificate { get; private set; }
		public ITopicRepository Topic { get ; private set; }
        public ICandidateExaminationRepository CandidateExamination { get; private set; }
        public IQuestionAnswersRepository QuestionAnswers { get; private set; }
        public IExamQuestionRepository ExamQuestion { get; private set; }
        public IExamRepository Exam { get; private set; }
        public IExamTopicRepository ExamTopic { get; private set; }

        public ICandidateResultsRepository CandidateResults { get; private set; }

        public ICandidateCartRepository CandidateCart { get; private set; }

        public ICandidateCertificateRepository CandidateCertificate { get; private set; }
        public IToDoListRepository ToDoList { get; private set; }

        public IOrderRepository Order { get; private set; }
        public IQcNoteRepository QcNote { get; private set; }

        public IMessagesRepository Messages { get; private set; }
        public void Save()
        { 
            _context.SaveChanges();
        }
    }
}

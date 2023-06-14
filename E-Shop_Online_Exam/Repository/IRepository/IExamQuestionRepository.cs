using EShopOnlineExam.Models;

namespace EShopOnlineExam.Repository.IRepository
{
    public interface IExamQuestionRepository: IRepository<ExamQuestion>
    {
        void Update(ExamQuestion examQuestion);
        void UpdateRange(List<ExamQuestion> examQuestions);

        Task<IEnumerable<ExamQuestion>> GetAllExamQuestionsAs();
        Task<IEnumerable<ExamQuestion>> WhereExamId(int id);
        Task<IEnumerable<ExamQuestion>> WhereExamTopicId(int id);
        IEnumerable<ExamQuestion> WhereExamTopicIdSync(int id);
        Task<IEnumerable<ExamQuestion>> WhereTopicId(int id);
        ExamQuestion WhereQuestionId(int id);
        public void DeleteRange(IEnumerable<ExamQuestion> examquestions);
        ExamQuestion GetExamQuestion(int examTopicId, int questionAnswerId);
    }
}

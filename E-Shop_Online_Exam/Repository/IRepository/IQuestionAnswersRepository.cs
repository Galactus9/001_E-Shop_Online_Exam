using EShopOnlineExam.Models;

namespace EShopOnlineExam.Repository.IRepository
{
    public interface IQuestionAnswersRepository : IRepository<QuestionAnswers>
    {
        void Update(QuestionAnswers questionAnswers);
        public Task<IEnumerable<QuestionAnswers>> GetAllQuestionAnswersAs();
        public ICollection<QuestionAnswers> WhereTopicIdLoaded(int id);

        public Task<QuestionAnswers> GetLoadedAs(int id);
        ICollection<QuestionAnswers> WhereTopicId(int id);

    }
}

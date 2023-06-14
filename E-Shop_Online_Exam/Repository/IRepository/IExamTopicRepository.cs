using EShopOnlineExam.Models;

namespace EShopOnlineExam.Repository.IRepository
{
    public interface IExamTopicRepository : IRepository<ExamTopics>
    {
        Task<IEnumerable<ExamTopics>> WhereExamId(int id);
        IEnumerable<ExamTopics> WhereExamIdSync(int id);
        public Task<IEnumerable<ExamTopics>> GetEnableExamTopic(int id);
    }
}

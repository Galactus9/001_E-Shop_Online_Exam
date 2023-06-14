using EShopOnlineExam.Models;

namespace EShopOnlineExam.Repository.IRepository
{
    public interface ITopicRepository : IRepository<Topic>
    {
        IEnumerable<Topic> GetTopicByCertificate(int certificate_id);

        Task<Topic> GetTopicAs(int topicId);
        Topic GetTopicByTitle(string title);
        public void Update(Topic topic);
        public void UpdateRange(List<Topic> topics);

    }
}

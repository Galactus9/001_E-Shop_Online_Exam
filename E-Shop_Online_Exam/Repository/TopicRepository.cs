using EShopOnlineExam.Data;
using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EShopOnlineExam.Repository
{

        public class TopicRepository : Repository<Topic>, ITopicRepository
        {
            private readonly MyDbContext _context;
            public TopicRepository(MyDbContext context) : base(context)
            {
                _context = context;
            }

            public async Task<Topic> GetTopicAs(int topicId)
            {
                return await _context.Set<Topic>().Include(x => x.Certificate).SingleOrDefaultAsync(x=>x.Id== topicId);
            }
            public IEnumerable<Topic> GetTopicByCertificate(int certificate_id)
            {
                return _context.Set<Topic>().Where(x => x.Certificate.Id == certificate_id).Include(x => x.Certificate).ToList();
            }

            public Topic GetTopicByTitle(string title)
            {
                return _context.Set<Topic>().Where(x => x.Title == title).Single();
            }

            public void Update(Topic topic)
            {
                _context.Update(topic);
            }
            public void UpdateRange(List<Topic> topics)
            {
                _context.UpdateRange(topics);
            }
        }
    
}

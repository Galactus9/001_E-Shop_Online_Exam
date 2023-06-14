 using EShopOnlineExam.Data;
using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EShopOnlineExam.Repository
{
    public class QuestionAnswersRepository : Repository<QuestionAnswers> , IQuestionAnswersRepository
    {
        private readonly MyDbContext _context;
        public QuestionAnswersRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(QuestionAnswers questionAnswers)
        {
            _context.Update(questionAnswers);
        }

        public async Task<IEnumerable<QuestionAnswers>> GetAllQuestionAnswersAs()
        {
            return await _context.Set<QuestionAnswers>().Include(x => x.Topics.Certificate).ToListAsync();
        }

        public ICollection<QuestionAnswers> WhereTopicId(int id)
        {
            return _context.Set<QuestionAnswers>().Include(x => x.Topics).Where(x => x.Topics.Id == id).ToList();
        }
        public ICollection<QuestionAnswers> WhereTopicIdLoaded(int id)
        {
            return _context.Set<QuestionAnswers>().Include(x => x.Topics).ThenInclude(x => x.Certificate).Where(x => x.Topics.Id == id).ToList();
        }

        public async Task<QuestionAnswers> GetLoadedAs(int id)
        {
            return await _context.Set<QuestionAnswers>().Include(x => x.Topics.Certificate).SingleAsync(x => x.Id == id);
        }
    }
}

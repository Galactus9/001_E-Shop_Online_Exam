using EShopOnlineExam.Data;
using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EShopOnlineExam.Repository
{
    public class ExamTopicRepository : Repository<ExamTopics> , IExamTopicRepository
    {
        private readonly MyDbContext _context;

        public ExamTopicRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }
        

        public async Task<IEnumerable<ExamTopics>> WhereExamId(int id)
        {
            return await _context.Set<ExamTopics>().Where(x => x.Exam.Id == id).Include(x => x.Exam).Include(x => x.Topic).ToListAsync();
        } 
        public async Task<IEnumerable<ExamTopics>> GetEnableExamTopic(int id)
        {
            return await _context.Set<ExamTopics>().Where(x => x.Exam.Id == id).Where(x => x.SubjectWeight > 0).Include(x => x.Topic).ToListAsync();
        }

        public IEnumerable<ExamTopics> WhereExamIdSync(int id)
        {
            return _context.Set<ExamTopics>().Where(x => x.Exam.Id == id).Include(x => x.Topic).ToList();
        }
    }
}

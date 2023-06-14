using EShopOnlineExam.Data;
using EShopOnlineExam.Repository.IRepository;
using EShopOnlineExam.Models;
using Microsoft.EntityFrameworkCore;

namespace EShopOnlineExam.Repository
{
    public class ExamQuestionRepository:  Repository<ExamQuestion>, IExamQuestionRepository
    {
        private readonly MyDbContext _context;

        public ExamQuestionRepository(MyDbContext context) : base(context) 
        {
            _context= context;
        }

        public void Update(ExamQuestion examQuestion)
        {
            _context.Update(examQuestion);
        }
        public void UpdateRange(List<ExamQuestion> examQuestions)
        {
            _context.UpdateRange(examQuestions);
        }

        public async Task<IEnumerable<ExamQuestion>> GetAllExamQuestionsAs()
        {
            return await _context.Set<ExamQuestion>().Include(x => x.ExamTopics).Include(x=>x.QuestionAnswer).ToListAsync();
        }

        public async Task<IEnumerable<ExamQuestion>> WhereExamId(int id)
        {
            return await _context.Set<ExamQuestion>().Include(x => x.QuestionAnswer).Where(y => y.ExamTopics.Exam.Id == id).ToListAsync();
        }
        public async Task<IEnumerable<ExamQuestion>> WhereExamTopicId(int id)
        {
            return await _context.Set<ExamQuestion>().Include(x => x.QuestionAnswer).Where(y => y.ExamTopics.Id == id).ToListAsync();
        }
        public async Task<IEnumerable<ExamQuestion>> WhereTopicId(int id)
        {
            return await _context.Set<ExamQuestion>().Include(x => x.QuestionAnswer).Where(y => y.ExamTopics.Topic.Id == id).ToListAsync();
        }

        public ExamQuestion GetExamQuestion(int examTopicId, int questionAnswerId)
        {
            return _context.Set<ExamQuestion>().Where(y => y.ExamTopics.Id == examTopicId).Where(x => x.QuestionAnswer.Id == questionAnswerId).SingleOrDefault();
        }
        public void DeleteRange(IEnumerable<ExamQuestion> examquestions)
        {
            _context.Set<ExamQuestion>().RemoveRange(examquestions);
        }

        public IEnumerable<ExamQuestion> WhereExamTopicIdSync(int id)
        {
            return _context.Set<ExamQuestion>().Include(x => x.QuestionAnswer).Where(y => y.ExamTopics.Id == id).ToList();
        }

        public ExamQuestion WhereQuestionId(int id)
        {
            return _context.Set<ExamQuestion>().Include(x => x.QuestionAnswer).Single(y => y.QuestionAnswer.Id == id);
        }
    }
}

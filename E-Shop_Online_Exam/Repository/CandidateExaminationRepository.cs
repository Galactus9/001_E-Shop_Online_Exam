using EShopOnlineExam.Data;

using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EShopOnlineExam.Repository
{
    public class CandidateExaminationRepository : Repository<CandidateExamination>, ICandidateExaminationRepository
    {
        private readonly MyDbContext _context;

        public CandidateExaminationRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(CandidateExamination candidateExamination)
        {
            _context.Update(candidateExamination);
        }

        public async Task<IEnumerable<CandidateExamination>> GetAllCandidateExaminationsAs()
        {
            return await _context.Set<CandidateExamination>().Include(x => x.Candidate).Include(x => x.Exam).ThenInclude(x=>x.Certificate).ToListAsync();
        }

        public async Task<CandidateExamination> GetAllCandidateExaminationsExamId(int Id)
        {
            return await _context.Set<CandidateExamination>().Include(x => x.CandidateResults).ThenInclude(x => x.ExamQuestion).ThenInclude(x => x.QuestionAnswer).SingleAsync(x => x.Id == Id);
            
        }
        public async Task<List<CandidateExamination>> GetAllExaminationsForCand(string Id)
        {
            return await _context.Set<CandidateExamination>().Include(x => x.Candidate).Include(x => x.Exam).ThenInclude(x => x.Certificate).Where(x => x.Candidate.Id == Id).ToListAsync();
        }

        public async Task<CandidateExamination> GetWithExam(int Id)
        {
            return await _context.Set<CandidateExamination>().Include(x => x.Candidate).Include(x => x.Exam).Where(x => x.Id == Id).SingleOrDefaultAsync();
        }
    }
}

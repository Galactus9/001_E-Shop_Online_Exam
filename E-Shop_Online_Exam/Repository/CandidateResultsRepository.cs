using EShopOnlineExam.Data;
using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EShopOnlineExam.Repository
{
    public class CandidateResultsRepository : Repository<CandidateResults> , ICandidateResultsRepository
    {
        private readonly MyDbContext _context;
        public CandidateResultsRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(CandidateResults candidateResults)
        {
            _context.Update(candidateResults);
        }
        public void UpdateList(List<CandidateResults> candidateResults)
        {
            _context.UpdateRange(candidateResults);
        }

        public async Task<ICollection<CandidateResults>> WhereExaminationId(int examinationId)
        {
            return await _context.Set<CandidateResults>().Include(x => x.ExamQuestion).Where(x => x.candidateExamination.Id == examinationId).ToListAsync();
        List<int> av = new List<int>();

        }
        public async Task<ICollection<CandidateResults>> WhereExaminationIdFull(int examinationId)
        {
            return await _context.Set<CandidateResults>().Include(x => x.candidateExamination)
                                                         .Include(x => x.ExamQuestion)
                                                         .ThenInclude(x => x.QuestionAnswer)
                                                         .Where(x => x.candidateExamination.Id == examinationId).ToListAsync();
        }

        public void UpdateCandidateQuestionAnswer(int? candidateExaminationId, int? answer)
        {
            var candidateResult = _context.Set<CandidateResults>().FirstOrDefault(c => c.Id == candidateExaminationId);
            candidateResult.CandidateQuestionAnswer = answer;
        }
    }
}

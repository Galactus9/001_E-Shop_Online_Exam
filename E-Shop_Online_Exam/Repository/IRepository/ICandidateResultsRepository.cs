using EShopOnlineExam.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace EShopOnlineExam.Repository.IRepository
{
    public interface ICandidateResultsRepository : IRepository<CandidateResults>
    {
        Task<ICollection<CandidateResults>> WhereExaminationId(int examinationId);

        void UpdateCandidateQuestionAnswer(int? candidateExaminationId, int? answer);
        public Task<ICollection<CandidateResults>> WhereExaminationIdFull(int examinationId);
        public void UpdateList(List<CandidateResults> candidateResults);
        public void Update(CandidateResults candidateResults);
    }
}

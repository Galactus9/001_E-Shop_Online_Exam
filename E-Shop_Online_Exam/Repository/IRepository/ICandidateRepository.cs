using EShopOnlineExam.Models;
using Microsoft.AspNetCore.Identity;

namespace EShopOnlineExam.Repository.IRepository
{
    public interface ICandidateRepository : IRepository<Candidate>
    {
        void Update(Candidate candidate);

        public Candidate GetCandidateByCandNum(int candNum);

        public Task<Candidate> GetCandidateById(string Id);
        public Candidate GetCandidateByIdSync(string Id);

        public Task<Candidate> GetCandidateByEmail(string userName);

    }
}

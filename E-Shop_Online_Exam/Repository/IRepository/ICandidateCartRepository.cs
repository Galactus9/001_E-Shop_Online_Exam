using EShopOnlineExam.Models;
using Microsoft.EntityFrameworkCore;

namespace EShopOnlineExam.Repository.IRepository
{
    public interface ICandidateCartRepository : IRepository<CandidateCart>
    {

        void Update (CandidateCart cart);

        public Task<IEnumerable<CandidateCart>> GetAllCertsForCand(string Id);
        public Task<CandidateCart> GetLastOrderForCand(string Id);
        

    }
}

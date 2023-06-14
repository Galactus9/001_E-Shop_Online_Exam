using EShopOnlineExam.Data;
using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EShopOnlineExam.Repository
{
    public class CandidateCartRepository : Repository<CandidateCart>, ICandidateCartRepository
    {
        private readonly MyDbContext _context;

        public CandidateCartRepository(MyDbContext context) : base(context) 
        { 
        
            _context= context;
        }
        public void Update(CandidateCart candidateCart)
        {
            _context.Update(candidateCart);
        }

        public async Task<IEnumerable<CandidateCart>> GetAllCertsForCand(string Id)
        {
            return await _context.Set<CandidateCart>().Include(x => x.Candidate).Include(x => x.Certificates).Where(x => x.Candidate.Id == Id).ToListAsync();
        }

        public async Task<CandidateCart> GetLastOrderForCand(string Id)
        {
            return await _context.Set<CandidateCart>().Include(x => x.Candidate).Include(x => x.Certificates).Where(x => x.Candidate.Id == Id).LastAsync();
        }

        
    }
}

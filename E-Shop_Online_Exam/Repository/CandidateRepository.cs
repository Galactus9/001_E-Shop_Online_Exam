using System.Security.Claims;
using EShopOnlineExam.Data;
using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EShopOnlineExam.Repository
{
    public class CandidateRepository : Repository<Candidate>, ICandidateRepository
    {
        private readonly MyDbContext _context;
        public CandidateRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Candidate candidate)
        {
            _context.Update(candidate);
        }

        public  Candidate GetCandidateByCandNum(int candNum)
        {
            return _context.Set<Candidate>().Where(x => x.CandidateNumber == candNum).SingleOrDefault();
        }

        public async Task<Candidate> GetCandidateById(string Id)
        {
            return await _context.Set<Candidate>().Where(x => x.Id == Id).SingleOrDefaultAsync();
        }

        public Candidate GetCandidateByIdSync(string Id)
        {
            return _context.Set<Candidate>().Where(x => x.Id == Id).SingleOrDefault();
        }

        public async Task<Candidate> GetCandidateByEmail(string userName)
        {
            return await _context.Set<Candidate>().Where(x => x.UserName == userName).SingleOrDefaultAsync();
        }
        
    }
}

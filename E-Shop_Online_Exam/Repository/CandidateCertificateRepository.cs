using EShopOnlineExam.Data;
using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EShopOnlineExam.Repository
{
    public class CandidateCertificateRepository : Repository<CandidateCertificate>, ICandidateCertificateRepository
    {
        private readonly MyDbContext _context;

        public CandidateCertificateRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(CandidateCertificate candidateCertificate)
        {
            _context.Update(candidateCertificate);
        }

        public async Task<IEnumerable<CandidateCertificate>> GetAllCertsForCand (string Id)
        {
            return await _context.Set<CandidateCertificate>().Include(x => x.Candidate).Include(x => x.Certificate).Where(x => x.Candidate.Id == Id).ToListAsync();
        }
    }
}

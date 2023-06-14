using EShopOnlineExam.Data;
using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EShopOnlineExam.Repository
{
    public class CertificateRepository : Repository<Certificate>, ICertificateRepository
    {
        private readonly MyDbContext _context;

        public CertificateRepository (MyDbContext context) : base (context)
        {
            _context = context;
        }

        public async Task<ICollection<Certificate>> GetEnabledCertificates()
        {
            return await _context.Set<Certificate>().Where(x => x.State == CertificateStatus.Enabled).ToListAsync();
        }

        public void Update(Certificate certificate)

        {
            _context.Update(certificate);
        }
        public async Task<IEnumerable<Certificate>> GetCertWhithQuestions()
        {
            return await _context.Set<Certificate>().Include(x => x.Topics).ThenInclude(x => x.QuestionAnswers).ToListAsync();
        }
    }
}

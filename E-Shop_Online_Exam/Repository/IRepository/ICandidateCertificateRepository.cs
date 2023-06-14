using EShopOnlineExam.Models;

namespace EShopOnlineExam.Repository.IRepository
{
    public interface ICandidateCertificateRepository : IRepository<CandidateCertificate>
    {
        void Update(CandidateCertificate candidateCertificate);

        
        public Task<IEnumerable<CandidateCertificate>> GetAllCertsForCand(string Id);
    }
}

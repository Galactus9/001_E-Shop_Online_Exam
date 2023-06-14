using EShopOnlineExam.Models;

namespace EShopOnlineExam.Repository.IRepository
{
    public interface ICertificateRepository : IRepository<Certificate>
    {
        void Update(Certificate certificate);
        public Task<IEnumerable<Certificate>> GetCertWhithQuestions();

        Task<ICollection<Certificate>> GetEnabledCertificates();
    }
}

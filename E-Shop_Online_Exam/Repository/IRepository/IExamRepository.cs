using EShopOnlineExam.Models;

namespace EShopOnlineExam.Repository.IRepository
{
    public interface IExamRepository : IRepository<Exam>
    {
        public Task<Exam> GetExamLoaded(int id);
        Task<IEnumerable<Exam>> GetAllWhithCertAs();
        Task<Exam> GetLoadedAs(int id);
        Task<IEnumerable<Exam>> WhereCertId(int certId);
        public void Update(Exam exam);
    }
}

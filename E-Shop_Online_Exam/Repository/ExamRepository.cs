using EShopOnlineExam.Data;
using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EShopOnlineExam.Repository
{
    public class ExamRepository : Repository<Exam> , IExamRepository
    {
        private readonly MyDbContext _context;

        public ExamRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Exam> GetExamLoaded(int id)
        {
            return await _context.Set<Exam>().Include(x => x.Certificate).SingleAsync(x => x.Id == id);
        }
        public async Task<IEnumerable<Exam>> GetAllWhithCertAs()
        {
            return await _context.Set<Exam>().Include(x => x.Certificate).ToListAsync();
        }

        public async Task<Exam> GetLoadedAs(int id)
        {
            return await _context.Set<Exam>().Include(x => x.Certificate).Include(x => x.Topic).ThenInclude(x => x.Topic).SingleAsync(x => x.Id == id);
        }
        public void Update(Exam exam)
        {
            _context.Update(exam);
        }

        public async Task<IEnumerable<Exam>> WhereCertId(int certId)
        {
            return await _context.Set<Exam>().Where(x=>x.Certificate.Id == certId).ToListAsync();
        }
    }
}

using EShopOnlineExam.Data;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EShopOnlineExam.Repository
{
    public class QcNoteRepository : Repository<QcNote> , IQcNoteRepository
    {
        private readonly MyDbContext _context;
        public QcNoteRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<QcNote>> GetAllQcNotesForUser(string Id)
        {
            return await _context.Set<QcNote>().Where(x => x.UserId == Id).ToListAsync();
        }
        public QcNote GetQcNoteById(int Id)
        {
            return _context.Set<QcNote>().Where(x => x.Id == Id).SingleOrDefault();
        }
        public void RemoveQcNoteById(int Id)
        {
            QcNote qcnote = _context.Set<QcNote>().Where(x => x.Id == Id).SingleOrDefault();
            _context.Remove(qcnote);
            _context.SaveChanges();
        }
    }
}

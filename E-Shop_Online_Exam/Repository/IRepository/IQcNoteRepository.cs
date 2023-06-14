namespace EShopOnlineExam.Repository.IRepository
{
    public interface IQcNoteRepository : IRepository<QcNote>
    {
        public Task<IEnumerable<QcNote>> GetAllQcNotesForUser(string Id);
        public QcNote GetQcNoteById(int Id);
        public void RemoveQcNoteById(int Id);
    }
}

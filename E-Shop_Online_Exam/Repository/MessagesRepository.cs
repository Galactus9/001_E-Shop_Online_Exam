using EShopOnlineExam.Data;

namespace EShopOnlineExam.Repository.IRepository
{
    public class MessagesRepository : Repository<Messages>, IMessagesRepository
    {
        private readonly MyDbContext _context;

        public MessagesRepository(MyDbContext context) : base(context) 
        {
            _context= context;
        }

        public void Update(Messages message)
        {
            _context.Update(message);
        }
    }
}

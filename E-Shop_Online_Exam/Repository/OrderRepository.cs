using EShopOnlineExam.Data;
using EShopOnlineExam.Models;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EShopOnlineExam.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly MyDbContext _context;

        public OrderRepository(MyDbContext context): base(context) 
        {
        
            _context= context;
        }

        public void Update(Order order)
        {
            _context.Update(order);
        }

        public async  Task<IEnumerable<Order>> OrdersGetByCandId(string candId) 
        {
            return await _context.Set<Order>().Include(x => x.Certificate).Include(x=>x.Candidate).Where(x=>x.Candidate.Id == candId).ToListAsync();
        }

        public async Task<IEnumerable<Order>> OrdersGetByOrderId(string orderId)
        {
            return await _context.Set<Order>().Include(x => x.Certificate).Include(x => x.Candidate).Where(x => x.OrderId == orderId).ToListAsync();
        }
        public async Task<IEnumerable<Order>> OrdersGetByOrderIdLoadTopics(string orderId)
        {
            return await _context.Set<Order>().Include(x => x.Certificate).ThenInclude(x => x.Topics).Include(x => x.Candidate).Where(x => x.OrderId == orderId).ToListAsync();
        }

        public async Task<Order> OrderGetByCandId(string candId)
        {
            return await _context.Set<Order>().Include(x => x.Certificate).Include(x => x.Candidate).Where(x => x.Candidate.Id == candId).LastAsync();

        }

        public async Task<IEnumerable<Order>> WhereSessionId(string SessionId)
        {
            return await _context.Set<Order>().Include(x => x.Certificate).Include(x => x.Candidate).Where(x => x.SessionId == SessionId).ToListAsync();
        }
    }
}

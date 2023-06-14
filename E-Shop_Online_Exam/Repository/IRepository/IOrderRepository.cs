using EShopOnlineExam.Models;
namespace EShopOnlineExam.Repository.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order order);

        public Task<IEnumerable<Order>> OrdersGetByCandId(string candId);

        public Task<IEnumerable<Order>> OrdersGetByOrderId(string orderId);
        public Task<IEnumerable<Order>> OrdersGetByOrderIdLoadTopics(string orderId);
        public Task<IEnumerable<Order>> WhereSessionId(string SessionId);
        public Task<Order> OrderGetByCandId(string candId);
    }
}

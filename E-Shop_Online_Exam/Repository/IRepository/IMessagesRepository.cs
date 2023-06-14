namespace EShopOnlineExam.Repository.IRepository
{
    public interface IMessagesRepository : IRepository<Messages>
    {
        void Update(Messages message);
    }
}

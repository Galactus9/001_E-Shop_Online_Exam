using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Linq.Expressions;

namespace EShopOnlineExam.Repository.IRepository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);
        TEntity Get(string id);
        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter);
        void Add(TEntity entity);
        void AddRange(List<TEntity> entities);
        void Delete(TEntity entity);
        public void DeleteRange(IEnumerable<TEntity> entity);

        Task<IEnumerable<TEntity>> GetAllAs();
		Task<TEntity> GetAs(int id);

        Task<TEntity> GetAs(string id);
        Task<TEntity> GetFirstOrDefaultAs(Expression<Func<TEntity, bool>> filter);
		Task AddAs(TEntity entity);
	}
}

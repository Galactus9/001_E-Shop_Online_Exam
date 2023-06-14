using EShopOnlineExam.Data;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace EShopOnlineExam.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly MyDbContext _context;
        internal DbSet<TEntity> dbSet;
        public Repository(MyDbContext context)
        {
            _context = context;
            this.dbSet = _context.Set<TEntity>();
        }
        public virtual void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual TEntity Get(int id)
        {
            return dbSet.Find(id);
        }

        public virtual TEntity Get(string id)
        {
            return dbSet.Find(id);
        }
        public virtual IEnumerable<TEntity> GetAll()
        {
            //IQueryable<TEntity> query = dbSet;
            //return query.ToList();
            return dbSet.ToList();
        }
        public virtual void Delete(TEntity entity)
        {
            dbSet.Remove(entity);
        }
        public virtual void DeleteRange(IEnumerable<TEntity> entity)
        {
            dbSet.RemoveRange(entity);
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = dbSet.Where(filter);
            return query.FirstOrDefault();
        }

		public virtual async Task<IEnumerable<TEntity>> GetAllAs()
		{
            return await dbSet.ToListAsync();
		}

		public virtual async Task<TEntity> GetAs(int id)
		{
            return await dbSet.FindAsync(id);
		}


        public virtual async Task<TEntity> GetAs(string id)
        {
            return await dbSet.FindAsync(id);
        }
        public async Task<TEntity> GetFirstOrDefaultAs(Expression<Func<TEntity, bool>> filter)
		{
			IQueryable<TEntity> query = dbSet.Where(filter);
			return await query.FirstOrDefaultAsync();
		}

		public async Task AddAs(TEntity entity)
		{
			await dbSet.AddAsync(entity);
		}

		public Task DeleteAs(TEntity entity)
		{
			throw new NotImplementedException();
		}

        public void AddRange(List<TEntity> entities)
        {
            _context.AddRange(entities);
        }

    }
}

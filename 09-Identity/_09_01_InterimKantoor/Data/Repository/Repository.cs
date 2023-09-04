using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Linq.Expressions;
using _09_01_InterimKantoor.Data;

namespace _09_01_InterimKantoor
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class { 
        private readonly InterimKantoorContext _context;
        public Repository(InterimKantoorContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(object id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        //De methode SearchKlant toevoegen
        public async Task<IEnumerable<TEntity>> KlantSearch(Expression<Func<TEntity, bool>> zoekwaarde)
        {
            return await _context.Set<TEntity>().Where(zoekwaarde).ToListAsync();
        }
        // 
        public async Task AddAsync(TEntity entity)
        {
            try
            {
                await _context.Set<TEntity>().AddAsync(entity);
            } catch (Exception e)
            {
                throw new Exception(""+ e.Message);
            }
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>>? voorwaarden,
            params Expression<Func<TEntity, object>>[]? includes)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (includes != null)
            {
                foreach (var item in includes)
                {
                        query = query.Include(item);
                }
            }
            if (voorwaarden != null)
            {
                query = query.Where(voorwaarden);
            }
            return query.ToList();
        }
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> voorwaarden)
        {
            return Find(voorwaarden, null).ToList();
        }

        public IEnumerable<TEntity> Find(params Expression<Func<TEntity, object>>[] includes)
        {
            return Find(null, includes).ToList();
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }
        public IQueryable<TEntity> Search()
        {
            return _context.Set<TEntity>().AsQueryable();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

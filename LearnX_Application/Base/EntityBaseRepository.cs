using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Application.Base;
using LearnX_Data.EF;
using Microsoft.EntityFrameworkCore;

namespace LearnX_Application.Base
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class 
    {
        protected DbContext _context;
        internal DbSet<T> dbSet { get; set; }
        public EntityBaseRepository(DbContext context)
        {
            _context = context;
            this.dbSet = this._context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await this.dbSet.AddAsync(entity);
        }
        public  async Task<T> FindAsync(int? id)
        {
            
            return await this.dbSet.FindAsync(id);
        }
        public virtual async Task DeleteAsync(T entity)
        {
            this.dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            
        }
        public IQueryable<T> GetClass()
        {
            return this.dbSet;
        }
        public virtual async Task<List<T>> GetAllAsync()
        {
            return await this.dbSet.ToListAsync();
        }

        public Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, T entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task Update(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
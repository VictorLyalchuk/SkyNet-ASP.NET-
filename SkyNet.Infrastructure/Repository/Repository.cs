using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkyNet.Core.Interfaces;
using SkyNet.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SkyNet.Infrastructure.Repository
{
    internal class Repository <TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        internal AppDbContext context;
        internal DbSet<TEntity> dbSet;
        public Repository(AppDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
        public async Task Insert(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }
        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await dbSet.ToListAsync();
        }
        public async Task Delete(TEntity entity)
        {
            await Task.Run(
                () =>
                {
                    if (context.Entry(entity).State == EntityState.Detached)
                    {
                        dbSet.Attach(entity);
                    }
                    dbSet.Remove(entity);
                });
        }

        public async Task Delete(object id)
        {
            TEntity? entityToDelete = await dbSet.FindAsync(id);
            if (entityToDelete != null)
            {
                await Delete(entityToDelete);
            }
        }

        public async Task Update(TEntity entity)
        {
            await Task.Run
              (
              () =>
              {
                  dbSet.Attach(entity);
                  context.Entry(entity).State = EntityState.Modified;
              });
        }

        public async Task<TEntity?> GetItemBySpec(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetListBySpec(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public async Task<TEntity?> GetByID(object id)
        {
            return await dbSet.FindAsync(id);
        }
        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            var evaluator = new SpecificationEvaluator();
            return evaluator.GetQuery(dbSet, specification);
        }
    }
}

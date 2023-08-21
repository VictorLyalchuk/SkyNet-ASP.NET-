using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task Save();
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity?> GetByID(object id);
        Task Insert(TEntity entity);
        Task Delete(object id);
        Task Delete(TEntity entity);
        Task Update(TEntity entity);
        Task<TEntity?> GetItemBySpec(ISpecification<TEntity> secification);
        Task<IEnumerable<TEntity>> GetListBySpec(ISpecification<TEntity> secification);
    }
}

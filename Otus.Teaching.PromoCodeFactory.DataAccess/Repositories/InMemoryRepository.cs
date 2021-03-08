using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>
        : IRepository<T>
        where T : BaseEntity
    {
        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }

        protected IEnumerable<T> Data { get; set; }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public Task UpdateAsync(T entity)
        {
            var item = ((IList<T>) Data).FirstOrDefault(p => p.Id == entity.Id);
            if (item != null)
            {
                ((IList<T>) Data).Remove(item);
                ((IList<T>) Data).Add(entity);
            }

            return Task.CompletedTask;
        }

        public Task AddAsync(T entity)
        {
          
            ((IList<T>) Data).Add(entity);
            entity.Id =   Guid.NewGuid();
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            var item = ((IList<T>) Data).FirstOrDefault(p => p.Id == entity.Id);
            if (item != null) ((IList<T>) Data).Remove(item);
            return Task.CompletedTask;
        }
    }
}
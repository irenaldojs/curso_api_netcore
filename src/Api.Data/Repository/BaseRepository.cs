using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Data.Context;
using Api.Domain.Entities;
using Api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {   

        protected readonly MyContext _context;
        private DbSet<T> _dataSet;
        public BaseRepository(MyContext context)
        {
            _context = context;
            _dataSet = _context.Set<T>();
        }
        Task<bool> IRepository<T>.DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        async Task<T> IRepository<T>.InsertAsync(T item)
        {
            try{
                // If the id is empty add new id
                if(item.Id == Guid.Empty)
                    item.Id = Guid.NewGuid();
                
                // Receive current date
                item.CreatAt =  DateTime.UtcNow;

                // Add in data set
                _dataSet.Add(item);
                // Commit to database
                await _context.SaveChangesAsync();
            }
            catch(Exception ex){
                throw ex;
            }

            return item;            
        }

        Task<T> IRepository<T>.SelectAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<T>> IRepository<T>.SelectAsync()
        {
            throw new NotImplementedException();
        }

        Task<T> IRepository<T>.UpdateAsync(T item)
        {
            throw new NotImplementedException();
        }
    }
}
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
        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<T> InsertAsync(T item)
        {
            try
            {
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
            catch(Exception ex)
            {
                throw ex;
            }

            return item;            
        }

        public Task<T> SelectAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> SelectAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<T> UpdateAsync(T item)
        {
            try
            {
                // Selecting the record
                var result = await _dataSet.SingleOrDefaultAsync(p => p.Id.Equals(item.Id));
                // Not finding record
                if(result == null)
                    return null;
                // Saving the update time
                item.UpdateAt = DateTime.UtcNow;
                // Saving the create time
                item.CreatAt = result.CreatAt;
                // Setting the values
                _context.Entry(result).CurrentValues.SetValues(item);
                // Commit to database
                await _context.SaveChangesAsync();
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return item;
        }
    }
}
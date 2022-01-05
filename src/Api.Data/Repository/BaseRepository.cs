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
        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                // Selecting the record
                var result = await _dataSet.SingleOrDefaultAsync(p => p.Id.Equals(id));
                // Not finding record
                if(result == null)
                    return false;
                // Delete the record
                _dataSet.Remove(result);
                // Commit to database
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
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

        public async Task<bool> ExistAsync(Guid id)
        {
            // Check if there is a record
            return await _dataSet.AnyAsync(p => p.Id.Equals(id));
        }
        public async Task<T> SelectAsync(Guid id)
        {
            try
            {
                return await _dataSet.SingleOrDefaultAsync(p => p.Id.Equals(id));
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<T>> SelectAsync()
        {
             try
            {
                return await _dataSet.ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
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
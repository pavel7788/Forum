using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRepositoryDetails<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllWithDetailsAsync();

        Task<TEntity> GetByIdWithDetailsAsync<T>(T id);
    }
}

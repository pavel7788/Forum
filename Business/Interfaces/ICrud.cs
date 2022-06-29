using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICrud<TModel> where TModel : class
    {
        Task<IEnumerable<TModel>> GetAllAsync();

        Task<TModel> GetByIdAsync<T>(T id);

        Task AddAsync(TModel model);

        Task UpdateAsync(TModel model);

        Task DeleteAsync<T>(T modelId);


        Task<IEnumerable<TModel>> GetAllWithDetailsAsync();
        Task<TModel> GetByIdWithDetailsAsync<T>(T id);
    }
}

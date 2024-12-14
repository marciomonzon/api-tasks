using System.Linq.Expressions;

namespace Application.Interfaces.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> GetAsync(Expression<Func<T, bool>> expression);
        IEnumerable<T> GetAll();
        Task<T> CreateAsync(T command);
        Task<T> UpdateAsync(T commandUpdate);
        Task DeleteAsync(Guid Id);
    }
}

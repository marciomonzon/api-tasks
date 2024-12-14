using Application.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly TasksDbContext _context;

        public BaseRepository(TasksDbContext context)
        {
            _context = context;
        }

        public async Task<T> CreateAsync(T command)
        {
            await _context.Set<T>().AddAsync(command);
            return command;
        }

        public Task DeleteAsync(Guid Id)
        {
            _context.Remove(Id);
            return Task.CompletedTask;
        }

        public IEnumerable<T> GetAll()
        {
            return [.. _context.Set<T>().ToList()];
        }

        public async Task<T> GetAsync(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<T> UpdateAsync(T commandUpdate)
        {
            _context.Set<T>().Update(commandUpdate);
            return commandUpdate;
        }
    }
}

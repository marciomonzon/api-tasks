using Application.Interfaces.Repository;
using Domain.Entities;

namespace Infrastructure.Persistence.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(TasksDbContext context) : base(context)
        {
        }
    }
}

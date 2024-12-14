using Application.Interfaces.Repository;

namespace Application.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        Task CommitAsync();
        void Commit();
        void Rollback();
    }
}

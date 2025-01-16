using Domain.Repositories.Interfaces;

namespace Domain.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();

        IUserRepository UserRepository { get; set; }
    }
}

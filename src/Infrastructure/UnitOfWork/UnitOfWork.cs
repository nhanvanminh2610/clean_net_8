using Domain.Repositories.Interfaces;
using Domain.UnitOfWork;
using Infrastructure.Context;
using Infrastructure.Repositories.Concrete;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CompleteAsync()
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var result = await _dbContext.SaveChangesAsync();
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        #region repositories container

        private IUserRepository _userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null) _userRepository = new UserRepository(_dbContext);
                return _userRepository;
            }
        }

        #endregion
    }
}

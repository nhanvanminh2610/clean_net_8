using Domain.Entities.Tables;
using Domain.Repositories.Interfaces;
using Infrastructure.Context;
using Infrastructure.Repositories.Abstract;

namespace Infrastructure.Repositories.Concrete
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}

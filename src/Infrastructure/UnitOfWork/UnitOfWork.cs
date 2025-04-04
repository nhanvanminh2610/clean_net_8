﻿using Domain.UnitOfWork;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork<TDbContext> : IUnitOfWork, IDisposable where TDbContext : DbContext
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
    }
}

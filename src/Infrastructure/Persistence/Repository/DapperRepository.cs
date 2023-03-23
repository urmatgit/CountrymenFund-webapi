using System.Data;
using System.Linq.Expressions;
using Dapper;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Domain.Common.Contracts;
using FSH.WebApi.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FSH.WebApi.Infrastructure.Persistence.Repository;

public class DapperRepository : IDapperRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DapperRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    where T : class, IEntity =>
        (await _dbContext.Connection.QueryAsync<T>(sql, param, transaction))
            .AsList();

    public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where T : class, IEntity
    {
        if (_dbContext.Model.GetMultiTenantEntityTypes().Any(t => t.ClrType == typeof(T)))
        {
            sql = sql.Replace("@tenant", _dbContext.TenantInfo.Id);
        }

        return await _dbContext.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
    }

    public Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    where T : class, IEntity
    {
        if (_dbContext.Model.GetMultiTenantEntityTypes().Any(t => t.ClrType == typeof(T)))
        {
            sql = sql.Replace("@tenant", _dbContext.TenantInfo.Id);
        }

        return _dbContext.Connection.QuerySingleAsync<T>(sql, param, transaction);
    }

    public async Task<decimal> QuerySumValueAsync<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default)
         where T : class, IEntity => await _dbContext.Set<T>().Where(predicate).SumAsync(selector, cancellationToken);//return _dbContext.Connection.QuerySingleAsync<T>(sql, param, transaction);
    public async  Task<IEnumerable<IGroupTotal>> QueryRuralGovSumsValue(DateTime start, DateTime end, CancellationToken cancellationToken = default)
        
    {
        var controlbution = await   _dbContext.Contributions
            .Include(x => x.Native)
            .ThenInclude(x => x.RuralGov)
            .Where(x => x.Date >= start && x.Date <= end)
            .GroupBy(x => x.Native.RuralGov.Name)
            .Select(x => new GroupTotal
            {
                Name=x.Key,
                Total=x.Sum(y=>y.Summa)
            }).ToListAsync(cancellationToken);
        return controlbution;
    }

   public IQueryable<T> GetQueryable<T>()
        where T : class, IEntity
    {
        return _dbContext.Set<T>().AsQueryable();
    }

    public async Task<IEnumerable<IGroupTotal>> QueryFinSupportSumsValue(DateTime start, DateTime end, CancellationToken cancellationToken = default)
    {
        var controlbution = await _dbContext.FSContributions
          .Include(x => x.FinSupport)
          .Where(x => x.Date >= start && x.Date <= end)
          .GroupBy(x => x.FinSupport.Name)
          .Select(x => new GroupTotal
          {
              Name = x.Key,
              Total = x.Sum(y => y.Summa)
          }).ToListAsync(cancellationToken);
        return controlbution;
    }
}
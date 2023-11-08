using FloraEdu.Application.Services.Interfaces;
using FloraEdu.Domain.Entities;
using FloraEdu.Domain.Exceptions;
using FloraEdu.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloraEdu.Application.Services.Implementations;

public class BaseService<T> : IService<T> where T : BaseEntity
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger _logger;

    public BaseService(ApplicationDbContext dbContext, ILogger logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public virtual async Task<IEnumerable<T>> GetAll()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public virtual async Task<T> GetById(Guid id)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id);

        if (entity is not null) return entity;

        _logger.LogError("Entity with ID: {id} not found", id);
        throw new ApiException("Entity not found", ErrorCodes.NotFound);
    }

    public virtual async Task<T> Create(T entity)
    {
        var entry = _dbContext.Set<T>().Add(entity);
        await _dbContext.SaveChangesAsync();
        return entry.Entity;
    }

    public virtual async Task<T> Update(T entity)
    {
        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<bool> Delete(Guid id)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id);

        if (entity is null) return false;

        entity.IsDeleted = true;
        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}

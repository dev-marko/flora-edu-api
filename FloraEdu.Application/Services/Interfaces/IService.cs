using FloraEdu.Domain.Entities;

namespace FloraEdu.Application.Services.Interfaces;

public interface IService<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(Guid id);
    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task<bool> Delete(Guid id);
}

using FloraEdu.Domain.Entities;

namespace FloraEdu.Persistence.Repository.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAll();
    Task<T>? GetById(Guid? id);
    Task<T> Insert(T entity);
    Task Update(T entity);
    Task Delete(T entity);
}

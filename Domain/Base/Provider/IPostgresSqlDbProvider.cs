
using Domain.Entities;

namespace Domain.Base.Provider;

public interface IPostgresSqlDbProvider
{
    Task PersistAsync(State state, IEntity entity);
    

    // Task Add(IEntity entity);
    // Task AddAsync(IEntity entity);

    // Task Update(IEntity entity);
    // Task UpdateAsync(IEntity entity);

    // Task Delete(IEntity entity);
    // Task DeleteAsync(IEntity entity);

    // Task Persist();
    // Task PersistAsync();

}
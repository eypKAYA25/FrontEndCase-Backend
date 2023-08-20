using System.ComponentModel.DataAnnotations;
using System.Reflection;
using AutoMapper;
using Domain.Base;
using Domain.Base.Provider;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Provider;

public class PostgresSqlDbProvider : IPostgresSqlDbProvider
{
    private readonly PostgreSqlDbContext _postgreSqlDbContext;
    private readonly IMapper _mapper;
    public PostgresSqlDbProvider(
        PostgreSqlDbContext postgreSqlDbContext
        , IMapper mapper
    )
    {
        this._postgreSqlDbContext = postgreSqlDbContext;
        this._mapper = mapper;
    }

    public async Task PersistAsync(State state, IEntity entity)
    {
        try
        {
            string entityName = entity.GetType().Name;
            Type entityType = Assembly.GetAssembly(typeof(PostgreSqlDbContext)).GetTypes().First(x =>
                x.Namespace is not null && x.Namespace.Contains("Infrastructure.Entities") &&
                x.Name == entityName);

            switch (state)
            {
                case State.Detached:
                    // entry.State = EntityState.Detached;
                    break;
                case State.Unchanged:
                    // entry.State = EntityState.Unchanged;
                    break;
                case State.Deleted:
                {
                    object modifiedEntity = this._mapper.Map(entity, entity.GetType(), entityType);
                    List<PropertyInfo> primaryKeyPropertyInfos = entityType.GetProperties().Where(p => p.GetCustomAttribute<KeyAttribute>() != null).ToList();
                    if (primaryKeyPropertyInfos.Count == decimal.Zero)
                    {
                        throw new ArgumentNullException("idProperty doesn't exists in entity obj");
                    }

                    object?[] keyValue = primaryKeyPropertyInfos.Select(x => x.GetValue(modifiedEntity)).ToArray();
                    object entityInContext = this._postgreSqlDbContext.Find(entityType, keyValue);
                    EntityEntry entityEntry = this._postgreSqlDbContext.Entry(entityInContext);
                    entityEntry.State = EntityState.Deleted;
                }
                    break;
                case State.Modified:
                {
                    object modifiedEntity = this._mapper.Map(entity, entity.GetType(), entityType);
                    List<PropertyInfo> primaryKeyPropertyInfos = entityType.GetProperties().Where(p => p.GetCustomAttribute<KeyAttribute>() != null).ToList();
                    if (primaryKeyPropertyInfos.Count == decimal.Zero)
                    {
                        throw new ArgumentNullException("idProperty doesn't exists in entity obj");
                    }

                    object?[] keyValue = primaryKeyPropertyInfos.Select(x => x.GetValue(modifiedEntity)).ToArray();
                    object entityInContext = this._postgreSqlDbContext.Find(entityType, keyValue);
                    EntityEntry entityEntry = this._postgreSqlDbContext.Entry(entityInContext);
                    entityEntry.CurrentValues.SetValues(modifiedEntity);
                    entityEntry.State = EntityState.Modified;
                }
                    break;
                case State.Added:
                    object dbEntity = this._mapper.Map(entity, entity.GetType(), entityType);
                    
                    MethodInfo? genericDbSet = this._postgreSqlDbContext.GetType().GetMethods().FirstOrDefault(x => x.Name.Equals("Set")).MakeGenericMethod(entityType);
                    object genericSet = genericDbSet.Invoke(this._postgreSqlDbContext, null);
                    
                    MethodInfo? addMethodInfo = genericSet.GetType().GetMethods().FirstOrDefault(x => x.Name.Equals("Add"));
                    addMethodInfo.Invoke(genericSet, new object?[] { dbEntity });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            await this._postgreSqlDbContext.SaveChangesAsync();
        }
        catch (Exception exception)
        {           
            Console.WriteLine($"{exception}, Exception Occured at PersistAsync");
            throw;
        }
        finally
        {
        }
    }

    
}
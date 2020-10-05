using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Repositories;

namespace QiProcureDemo.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// Base class for custom repositories of the application.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
    public abstract class QiProcureDemoRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<QiProcureDemoDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected QiProcureDemoRepositoryBase(IDbContextProvider<QiProcureDemoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add your common methods for all repositories
    }

    /// <summary>
    /// Base class for custom repositories of the application.
    /// This is a shortcut of <see cref="QiProcureDemoRepositoryBase{TEntity,TPrimaryKey}"/> for <see cref="int"/> primary key.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class QiProcureDemoRepositoryBase<TEntity> : QiProcureDemoRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected QiProcureDemoRepositoryBase(IDbContextProvider<QiProcureDemoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)!!!
    }
}

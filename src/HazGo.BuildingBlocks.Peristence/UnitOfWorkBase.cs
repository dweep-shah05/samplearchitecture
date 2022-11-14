namespace HazGo.BuildingBlocks.Persistence.EF
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using HazGo.BuildingBlocks.Core.Domain;
    using Microsoft.EntityFrameworkCore;

    public abstract class UnitOfWorkBase<TContext> : IUnitOfWork
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly IDomainEventDispatcher _domaineventdispatcher;

        protected UnitOfWorkBase(TContext context, IDomainEventDispatcher domainEventDispatcher)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _domaineventdispatcher = domainEventDispatcher;
        }

        public virtual async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            await _domaineventdispatcher.DispatchEventsAsync();
            return await this._context.SaveChangesAsync(cancellationToken);
        }
    }
}

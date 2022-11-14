namespace HazGo.Infrastructure
{
    using HazGo.BuildingBlocks.Core.Domain;
    using HazGo.BuildingBlocks.Persistence.EF;
    public class UnitOfWork : UnitOfWorkBase<ApplicationDbContext>
    {
        public UnitOfWork(ApplicationDbContext dbContext, IDomainEventDispatcher domainEventDispatcher)
             : base(dbContext, domainEventDispatcher)
        {

        }
    }
}

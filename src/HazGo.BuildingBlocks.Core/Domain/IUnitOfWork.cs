namespace HazGo.BuildingBlocks.Core.Domain
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IUnitOfWork
    {
        Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}

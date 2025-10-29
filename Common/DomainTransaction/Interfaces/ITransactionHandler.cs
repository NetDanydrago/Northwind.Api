namespace DomainTransaction.Interfaces;
public interface ITransactionHandler
{
    Task BeginTransactionAsync();

    Task CommitAsync();

    Task RollbackAsync();
}

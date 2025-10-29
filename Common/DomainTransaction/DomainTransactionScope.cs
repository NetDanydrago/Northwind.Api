using DomainTransaction.Interfaces;

namespace DomainTransaction;
public class DomainTransactionScope : IAsyncDisposable
{
    // bandera para saber si invocaron al complete
    bool Completed = false;

    // una lista para guardar los handlers que están participando en la transacción.
    List<ITransactionHandler> Handlers = new List<ITransactionHandler>();

    public async Task EnlistAsync(ITransactionHandler handler)
    {
        Handlers.Add(handler);
        await handler.BeginTransactionAsync();
    }

    public void Complete()
    {
        Completed = true;
    }

    async Task CommitAsync()
    {
        foreach (var Handler in Handlers)
        {
            await Handler.CommitAsync();
        }
    }

    async Task RollBackAsync()
    {
        foreach (var Handler in Handlers)
        {
            await Handler.RollbackAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (Completed)
        {
            await CommitAsync();

        }
        else
        {
            await RollBackAsync();
        }
        Handlers.Clear();
    }
}

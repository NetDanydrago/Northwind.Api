using DomainTransaction.Interfaces;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace NorthWind.EFCore.Repositories.Repositories;

public abstract class TransactionHandlerBase(DatabaseFacade database) : ITransactionHandler, IAsyncDisposable
{
    IDbContextTransaction Transaction;

    public async Task BeginTransactionAsync()
    {
        if (Transaction != null)
            await DisposeTransactionAsync();

        Transaction = await database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (Transaction != null)
        {
            await Transaction.CommitAsync();
            await DisposeTransactionAsync();
        }
    }

    public async Task RollbackAsync()
    {
        if (Transaction != null)
        {
            await Transaction.RollbackAsync();
            await DisposeTransactionAsync();
        }
    }

    async Task DisposeTransactionAsync()
    {
        await Transaction.DisposeAsync();
        Transaction = null;
    }

    public async ValueTask DisposeAsync()
    {
        if (Transaction != null)
            await DisposeTransactionAsync();
    }

}


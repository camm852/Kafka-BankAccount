using Banking.Account.Command.Domain.Common;

namespace Banking.Account.Command.Application.Contracts.Persistence
{
    public interface IMongoRepository<TDocument> where TDocument : IDocument
    {
        Task<IEnumerable<TDocument>> GetAllAsync();
        Task<TDocument> GetByIdAsync(string id);
        Task InsertDocument(TDocument document);
        Task UpdateDocument(TDocument document);
        Task DeleteDocument(string id);
    }
}

using Banking.Account.Command.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Command.Application.Contracts.Persistence
{
    public interface IEventStoreRepository : IMongoRepository<EventModel>
    {
        Task<IEnumerable<EventModel>> FindByAgreggateByIdentifier(string aggregateIdentifier);
    }
}

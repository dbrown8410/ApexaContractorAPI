using ApexaContractorAPI.Entities;

namespace ApexaContractorAPI.Repository
{
    public abstract class BaseRepository : IRepository
    {
        protected readonly ContractorDBContext Context;

        public BaseRepository(ContractorDBContext context)
        {
            Context = context;
        }
    }
}

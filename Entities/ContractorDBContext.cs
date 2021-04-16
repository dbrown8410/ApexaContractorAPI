using Microsoft.EntityFrameworkCore;

namespace ApexaContractorAPI.Entities
{
    public class ContractorDBContext : DbContext
    {
        public ContractorDBContext(DbContextOptions<ContractorDBContext> options) : base(options)
        {
        }

        public DbSet<Contractor> Contractor { get; set; } 
        public DbSet<Contracts> Contracts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}

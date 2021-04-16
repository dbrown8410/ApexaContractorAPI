using System.ComponentModel.DataAnnotations;

namespace ApexaContractorAPI.Entities
{
    public class Contracts
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ContractorOneId { get; set; }
        [Required]
        public int ContractorTwoId { get; set; }
    }
}

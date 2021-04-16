using System.ComponentModel.DataAnnotations;

namespace ApexaContractorAPI.Entities
{
    public class Contractor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string HealthStatus { get; set; }
    }
}

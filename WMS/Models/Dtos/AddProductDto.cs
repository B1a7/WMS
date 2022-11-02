using System.ComponentModel.DataAnnotations;

namespace WMS.Models.Dtos
{
    public class AddProductDto
    {

        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        public DateTime ProductionDate { get; set; }
        public string Category { get; set; }
        [Required]
        public string ClientName { get; set; }
        [Required]
        [EmailAddress]
        public string ClientEmail { get; set; }
        [Phone]
        public string ClientPhoneNumber { get; set; }
        [Required]
        [MaxLength(50)]
        public string City { get; set; }
        [Required]
        [MaxLength(50)]
        public string Street { get; set; }
        public string PostalCode { get; set; }
        [Required]
        public string Status { get; set; }
        public DateTime StatusRegistrationDate { get; set; }

    }
}

namespace Transport.API.Models.CreateModels
{
    using System.ComponentModel.DataAnnotations;
    public class CreateTransportModel
    {
        [Required]
        public string TransportType { get; set; }
        [Required]
        public string ProducerCompany { get; set; }
        [Required]
        public string ModelName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int DriverId { get; set; }
    }
}

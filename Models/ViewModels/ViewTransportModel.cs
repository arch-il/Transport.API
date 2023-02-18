namespace Transport.API.Models.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    public class ViewTransportModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string TransportType { get; set; }
        [Required]
        public string ProducerCompany { get; set; }
        [Required]
        public string ModelName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public ViewPersonModel Driver { get; set; }
    }
}

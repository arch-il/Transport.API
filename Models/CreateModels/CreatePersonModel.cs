namespace Transport.API.Models.CreateModels
{
    using System.ComponentModel.DataAnnotations;
    public class CreatePersonModel
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public bool IsDriver { get; set; }
    }
}

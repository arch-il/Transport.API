namespace Transport.API.Models.UpdateModels
{
    using System.ComponentModel.DataAnnotations;
    public class UpdatePersonModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public bool IsDriver { get; set; }
    }
}

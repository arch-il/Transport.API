namespace Transport.API.Models.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    public class ViewPersonModel
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

namespace Transport.API.Interfaces
{
    using Transport.API.Models.CreateModels;

    public interface IPersonService
    {
        public bool IsValidPerson(CreatePersonModel person);
    }
}

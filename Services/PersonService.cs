namespace Transport.API.Services
{
    using Transport.API.Interfaces;
    using Transport.API.Models.CreateModels;

    public class PersonService : IPersonService
    {
        public bool IsValidPerson(CreatePersonModel person)
        {
            // check if values are valid
            if (person == null ||
                person.FullName.Length < 0 ||
                person.Age < 0)
                return false;
            return true;
        }
    }
}

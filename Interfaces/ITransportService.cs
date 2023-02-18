namespace Transport.API.Interfaces
{
    using Transport.API.Models.CreateModels;

    public interface ITransportService
    {
        public bool isValidTransport(CreateTransportModel transport);
    }
}

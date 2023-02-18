namespace Transport.API.Services
{
    using Transport.API.Interfaces;
    using Transport.API.Models.CreateModels;

    public class TransportService : ITransportService
    {
        public bool isValidTransport(CreateTransportModel transport)
        {
            // check if values are valid
            if (transport == null ||
                transport.ModelName.Length < 0 ||
                transport.TransportType.Length < 0)
                return false;
            return true;
        }
    }
}

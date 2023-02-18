namespace Transport.API.Entities
{
    public class Transport
    {
        public int Id { get; set; }
        public string TransportType { get; set; }
        public string ProducerCompany { get; set; }
        public string ModelName { get; set; }
        public decimal Price { get; set; }
        public Person Driver { get; set; }
    }
}

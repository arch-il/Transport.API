namespace Transport.API.Context
{
    using Transport.API.Entities;
    using Microsoft.EntityFrameworkCore;

    public sealed class TransportContext : DbContext
    {
        public DbSet<Transport> Transport { get; set; }
        public DbSet<Person> Person { get; set; }

        public TransportContext(DbContextOptions<TransportContext> options)
                : base(options)
        {
            this.Database.EnsureCreated();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Transport.API.Context;
using Transport.API.Interfaces;
using Transport.API.Services;

var builder = WebApplication.CreateBuilder(args);

// add context
builder.Services.AddDbContext<TransportContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("TransportDB")));


// add controllers
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add services
builder.Services.AddTransient<IPersonService, PersonService>();
builder.Services.AddTransient<ITransportService, TransportService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

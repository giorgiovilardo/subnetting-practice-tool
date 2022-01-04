using System.Net;

using SubnetPracticeTool.API.GetExercise;
using SubnetPracticeTool.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddResponseCompression();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseResponseCompression();

app.MapGet("/exercise", () =>
{
    (IPAddress ipAddress, SubnetMask subnetMask) = new ExerciseFactory(new Random()).GetRandomExercise();
    return new GetExerciseResponse(ipAddress.ToString(), subnetMask.ToCidrBitsString());
}).Produces<GetExerciseResponse>();

app.Run();

// Needed to make the API instantiable by the tests, do not remove
public partial class Program
{
}

using System.Net;

using FluentValidation;
using FluentValidation.AspNetCore;

using SubnetPracticeTool.API.Extensions;
using SubnetPracticeTool.API.GetExercise;
using SubnetPracticeTool.API.SolveExercise;
using SubnetPracticeTool.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddResponseCompression();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidation(c =>
    c.RegisterValidatorsFromAssemblyContaining<SolveExerciseCommand>(includeInternalTypes: true));

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
    return Results.Ok(new GetExerciseQuery(ipAddress.ToString(), subnetMask.ToCidrBitsString()));
}).Produces<GetExerciseQuery>();

app.MapPost("/solution", (IValidator<SolveExerciseCommand> validator, SolveExerciseCommand solution) =>
{
    var validationResult = validator.Validate(solution);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var solver = ExerciseSolver.FromString(solution.IpAddress, solution.SubnetBits);
    if (solver.IsValidSolution(solution.NetworkAddress, solution.BroadcastAddress))
    {
        return Results.Ok(new SolveExerciseCommandResponse(true));
    }

    var wrongSolutionItems =
        solver.GetSolutionErrors(solution.NetworkAddress, solution.BroadcastAddress).Select(x =>
            new WrongSolutionItem(x["field"], x["message"]));

    return Results.BadRequest(new SolveExerciseCommandResponse(false, wrongSolutionItems));


}).Accepts<SolveExerciseCommand>("application/json")
.ProducesValidationProblem(contentType: "application/json")
.Produces<SolveExerciseCommandResponse>(contentType: "application/json");

app.Run();

// Needed to make the API instantiable by the tests, do not remove
public partial class Program
{
}

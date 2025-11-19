using VoteCastingService.Application.Extensions;
using VoteCastingService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplicationServices()
    .AddInfrastructureServices();


var app = builder.Build();


app.MapControllers();
app.Run();
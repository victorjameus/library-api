using Library.Api.Extensions;
using Library.Application;
using Library.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation();

var app = builder.Build();

app.ConfigurePipeline();
app.Run();

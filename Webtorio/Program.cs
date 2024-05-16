using System.Reflection;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Webtorio.Application.Buildings.Services;
using Webtorio.Application.Buildings.Services.StateMachine;
using Webtorio.Application.Interfaces;
using Webtorio.Common.Behaviors;
using Webtorio.Common.ProblemDetails;
using Webtorio.PersistentData;
using Webtorio.PersistentData.Repositories;
using Webtorio.Services;
using Webtorio.StartupFilters;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, loggerConfiguration) => loggerConfiguration
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
    .MinimumLevel.Override("System", LogEventLevel.Debug)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs.txt",                     
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        rollOnFileSizeLimit: true));

string connection = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connection));

builder.Services.AddTransient<IStartupFilter, LoadBuildingStateStartupFilter>();
builder.Services.AddTransient<IStartupFilter, SeedDatabaseStartupFilter>();
builder.Services.AddTransient<IStartupFilter, MigrationApplyStartupFilter>();

builder.Services.AddScoped<IRepository, Repository>();

builder.Services.AddHostedService<GameLoopService>();
builder.Services.AddSingleton<GameTickHandler>();
builder.Services.AddScoped<BuildingWorkService>();
builder.Services.AddSingleton<BuildingStateMachine>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);

builder.Services.AddSingleton<ProblemDetailsFactory, CustomProblemDetailsFactory>();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddControllers();

var config = TypeAdapterConfig.GlobalSettings;
config.Scan(Assembly.GetExecutingAssembly());

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
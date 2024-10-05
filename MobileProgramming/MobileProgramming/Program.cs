
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MobileProgramming.API;
using MobileProgramming.Business;
using MobileProgramming.Data.Configuration;
using MobileProgramming.Data.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.ConfigureSerilog();
builder.Services.AddScoped<GlobalException>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.ConfigureHealthChecks(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SaleProductDbContext>();
}



app.UseCors(app => app
    .AllowAnyOrigin()
        .AllowAnyMethod()
            .AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.ConfigureSwaggerUI();
}


app.UseSerilogRequestLogging();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapControllers();

app.MapHealthChecks("/h", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.ConfigureExceptionHandler();


app.Run();

using Fanex.BetBuilder.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.LoadConfiguration();
builder.Services.RegisterSentry(builder.Configuration);
builder.Services.RegisterFanexData(builder.Configuration);
builder.Services.RegisterRemotingRef(builder.Configuration);
builder.Services.InternalServiceRegister();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
using BetBuilder.Business.Services;
using BetBuilder.DailyService;
using Fanex.MailDelivery.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterSentry(builder.Configuration);
builder.Services.RegisterOnebookUm(builder.Configuration);
builder.Services.RegisterFanexData(builder.Configuration);
builder.Services.InternalServiceRegister();

BetBuilderSettings.LoadSettings(builder.Configuration);

MailClientHandler.Start();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Telegram.Bot;
using TelegramBot;
using BLL.Mappers;
using Common.IoC;
using DAL.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterBuildCallback(ctx => IoC.Container = ctx.Resolve<ILifetimeScope>());

    BLL.Startup.Bootstrapper.Bootstrap(containerBuilder);
});


builder.Services.AddAutoMapper(typeof(DefaultMappingProfile));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITelegramBotClient>(sp =>
{
    var botToken = BotConfiguration.BotToken; // Replace with your actual bot token
    return new TelegramBotClient(botToken);
});
builder.Services.AddSingleton<BotEventHandler>();

builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

var botClient = app.Services.GetRequiredService<ITelegramBotClient>();
var serviceProvider = app.Services;
BotRequestHandler.Initialize(botClient, serviceProvider);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

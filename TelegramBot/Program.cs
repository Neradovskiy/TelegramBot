using Telegram.Bot;
using TelegramBot;
using TelegramBot.Model;
using Microsoft.EntityFrameworkCore;
using TelegramBot.Service;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddTransient<DaoClient>();
builder.Services.AddTransient<DaoWorker>();
builder.Services.AddTransient<DaoPlanForDay>();
builder.Services.AddTransient<DaoAbonement>();
builder.Services.AddTransient<DaoStatus>();
builder.Services.AddSingleton<DbFactory>();
//builder.Services.AddTransient<TgBot>();
builder.Services.AddSingleton<TgBot>();
var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(() =>
{
    app.Services.GetRequiredService<TgBot>();
    app.Services.GetRequiredService<DbFactory>();
});

app.MapGet("/",() => { return DateTime.UtcNow; });
//Client
app.MapGet("/getClients", async (DaoClient dao) =>
{
    return await dao.GetAllAsync();
});
app.MapPost("/addClient", async (Client client, DaoClient dao) =>
{
    return await dao.AddAsync(client);
});

app.MapPost("/deleteClient", async (Client client, DaoClient dao) =>
{
    return await dao.DeleteAsync(client.Id);
});

app.MapPost("/updateClient", async (Client client, DaoClient dao) =>
{
    return await dao.UpdateAsync(client);
});

app.MapPost("/findByIdClient", async (Client client, DaoClient dao) =>
{
    return await dao.GetAsync(client.Id);
});

//Worker
app.MapGet("/getWorkers", async (DaoWorker dao) =>
{
    return await dao.GetAllAsync();
});
app.MapPost("/addWorkers", async (Worker worker, DaoWorker dao) =>
{
    return await dao.AddAsync(worker);
});

app.MapPost("/deleteWorker", async (Worker worker, DaoWorker dao) =>
{
    return await dao.DeleteAsync(worker.Id);
});

app.MapPost("/updateWorker", async (Worker worker, DaoWorker dao) =>
{
    return await dao.UpdateAsync(worker);
});

app.MapPost("/findByIdWorker", async (Worker worker, DaoWorker dao) =>
{
    return await dao.GetAsync(worker.Id);
});

//PlanForDay

app.MapGet("/getPlan", async (DaoPlanForDay dao) =>
{
    return await dao.GetAllAsync();
});
app.MapPost("/addPlan", async (PlanForDay plan, DaoPlanForDay dao) =>
{
    return await dao.AddAsync(plan);
});

app.MapPost("/deletePlan", async (PlanForDay plan, DaoPlanForDay dao) =>
{
    return await dao.DeleteAsync(plan.Id);
});

app.MapPost("/updatePlan", async (PlanForDay plan, DaoPlanForDay dao) =>
{
    return await dao.UpdateAsync(plan);
});

app.MapPost("/findByIdPlan", async (PlanForDay plan, DaoPlanForDay dao) =>
{
    return await dao.GetAsync(plan.Id);
});

// Abonement
app.MapGet("/getAbonement", async (DaoAbonement dao) =>
{
    return await dao.GetAllAsync();
});
app.MapPost("/addAbonement", async (Abonement abonement, DaoAbonement dao) =>
{
    return await dao.AddAsync(abonement);
});

app.MapPost("/deleteAbonement", async (Abonement abonement, DaoAbonement dao) =>
{
    return await dao.DeleteAsync(abonement.Id);
});

app.MapPost("/updateAbonement", async (Abonement abonement, DaoAbonement dao) =>
{
    return await dao.UpdateAsync(abonement);
});

app.MapPost("/findByIdAbonement", async (Abonement abonement, DaoAbonement dao) =>
{
    return await dao.GetAsync(abonement.Id);
});

//Status
app.MapGet("/getStatuses", async (DaoStatus dao) =>
{
    return await dao.GetAllAsync();
});
app.MapPost("/addStatus", async (Status status, DaoStatus dao) =>
{
    return await dao.AddAsync(status);
});

app.MapPost("/deleteStatus", async (Status status, DaoStatus dao) =>
{
    return await dao.DeleteAsync(status.Id);
});

app.MapPost("/updateStatus", async (Status status, DaoStatus dao) =>
{
    return await dao.UpdateAsync(status);
});

app.MapPost("/findByIdStatus", async (Status status, DaoStatus dao) =>
{
    return await dao.GetAsync(status.Id);
});


app.Run();

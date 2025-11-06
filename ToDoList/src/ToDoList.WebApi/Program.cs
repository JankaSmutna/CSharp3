using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args); //builder == postup, jak se aplikace vytvoří
{
    //Configure Dependency Injection (DI)
    builder.Services.AddControllers();
    builder.Services.AddDbContext<ToDoItemsContext>();
    builder.Services.AddScoped<IRepository<ToDoItem>, ToDoItemsRepository>();
}

var app = builder.Build();
{
    //Configure Middleware (HTTP request pipeline - co vše se dívá na requesty, které přijdou od klienta)
    app.MapControllers();
}

app.Run();

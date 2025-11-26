using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args); //builder == postup, jak se aplikace vytvoří
{
    //Configure Dependency Injection Container (DIC)
    builder.Services.AddControllers();
    builder.Services.AddDbContext<ToDoItemsContext>();
    builder.Services.AddScoped<IRepositoryAsync<ToDoItem>, ToDoItemsRepository>();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{
    //Configure Middleware (HTTP request pipeline - co vše se dívá na requesty, které přijdou od klienta)
    app.MapControllers();
    app.UseSwagger();
    app.UseSwaggerUI(config => config.SwaggerEndpoint("v1/swagger.json", "ToDoList API V1"));
}

app.Run();

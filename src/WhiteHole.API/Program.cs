using WhiteHole.Repository;
using WhiteHole.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using WhiteHole.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContextPool<RepositoryContext>((serviceProvider, options) => {
    options.UseSqlServer("");
});
builder.Services.AddScoped<IWhiteHoleObjectRepository, WhiteHoleObjectRepository>();
builder.Services.AddScoped<IWhiteHoleObjectKVRepository, WhiteHoleObjectKVRepository>();
builder.Services.AddScoped<IWhiteHoleObjectRelationRepository, WhiteHoleObjectRelationRepository>();

builder.Services.AddScoped<IWhiteHoleCRUDServices, WhiteHoleCRUDServices>();
builder.Services.AddScoped<IWhiteHoleQueryServices, WhiteHoleQueryServices>();


var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseAuthorization();

app.MapControllers();

app.Run();

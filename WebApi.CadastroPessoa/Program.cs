using Microsoft.EntityFrameworkCore;
using WebApi.CadastroPessoa.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WebApiCadastroPessoaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebApiCadastroPessoaContext") 
        ?? throw new InvalidOperationException("Connection string 'WebApiCadastroPessoaContext' not found.")));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

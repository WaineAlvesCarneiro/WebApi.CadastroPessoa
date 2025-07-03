using Microsoft.EntityFrameworkCore;
using WebApi.CadastroPessoa.Models;

namespace WebApi.CadastroPessoa.Data
{
    public class WebApiCadastroPessoaContext : DbContext
    {
        public WebApiCadastroPessoaContext (DbContextOptions<WebApiCadastroPessoaContext> options)
            : base(options)
        {
        }

        public DbSet<Pessoa> Pessoa { get; set; } = default!;
    }
}

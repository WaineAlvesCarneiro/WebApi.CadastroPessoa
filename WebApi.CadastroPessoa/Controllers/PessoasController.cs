using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.CadastroPessoa.Data;
using WebApi.CadastroPessoa.Models;

namespace WebApi.CadastroPessoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly WebApiCadastroPessoaContext _context;

        public PessoasController(WebApiCadastroPessoaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoa()
        {
            return await _context.Pessoa.Include(p => p.Endereco).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pessoa>> GetPessoa(int id)
        {
            var pessoa = await _context.Pessoa.Include(p => p.Endereco).FirstOrDefaultAsync(m => m.Id == id);

            if (pessoa == null)
            {
                return NotFound();
            }

            return pessoa;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPessoa(int id, [FromBody] Pessoa pessoa)
        {
            if (id != pessoa.Id)
            {
                return BadRequest("ID da pessoa não corresponde.");
            }

            var pessoaExistente = await _context.Pessoa.Include(p => p.Endereco).FirstOrDefaultAsync(p => p.Id == id);

            if (pessoaExistente == null)
            {
                return NotFound();
            }

            pessoaExistente.Nome = pessoa.Nome;
            pessoaExistente.CpfCnpj = pessoa.CpfCnpj;
            pessoaExistente.Celular = ApenasNumeros(pessoa.Celular);
            pessoaExistente.Telefone = ApenasNumeros(pessoa.Telefone);
            pessoaExistente.Email = pessoa.Email;
            pessoaExistente.DtAniversario = pessoa.DtAniversario;
            pessoaExistente.Ativo = pessoa.Ativo;
            pessoaExistente.TipoPessoa = pessoa.TipoPessoa;
            pessoaExistente.EnderecoId = pessoa.EnderecoId;

            if (pessoaExistente.Endereco != null && pessoa.Endereco != null)
            {
                pessoaExistente.Endereco.Cep = ApenasNumeros(pessoa.Endereco.Cep);
                pessoaExistente.Endereco.Logradouro = pessoa.Endereco.Logradouro;
                pessoaExistente.Endereco.Numero = pessoa.Endereco.Numero;
                pessoaExistente.Endereco.Bairro = pessoa.Endereco.Bairro;
                pessoaExistente.Endereco.Cidade = pessoa.Endereco.Cidade;
                pessoaExistente.Endereco.Estado = pessoa.Endereco.Estado;
                pessoaExistente.Endereco.Complemento = pessoa.Endereco.Complemento;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PessoaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<Pessoa>> PostPessoa([FromBody] Pessoa pessoa)
        {
            pessoa.Endereco.Cep = ApenasNumeros(pessoa.Endereco.Cep);
            pessoa.Celular = ApenasNumeros(pessoa.Celular);
            pessoa.Telefone = ApenasNumeros(pessoa.Telefone);

            _context.Pessoa.Add(pessoa);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPessoa", new { id = pessoa.Id }, pessoa);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            var pessoa = await _context.Pessoa.FindAsync(id);
            var endereco = await _context.Endereco.FirstOrDefaultAsync(m => m.Id == pessoa.EnderecoId);

            if (pessoa == null)
            {
                return NotFound();
            }

            _context.Endereco.Remove(endereco);
            _context.Pessoa.Remove(pessoa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PessoaExists(int id)
        {
            return _context.Pessoa.Any(e => e.Id == id);
        }

        public static string ApenasNumeros(string valor)
        {
            var onlyNumber = "";
            foreach (var s in valor)
            {
                if (char.IsDigit(s))
                {
                    onlyNumber += s;
                }
            }
            return onlyNumber.Trim();
        }
    }
}

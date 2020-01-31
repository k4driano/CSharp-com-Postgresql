using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CeltaBlue.models;

namespace CeltaBlue.Controllers
{
    [Route("api/pessoas")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private readonly CeltaBlueContext _context;

        public PessoaController(CeltaBlueContext context)
        {
            _context = context;
        }

        // GET: api/Pessoa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas()
        {
            return await _context.Pessoas
                .Include(p => p.Cliente)
                .Include(p => p.Fornecedor)
                .ToListAsync();
        }

        // GET: api/Pessoa/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pessoa>> GetPessoa(long id)
        {
            var pessoa = await _context.Pessoas
                .Include(p => p.Cliente)
                .Include(p => p.Fornecedor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pessoa == null)
            {
                return NotFound();
            }

            return pessoa;
        }

        [HttpGet("clientes")]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetClientes()
        {
            return await _context.Pessoas
                .Include(p => p.Cliente)
                .ToListAsync();
        }

        [HttpGet("teste")]
        public async Task<ActionResult<IEnumerable<object>>> GetPessoaTeste([FromQuery] string f = null) {
            var pessoas = _context.Pessoas
                .Select(p => new {
                    Id = p.Id,
                    Nome = p.Nome,
                    Fantasia = p.Fantasia,
                    Limite = p.Cliente.LimiteCredito
                });

            if (!String.IsNullOrEmpty(f)) {
                pessoas = pessoas
                    .Where(
                        p => EF.Functions.Like(p.Nome, "%" + f + "%") || EF.Functions.Like(p.Fantasia, "%" + f + "%")
                    );
            }

            return await pessoas.ToListAsync();
        } 

        // PUT: api/Pessoa/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPessoa(long id, Pessoa pessoa)
        {
            if (id != pessoa.Id)
            {
                return BadRequest();
            }

            var cliente = await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            EntityVerify(cliente, pessoa.Cliente);

            var fornecedor = await _context.Fornecedores.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
            EntityVerify(fornecedor, pessoa.Fornecedor);

            _context.Entry(pessoa).State = EntityState.Modified;          

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

        // POST: api/Pessoa
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Pessoa>> PostPessoa(Pessoa pessoa)
        {
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPessoa), new { id = pessoa.Id }, pessoa);
        }

        // DELETE: api/Pessoa/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pessoa>> DeletePessoa(long id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null)
            {
                return NotFound();
            }

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();

            return pessoa;
        }

        private bool PessoaExists(long id)
        {
            return _context.Pessoas.Any(e => e.Id == id);
        }

        private void EntityVerify(object localEntity, object dbEntity) {
            if (localEntity == null && dbEntity != null) 
            {
                _context.Entry(dbEntity).State = EntityState.Added;
            } 
            else if (localEntity != null) 
            {
                if (dbEntity == null)
                {
                    _context.Remove(localEntity);
                }
                else
                {
                    _context.Entry(dbEntity).State = EntityState.Modified;
                }
            }
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CeltaBlue.models;

namespace CeltaBlue.Controllers
{
    [Route("api/fornecedores")]
    [ApiController]
    public class FornecedorController : ControllerBase
    {
        private readonly CeltaBlueContext _context;

        public FornecedorController(CeltaBlueContext context)
        {
            _context = context;
        }

        // GET: api/Pessoa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fornecedor>>> GetFornecedores()
        {
            return await _context.Fornecedores.Include(f => f.Pessoa).ToListAsync();
        }

        // GET: api/Pessoa/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fornecedor>> GetFornecedor(long id)
        {
            var fornecedor = await _context.Fornecedores.Include(f => f.Pessoa).FirstOrDefaultAsync(f => f.Id == id);

            if (fornecedor == null)
            {
                return NotFound();
            }

            return fornecedor;
        }       
    }
}

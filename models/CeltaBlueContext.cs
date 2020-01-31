using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CeltaBlue.models
{
    public class CeltaBlueContext : DbContext
    {
        public CeltaBlueContext(DbContextOptions<CeltaBlueContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var logger = LoggerFactory.Create(builder =>
            {
                builder
                    .AddConsole()
                    .AddEventLog();
            });
           optionsBuilder.UseLoggerFactory(logger);
        }        

        protected override void OnModelCreating(ModelBuilder builder) 
        {
            buildPessoa(builder);
            buildCliente(builder);
            buildFornecedor(builder);
        }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }

        private void buildPessoa(ModelBuilder builder) 
        {
            var _builder = builder.Entity<Pessoa>();
            _builder.ToTable("pessoas");
            _builder.Property(p => p.Id).HasColumnName("pessoa_id");
            _builder.Property(p => p.EmpresaId).HasColumnName("empresa_id");
            _builder.Property(p => p.Nome).HasColumnName("nome");
            _builder.Property(p => p.Fantasia).HasColumnName("fantasia");

            _builder.HasOne(p => p.Cliente).WithOne(c => c.Pessoa).HasForeignKey<Cliente>(c => c.Id);
            _builder.HasOne(p => p.Fornecedor).WithOne(f => f.Pessoa).HasForeignKey<Fornecedor>(f => f.Id);
        }

        private void buildCliente(ModelBuilder builder) {
            var _builder = builder.Entity<Cliente>();
            _builder.ToTable("clientes");
            _builder.Property(c => c.Id).HasColumnName("cliente_id");
            _builder.Property(c => c.LimiteCredito).HasColumnName("limite_credito");
        }

        private void buildFornecedor(ModelBuilder builder) 
        {
            var _builder = builder.Entity<Fornecedor>();
            _builder.ToTable("fornecedores");
            _builder.Property(f => f.Id).HasColumnName("fornecedor_id");
            _builder.Property(f => f.Teste).HasColumnName("teste");
        }
    }
}
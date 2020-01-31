namespace CeltaBlue.models
{
    public class Pessoa
    {
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public string Nome { get; set; }
        public string Fantasia { get; set; } 
        public Cliente Cliente { get; set; }
        public Fornecedor Fornecedor { get; set; }
    }

}
namespace CeltaBlue.Models
{
    public class Cliente
    {
        public long Id { get; set; }
        public double LimiteCredito { get; set; }
        public Pessoa Pessoa { get; set; }
    }
}
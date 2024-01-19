using System;

namespace Raizen.DataType
{
    public class ClienteVO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DtNascimento { get; set; }
        public string Cep { get; set; }
        public int FlAtivo { get; set; }
        public DateTime DtCriacao { get; set; }
        public DateTime DtAlteracao { get; set; }
        public int CriadoPor { get; set; }
        public int AlteradoPor { get; set; }
    }
}

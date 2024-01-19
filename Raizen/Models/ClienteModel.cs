using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Raizen.Models
{
    public class ClienteModel
    {
        public ClienteModel()
        {
            ListClientes = new List<ClienteModel>();
            Erros = new List<string>();
        }

        [Display(Name = "Código Cliente")]
        public int CdCliente { get; set; }

        [Display(Name = "Nome Cliente")]
        public string NmNome { get; set; }

        [Display(Name = "Email")]
        public string DsEmail { get; set; }

        [Display(Name = "Data Nascimento")]
        public DateTime DataNascimento { get; set; }

        [Display(Name = "CEP")]
        public string CEP { get; set; }

        [Display(Name = "Lista de Clientes")]
        public List<ClienteModel> ListClientes { get; set; }

        [Display(Name = "Ativo")]
        public int Ativo { get; set; }

        [Display(Name = "Data Criação")]
        public DateTime DataCriacao { get; set; }

        [Display(Name = "Data Alteração")]
        public DateTime DataAlteracao { get; set; }

        [Display(Name = "Criado Por")]
        public int CriadoPor { get; set; }

        [Display(Name = "Alterado Por")]
        public int AlteradoPor { get; set; }

        public List<string> Erros { get; set; }
    }
}
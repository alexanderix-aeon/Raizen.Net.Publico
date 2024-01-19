using System;
using System.Web;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Raizen.Models;
using Raizen.DataType;
using Raizen.Business;

namespace Raizen.Controllers
{
    public class ClienteController : Controller
    {
        public ClienteController()
        {
        }

        public ActionResult Index(long codCli = 0, string nome = null, string email = null)
        {
            ClienteModel model = new ClienteModel();
            try
            {
                if(!string.IsNullOrEmpty(nome) || !string.IsNullOrEmpty(email))
                {
                    model.ListClientes = ObterClientes(nome, email);
                }
                else
                {
                    model.ListClientes = ObterClientes(codCli);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                model.Erros.Clear();
                model.Erros.Add(ex.Message);
                return View(model);
            }
        }

        /// <summary>
        /// Obtem todas os clientes para carregar o grid na base da tela.
        /// </summary>
        /// <param name="codCli"></param>
        /// <returns></returns>
        [HttpGet]
        public List<ClienteModel> ObterClientes(long codCli)
        {
            var listCli = new List<ClienteModel>();
            IEnumerable<ClienteVO> listClienteVO;

            listClienteVO = ClienteBusiness.BuscarCliente();

            if (codCli != 0) listClienteVO = listClienteVO.Where(x => x.Id == codCli);

            foreach (ClienteVO item in listClienteVO)
            {
                var cli = new ClienteModel()
                {
                    CdCliente = item.Id,
                    NmNome = item.Nome,
                    DsEmail = item.Email,
                    DataNascimento = item.DtNascimento,
                    CEP = item.Cep,
                    DataCriacao = item.DtCriacao,
                    DataAlteracao = item.DtAlteracao
                };

                listCli.Add(cli);
            }

            return listCli;
        }

        /// <summary>
        /// Obtem todas os clientes para carregar o grid na base da tela.
        /// </summary>
        /// <param name="codCli"></param>
        /// <returns></returns>
        [HttpGet]
        private List<ClienteModel> ObterClientes(string nome = null, string email = null)
        {
            var listCli = new List<ClienteModel>();
            IEnumerable<ClienteVO> listClienteVO;

            listClienteVO = ClienteBusiness.BuscarCliente();

            if (!string.IsNullOrEmpty(nome))
            {
                listClienteVO = listClienteVO.Where(x => x.Nome.IndexOf(nome, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (!string.IsNullOrEmpty(email))
            {
                listClienteVO = listClienteVO.Where(x => x.Email.IndexOf(email, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            foreach (ClienteVO item in listClienteVO)
            {
                var cli = new ClienteModel()
                {
                    CdCliente = item.Id,
                    NmNome = item.Nome,
                    DsEmail = item.Email,
                    DataNascimento = item.DtNascimento,
                    CEP = item.Cep,
                    DataCriacao = item.DtCriacao,
                    DataAlteracao = item.DtAlteracao
                };

                listCli.Add(cli);
            }

            return listCli;
        }

        /// <summary>
        /// Exibe os dados do cliente selecionado.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Visualizar(long id)
        {
            try
            {
                var cliente = ObterClientes(id).FirstOrDefault();
                if (cliente != null)
                {
                    return Json(cliente, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                // Tratar o erro conforme necessário
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        /// <summary>
        /// Exclui um novo cliente.
        /// </summary>
        /// <param name="oCliente"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Excluir(int cdCliente)
        {
            try
            {
                ClienteBusiness.ExcluirCliente(cdCliente);

                ClienteModel oCliente = new ClienteModel();

                oCliente.ListClientes = ObterClientes(0).ToList();

                return Json(oCliente);
            }
            catch (Exception ex)
            {
                return Json(new { Codigo = 1, Erro = ex.Message });
            }
        }

        /// <summary>
        /// Salva um novo cliente ou edita.
        /// </summary>
        /// <param name="oCliente"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Salvar(ClienteModel oCliente)
        {
            try
            {
                ClienteVO clienteVO = new ClienteVO
                {
                    Id = oCliente.CdCliente,
                    Nome = oCliente.NmNome,
                    Email = oCliente.DsEmail,
                    DtNascimento = oCliente.DataNascimento,
                    Cep = oCliente.CEP,
                    DtCriacao = oCliente.DataCriacao,
                    DtAlteracao = oCliente.DataAlteracao,
                    CriadoPor = oCliente.CriadoPor,
                    AlteradoPor = oCliente.AlteradoPor
                };

                ValidarDados(ref oCliente);

                if (oCliente.Erros.Count == 0)
                {
                    var modelClienteExist = ObterClientes(oCliente.CdCliente).FirstOrDefault();
                    if (modelClienteExist == null)
                    {
                        clienteVO.DtCriacao = DateTime.Now;
                        clienteVO.DtAlteracao = DateTime.Now;
                        clienteVO.CriadoPor = 1;
                        clienteVO.AlteradoPor = 1;
                        ClienteBusiness.InserirCliente(clienteVO);
                    }
                    else
                    {
                        clienteVO.DtAlteracao = DateTime.Now;
                        clienteVO.AlteradoPor = 1;
                        ClienteBusiness.EditarCliente(clienteVO);
                    }

                    oCliente.ListClientes = ObterClientes(0).ToList();

                    return View("Index", oCliente);
                }
                else
                    return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { Codigo = 1, Erro = ex.Message });
            }
        }

        /// <summary>
        /// Valida obrigatoriedade dos campos na inserção ou edição.
        /// </summary>
        /// <param name="oCliente"></param>
        private void ValidarDados(ref ClienteModel oCliente)
        {
            oCliente.Erros.Clear();

            if (string.IsNullOrWhiteSpace(oCliente.NmNome))
                oCliente.Erros.Add("Nome deve ser preenchido.<br/>");
            if (string.IsNullOrWhiteSpace(oCliente.DsEmail))
                oCliente.Erros.Add("Email deve ser preenchido.<br/>");
            if (string.IsNullOrWhiteSpace(oCliente.CEP))
                oCliente.Erros.Add("CEP deve ser preenchido.<br/>");
            if (oCliente.DataNascimento == DateTime.MinValue)
                oCliente.Erros.Add("Data de nascimento deve ser preenchido.<br/>");
        }
    }
}
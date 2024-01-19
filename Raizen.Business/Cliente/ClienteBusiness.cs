using System.Collections.Generic;
using Raizen.DataType;
using Raizen.Repository;

namespace Raizen.Business
{
    public static class ClienteBusiness
    {
        public static IEnumerable<ClienteVO> BuscarCliente()
        {
            return ClienteDAO.BuscarCliente();
        }

        /// <summary>
        /// Insere cliente.
        /// </summary>
        /// <returns></returns>
        public static string InserirCliente(ClienteVO oCliente)
        {
            return ClienteDAO.InserirCliente(oCliente);
        }

        /// <summary>
        /// Edita cliente.
        /// </summary>
        /// <returns></returns>
        public static string EditarCliente(ClienteVO oCliente)
        {
            return ClienteDAO.EditarCliente(oCliente);
        }

        /// <summary>
        /// Inativa cliente.
        /// </summary>
        /// <returns></returns>
        public static string ExcluirCliente(int id)
        {
            return ClienteDAO.ExcluirCliente(id);
        }
    }
}

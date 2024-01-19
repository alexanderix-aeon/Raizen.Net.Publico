using System;
using System.Data;
using System.Collections.Generic;
using Raizen.DataType;

namespace Raizen.Repository
{
    public static class ClienteDAO
    {
        private static AbstractDBAdapter sqlAdapter = new SqlServerAdapter("RAIZENDB");

        #region Buscar

        public static IEnumerable<ClienteVO> BuscarCliente()
        {
            using (var cnx = sqlAdapter.OpenConnection())
            {
                using (var command = sqlAdapter.CreateCommand(SELECT_CLIENTE, cnx))
                {
                    command.CommandType = CommandType.Text;

                    return DataExtensions.FetchObjects(command, MaterializeCliente);
                }
            }
        }

        #endregion

        #region Inserir

        public static string InserirCliente(ClienteVO _dados)
        {
            try
            {
                using (var cnx = sqlAdapter.OpenConnection())
                {
                    using (var command = sqlAdapter.CreateCommand(INSERT_CLIENTE, cnx))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(sqlAdapter.CreateParameter("@Nome", _dados.Nome));
                        command.Parameters.Add(sqlAdapter.CreateParameter("@Email", _dados.Email));
                        command.Parameters.Add(sqlAdapter.CreateParameter("@DtNascimento", _dados.DtNascimento));
                        command.Parameters.Add(sqlAdapter.CreateParameter("@Cep", _dados.Cep));
                        command.Parameters.Add(sqlAdapter.CreateParameter("@DtCriacao", _dados.DtCriacao));
                        command.Parameters.Add(sqlAdapter.CreateParameter("@DtAlteracao", _dados.DtAlteracao));
                        command.Parameters.Add(sqlAdapter.CreateParameter("@CriadoPor", _dados.CriadoPor));
                        command.Parameters.Add(sqlAdapter.CreateParameter("@AlteradoPor", _dados.AlteradoPor));

                        return command.ExecuteNonQuery() == -1 ? "Dados não inseridos na tabela Cliente" : "";
                    }
                }
            }
            catch (Exception ex)
            {
                return string.Format("Dados não inseridos na tabela Cliente : {0}", ex.Message);
            }
        }

        #endregion

        #region Editar

        public static string EditarCliente(ClienteVO _dados)
        {
            try
            {
                using (var cnx = sqlAdapter.OpenConnection())
                {
                    using (var command = sqlAdapter.CreateCommand(UPDATE_CLIENTE, cnx))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(sqlAdapter.CreateParameter("@Id", _dados.Id));
                        command.Parameters.Add(sqlAdapter.CreateParameter("@Nome", _dados.Nome));
                        command.Parameters.Add(sqlAdapter.CreateParameter("@Email", _dados.Email));
                        command.Parameters.Add(sqlAdapter.CreateParameter("@DtNascimento", _dados.DtNascimento));
                        command.Parameters.Add(sqlAdapter.CreateParameter("@Cep", _dados.Cep));
                        command.Parameters.Add(sqlAdapter.CreateParameter("@DtAlteracao", _dados.DtAlteracao));
                        command.Parameters.Add(sqlAdapter.CreateParameter("@AlteradoPor", _dados.AlteradoPor));

                        return command.ExecuteNonQuery() == -1 ? "Dados não atualizados na tabela Cliente" : "";
                    }
                }
            }
            catch (Exception ex)
            {
                return string.Format("Dados não atualizados na tabela Cliente : {0}", ex.Message);
            }
        }

        #endregion

        #region Excluir

        public static string ExcluirCliente(int id)
        {
            try
            {
                using (var cnx = sqlAdapter.OpenConnection())
                {
                    using (var command = sqlAdapter.CreateCommand(DELETE_CLIENTE, cnx))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(sqlAdapter.CreateParameter("@Id", id));

                        return command.ExecuteNonQuery() == -1 ? "Dados não excluidos na tabela Cliente" : "";
                    }
                }
            }
            catch (Exception ex)
            {
                return string.Format("Dados não excluidos na tabela Cliente : {0}", ex.Message);
            }
        }

        #endregion

        #region Materialize

        internal static ClienteVO MaterializeCliente(IDataRecord record)
        {
            return new ClienteVO
            {
                Id = record.Field<int>("ClienteId"),
                Nome = record.Field<string>("Nome"),
                Email = record.Field<string>("Email"),
                DtNascimento = record.Field<DateTime>("DataNascimento"),
                Cep = record.Field<string>("CEP"),
                FlAtivo = record.Field<int>("Ativo"),
                DtCriacao = record.Field<DateTime>("DataCriacao"),
                DtAlteracao = record.Field<DateTime>("DataAlteracao"),
                CriadoPor = record.Field<int>("CriadoPor"),
                AlteradoPor = record.Field<int>("AlteradoPor")
            };
        }

        #endregion

        #region Comandos

        //private const string SELECT_CLIENTE = @"SELECT 
        //                                            CD_PRACA [ClienteID],
        //                                            NM_PRACA [Nome],
        //                                            'ck_aandrade@hotmail.com' [Email],
        //                                            GETDATE() [DataNascimento],
        //                                            '13050-513' [CEP],
        //                                            CASE WHEN FL_ATIVO = 'A' THEN 1 ELSE 0 END [Ativo],
        //                                            GETDATE() [DataCriacao],
        //                                            GETDATE() [DataAlteracao],
        //                                            1 [CriadoPor],
        //                                            1 [AlteradoPor]
        //                                       FROM dbo.TB_PRACA
        //                                      ORDER BY 2";

        private const string SELECT_CLIENTE = @"SELECT ClienteId,
	                                                   Nome,
	                                                   Email,
	                                                   DataNascimento,
	                                                   CEP,
	                                                   Ativo,
	                                                   DataCriacao,
	                                                   DataAlteracao,
	                                                   CriadoPor,
	                                                   AlteradoPor
                                                  FROM dbo.tblCliente
                                                 WHERE Ativo = 1
                                                 ORDER BY Nome";

        private const string INSERT_CLIENTE = @" INSERT INTO dbo.tblCliente (
	                                                   Nome,
	                                                   Email,
	                                                   DataNascimento,
	                                                   CEP,
	                                                   Ativo,
	                                                   DataCriacao,
	                                                   DataAlteracao,
	                                                   CriadoPor,
	                                                   AlteradoPor)
                                                 VALUES (
	                                                   @Nome,
	                                                   @Email,
	                                                   @DtNascimento,
	                                                   @Cep,
	                                                   1,
	                                                   @DtCriacao,
	                                                   @DtAlteracao,
	                                                   @CriadoPor,
	                                                   @AlteradoPor)";

		private const string UPDATE_CLIENTE = @" UPDATE dbo.tblCliente
                                                    SET Nome = @Nome,
	                                                    Email = @Email,
	                                                    DataNascimento = @DtNascimento,
	                                                    CEP = @Cep,
	                                                    DataAlteracao = @DtAlteracao,
	                                                    AlteradoPor = @AlteradoPor
                                                  WHERE ClienteId = @Id ";

		private const string DELETE_CLIENTE = @" UPDATE dbo.tblCliente
                                                 SET Ativo = 0
                                                 WHERE ClienteId = @Id ";

		#endregion
	}
}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace Raizen.Api.Repository
{
    public abstract class AbstractDBAdapter
    {
        const string SEQUENTIAL_UPDATE_QUERY = "UPDATE TbSequencial SET NuSequencialDisponivel = @NuSequencialDisponivel WHERE CdSequencial = @CdSequencial";
        const string SEQUENTIAL_SELECT_QUERY = "SELECT CdSequencial, NuSequencialDisponivel FROM TbSequencial WHERE NmSequencial = @NmSequencial";
        const string SEQUENTIAL_SELECT_QUERY_NEW = "SELECT MAX({0}) FROM {1}";

        public string Name { get; private set; }

        protected AbstractDBAdapter(string name)
        {
            this.Name = name;
        }

        public abstract IDbConnection OpenConnection();
        public abstract IDbCommand CreateCommand(string query, IDbConnection connection);
        public abstract IDbDataParameter CreateParameter(string name, object value);

        public int NextSequential(string sequentialName)
        {
            return NextSequential(sequentialName, 1);
        }

        public int NextSequential(string sequentialName, short quantity)
        {
            if (quantity < 1)
            {
                throw new Exception("Invalid sequential quantity: " + quantity);
            }

            int sequential = -1;
            int cdSequential = -1;

            using (var tran = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                using (var cnx = OpenConnection())
                {
                    // Get Sequential 
                    using (IDbCommand command = CreateCommand(SEQUENTIAL_SELECT_QUERY, cnx))
                    {
                        command.Parameters.Add(CreateParameter("@NmSequencial", sequentialName));
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                sequential = (int)reader["NuSequencialDisponivel"];
                                cdSequential = (int)reader["CdSequencial"];
                            }
                            else
                            {
                                throw new Exception("Invalid sequential name: " + sequentialName);
                            }
                        }
                    }

                    // Update Table
                    using (IDbCommand command = CreateCommand(SEQUENTIAL_UPDATE_QUERY, cnx))
                    {
                        command.Parameters.Add(CreateParameter("@NuSequencialDisponivel", sequential + quantity));
                        command.Parameters.Add(CreateParameter("@CdSequencial", cdSequential));
                        command.ExecuteNonQuery();
                    }
                }
                tran.Complete();
            }
            return sequential;
        }

        public int NextSequential(string tableName, string pkColumnName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new Exception("Invalid table name: " + tableName);
            }

            if (string.IsNullOrEmpty(pkColumnName))
            {
                throw new Exception("Invalid primary key column name: " + pkColumnName);
            }

            int newSequential = -1;

            using (var tran = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var cnx = OpenConnection())
                {
                    // Get Sequential 
                    string sqlSequential = string.Format(SEQUENTIAL_SELECT_QUERY_NEW, pkColumnName, tableName);
                    using (IDbCommand command = CreateCommand(sqlSequential, cnx))
                    {
                        newSequential = (int)command.ExecuteScalar() + 1;
                    }
                }
                tran.Complete();
            }
            return newSequential;
        }

        protected string ConnectionString()
        {
            return DBSettingsHelper.ConnectionString(this.Name);
        }
    }
}

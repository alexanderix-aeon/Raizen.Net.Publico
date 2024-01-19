using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace Raizen.Repository
{
    public abstract class AbstractDBAdapter
    {
        public string Name { get; private set; }

        protected AbstractDBAdapter(string name)
        {
            this.Name = name;
        }

        public abstract IDbConnection OpenConnection();
        public abstract IDbCommand CreateCommand(string query, IDbConnection connection);
        public abstract IDbDataParameter CreateParameter(string name, object value);

        protected string ConnectionString()
        {
            return DBSettingsHelper.ConnectionString(this.Name);
        }
    }
}

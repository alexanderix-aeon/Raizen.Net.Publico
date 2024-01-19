using System;
using System.Data;
using System.Data.SqlClient;

namespace Raizen.Api.Repository
{
    public sealed class SQLServerAdapter : AbstractDBAdapter
    {
        public SQLServerAdapter(string name) : base(name) { }

        public override IDbConnection OpenConnection()
        {
            var cnx = new SqlConnection(this.ConnectionString());
            cnx.Open();
            return cnx;
        }

        public override IDbCommand CreateCommand(string query, IDbConnection connection)
        {
            return new SqlCommand(query, (SqlConnection)connection);
        }

        public override IDbDataParameter CreateParameter(string name, object value)
        {
            if (!name.StartsWith("@"))
            {
                name.Insert(0, "@");
            }
            if (value == null)
            {
                return new SqlParameter(name, DBNull.Value);
            }
            return new SqlParameter(name, value);
        }
    }
}

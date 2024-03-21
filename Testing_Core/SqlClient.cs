using System.Data.SqlClient;
using System.Data;

namespace Testing_Core
{
    public class SqlClient : IDisposable
    {
        private string ConnectionString { get; set; } = "Data Source=EPINHYDW0148\\SQLEXPRESS;Initial Catalog=Sojan_Test;User ID=sa;Password=Welcome123!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;";
        private static SqlClient sqlClient { get; set; } = null;
        private SqlConnection Connection { get; set; } = null;

        private SqlClient()
        {
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }

        public static SqlClient GetInstance()
        {
            sqlClient ??= new SqlClient();
            return sqlClient;
        }

        public SqlConnection GetConnection()
        {
            Connection ??= new SqlConnection(ConnectionString);
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
            return Connection;
        }

        public DataTable GetQueryResultDataTable(string query)
        {
            DataTable dataTable = new();
            using SqlCommand sqlCommand = new(query, GetConnection());
            using SqlDataAdapter adapter = new(sqlCommand);
            adapter.Fill(dataTable);
            return dataTable;
        }

        public SqlDataReader GetQueryResultSqlDataReader(string query)
        {
            using SqlCommand sqlCommand = new(query, GetConnection());
            return sqlCommand.ExecuteReader();
        }

        public bool Validate(SqlDataReader sqlDataReader, string colum, string value)
        {
            while (sqlDataReader.Read())
            {
                string name = sqlDataReader.GetString(colum);
                if (name.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        public void CloseDatabaseConnection(SqlConnection connection)
        {
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        public void ExecuteNonQuery(SqlConnection connection, string query)
        {
            using var command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
        }

        public DataTable ExecuteReader(SqlConnection connection, string query)
        {
            var command = new SqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public List<string> GetColumnValues(DataTable dataTable, string columnName)
        {
            List<string> columnValues = [];
            if (dataTable.Columns.Contains(columnName))
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    string? value = row[columnName]?.ToString();
                    columnValues.Add(value!);
                }
            }
            return columnValues;
        }
    }
}
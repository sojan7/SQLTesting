using System.Data;
using System.Data.SqlClient;

namespace Testing_Core.SQL
{
    public class SqlBase(string connectionString)
    {
        private readonly string _connectionString = connectionString;

        public SqlConnection CreateSqlConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public void OpenDatabaseConnection(SqlConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
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
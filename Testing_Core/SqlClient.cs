﻿using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace Testing_Core
{
    public class SqlClient : IDisposable
    {
        private static IConfiguration? Config { get; set; } = null;
        private string ConnectionString { get; set; }
        private static SqlClient? SqlClientInstance { get; set; } = null;
        private SqlConnection? Connection { get; set; } = null;

        private SqlClient()
        {
            var configurationFilePath = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.Parent!.FullName + "\\Testing_Core\\appsettings.json";
            Config ??= new ConfigurationBuilder()
              .AddJsonFile(configurationFilePath, optional: false, reloadOnChange: true)
              .Build();
            ConnectionString = Config["DataBaseConnectionStrings:Sojan_TestConnectionString"]!;
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }

        public static SqlClient GetInstance()
        {
            SqlClientInstance ??= new SqlClient();
            return SqlClientInstance;
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

        public DataTable GetQueryResultDataTable(string query, Dictionary<string, string> parameterDictionary)
        {
            DataTable dataTable = new();
            using SqlCommand sqlCommand = new(query, GetConnection());
            foreach (var item in parameterDictionary)
            {
                sqlCommand.Parameters.AddWithValue(item.Key, item.Value);
            }

            using SqlDataAdapter adapter = new(sqlCommand);
            adapter.Fill(dataTable);
            return dataTable;
        }

        public bool ValidateASpecificData(DataTable dataTable, string expectedValue)
        {
            return dataTable.Rows[0].ItemArray[0]!.ToString() == expectedValue;
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
                var name = sqlDataReader.GetString(sqlDataReader.GetOrdinal(colum));
                if (name.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        public void CloseDatabaseConnection()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
            }
        }

        public int ExecuteNonQuery(string query)
        {
            using var command = new SqlCommand(query, GetConnection());
            return command.ExecuteNonQuery();
        }

        public DataTable ExecuteReader(string query)
        {
            var command = new SqlCommand(query, GetConnection());
            using var reader = command.ExecuteReader();
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public List<string> GetColumnValues(DataTable dataTable, string columnName)
        {
            var columnValues = new List<string>();
            if (dataTable.Columns.Contains(columnName))
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    var value = row[columnName]?.ToString();
                    columnValues.Add(value!);
                }
            }
            return columnValues;
        }

        public bool DoesTableExist(string tableName)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand($"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'", connection);
            int count = (int)command.ExecuteScalar();
            return count > 0;
        }

        public int CreateDatabase(string databaseName)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand($"CREATE DATABASE {databaseName}", connection);
            return command.ExecuteNonQuery();
        }

        public int DropDatabase(string databaseName)
        {
            using var connection = GetConnection();
            using var command = new SqlCommand($"DROP DATABASE {databaseName}", connection);
            return command.ExecuteNonQuery();
        }
    }
}
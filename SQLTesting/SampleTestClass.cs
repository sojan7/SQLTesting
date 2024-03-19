using NUnit.Framework;
using Testing_Core.SQL;

namespace SQLTesting
{
    [TestFixture]
    public class SampleTestClass
    {
        [Test, Category("Sample Tests")]
        public void SampleTest()
        {
            string connectionString = "Data Source=EPINHYDW0148\\SQLEXPRESS;Initial Catalog=Sojan_Test;User ID=sa;Password=Welcome123!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;";
            string query = "SELECT * FROM [Test_Users] WITH(NOLOCK)";

            var sqlBase = new SqlBase(connectionString);
            var connection = sqlBase.CreateSqlConnection();
            sqlBase.OpenDatabaseConnection(connection);
            var data = sqlBase.ExecuteReader(connection, query);
            sqlBase.CloseDatabaseConnection(connection);
            var numberOfRows = data.Rows.Count;
            Assert.That(numberOfRows, Is.GreaterThan(1));
            var a = sqlBase.GetColumnValues(data, "UserID");
        }
    }
}
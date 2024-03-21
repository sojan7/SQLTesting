using NUnit.Framework;
using Testing_Core;

namespace SQLTesting
{
    [TestFixture]
    public class SampleTestClass
    {
        [Test, Category("Database Tests"), Order(1)]
        public void VerifyIsConnectionEstablished()
        {
            Assert.That(SqlClient.GetInstance().GetConnection(), Is.Not.Null);
        }

        [Test, Category("Database Tests"), Order(2)]
        public void VerifyQuery()
        {
            string query1 = "SELECT * FROM [Test_Users] WITH(NOLOCK) Where UserID < 10";
            string query2 = "SELECT * FROM [Test_Users] WITH(NOLOCK) Where UserID > 10 AND UserID < 20 ;";
            Assert.That(SqlClient.GetInstance().GetQueryResultDataTable(query1), Is.Not.Null);
            Assert.That(SqlClient.GetInstance().GetQueryResultDataTable(query2), Is.Not.Null);
        }
    }
}
using DataBase_BusinessLayer;
using DataBase_BusinessLayer.DataModels;
using NUnit.Framework;
using Testing_Core;

namespace SQLTesting
{
    [TestFixture]
    public class SampleTestClass
    {
        private readonly BusinessLogic businessLogic;

        public SampleTestClass()
        {
            businessLogic = new();
        }

        [Test, Category("Database Tests"), Order(1)]
        public void VerifyIsConnectionEstablished()
        {
            Assert.That(SqlClient.GetInstance().GetConnection(), Is.Not.Null);
        }

        [Test, Category("Database Tests"), Order(2)]
        public void VerifyQuery()
        {
            Assert.That(SqlClient.GetInstance().GetQueryResultDataTable(SQLQueries.GetAllUserDetailsWithLessThanUserID_10), Is.Not.Null);
            Assert.That(SqlClient.GetInstance().GetQueryResultDataTable(SQLQueries.GetAllUserDetailsBetweenUserIDs_10_20), Is.Not.Null);
        }

        [Test, Category("Database Tests"), Order(3)]
        public void VerifyData()
        {
            var data = SqlClient.GetInstance().GetQueryResultDataTable(SQLQueries.GetAllUserDetailsWithLessThanUserID_10);
            var dataList = businessLogic.ConvertDataTableToList<User>(data);
            var actualUserIds = dataList.Select(x => x.UserID).ToList();
            var expectedUserIds = Enumerable.Range(1, 9).ToList();
            Assert.That(actualUserIds.SequenceEqual(expectedUserIds), Is.True);
        }

        [Test, Category("Database Tests"), Order(4)]
        public void ValidateASpecificData()
        {
            var query = "SELECT [EmailID] FROM [Test_Users] WITH(NOLOCK) Where UserID = @UserID";
            Dictionary<string, string> parameterList = new()
            {
                { "@UserID","1"}
            };
            var data = SqlClient.GetInstance().GetQueryResultDataTable(query, parameterList);
            Assert.That(SqlClient.GetInstance().ValidateASpecificData(data, "user1@gmail.com.test"), Is.True);
        }
    }
}
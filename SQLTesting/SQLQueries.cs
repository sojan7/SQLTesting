namespace SQLTesting
{
    public static class SQLQueries
    {
        public const string GetAllUserDetailsWithLessThanUserID_10 = "SELECT * FROM [Test_Users] WITH(NOLOCK) Where UserID < 10";
        public const string GetAllUserDetailsBetweenUserIDs_10_20 = "SELECT * FROM [Test_Users] WITH(NOLOCK) Where UserID > 10 AND UserID < 20";
        public const string GetEmailIdForASpecificUser = "SELECT [EmailID] FROM [Test_Users] WITH(NOLOCK) Where UserID = @UserID";
    }
}
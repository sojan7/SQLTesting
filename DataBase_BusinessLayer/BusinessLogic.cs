using System.Data;

namespace DataBase_BusinessLayer
{
    public class BusinessLogic
    {
        public List<T> ConvertDataTableToList<T>(DataTable dt) where T : new()
        {
            List<T> data = [];
            foreach (DataRow row in dt.Rows)
            {
                T item = new();
                foreach (DataColumn column in dt.Columns)
                {
                    var property = typeof(T).GetProperty(column.ColumnName);
                    property?.SetValue(item, Convert.ChangeType(row[column.ColumnName], property.PropertyType));
                }

                data.Add(item);
            }

            return data;
        }
    }
}
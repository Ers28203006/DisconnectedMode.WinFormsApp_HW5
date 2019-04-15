using System.Data;
using System.Data.Common;
using System.Configuration;

namespace DisconnectedMode.DataAccess
{
    public class UserTableDataService
    {
        public static void UserTableData()
        {

            var configuration = ConfigurationManager.ConnectionStrings["appConnection"];

            var providerName = configuration.ProviderName;
            var connectionString = configuration.ConnectionString;
            var providerFactory = DbProviderFactories.GetFactory(providerName);

            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                var dataSet = new DataSet("users");
                var dataAdapter = providerFactory.CreateDataAdapter();

                var selectUsersCommand = connection.CreateCommand();
                selectUsersCommand.CommandText = "Select * from Users";
                dataAdapter.SelectCommand = selectUsersCommand;

                dataAdapter.Fill(dataSet, "Users");

                var commandBuilder = providerFactory.CreateCommandBuilder();
                commandBuilder.DataAdapter = dataAdapter;
                var usersTable = dataSet.Tables["Users"];
                var row = usersTable.Rows[0];
                row.BeginEdit();
                row["Login"] = "superUser";
                row.EndEdit();

                dataAdapter.Update(dataSet, "Users");
            }
        }
    }
}

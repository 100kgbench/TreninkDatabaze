using Microsoft.Data.Sqlite;
using TreninkDatabaze;
public class Program
{
    public static async Task Main(string[] args)
    {
        var db = new DatabaseHandler();
      //await db.CreateTablePlanAsync();
      //await db.CreateTableCvikAsync();
      //  await db.CreateTableTreninkAsync();
      //  await db.AddTreninkAsync(1,1,1);
      //await db.AddTreninkPlanAsync("A", "silovy", "nohy");
      //await db.AddCvikAsync()
      //await db.ReadDataPlan();
      //await db.ReadDataCvik();
      //await db.ReadDataTrenink();
      //  await db.DropTable("Trenink");
        await db.TreninkJoin();

    }
}
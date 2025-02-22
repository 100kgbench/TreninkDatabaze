using Microsoft.Data.Sqlite;
using TreninkDatabaze;
public class Program
{
    public static async Task Main(string[] args)
    {
        var db = new DatabaseHandler();
        await db.CreateTablePlanAsync();
        await db.CreateTableCvikAsync();
        await db.CreateTableTreninkAsync();
     /* await db.AddTreninkAsync();
        await db.AddCvikAsync();
        await db.AddTreninkPlanAsync();
        await db.ReadDataPlan();
        await db.ReadDataCvik();
        await db.ReadDataTrenink();*/
        
    }
}
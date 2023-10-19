using System.Text.Json;
using System.IO;
namespace sea_battle;
public class Data
{
    private string DbPath { get; set; } = "";
    
    public Data(string dbPath = "records.json")
    {
        DbPath = dbPath;
    }
    public List<PlayerData> GetRecords()
    {
        if (File.Exists(DbPath))
        {
            string json = File.ReadAllText(DbPath);
            return JsonSerializer.Deserialize<List<PlayerData>>(json) ?? new List<PlayerData>();
        }
        else
        {
            return new List<PlayerData>();
        }
    }

    

    public void CreateOrUpdateRecords(string name, int score, int deadShips)
    {
        var users = GetRecords();
        var user = users.Find(usr => usr.Name == name);
        if (user == null)
        {
            users.Add(new PlayerData(name, score, deadShips));
        }
        else
        {
            user.Score = score;
            user.DeadShips = deadShips;
        }

        SaveRecords(users);
    }
    
    
    
    
    
    
    public void SaveRecords(List<PlayerData> records)
    {
        string json = JsonSerializer.Serialize(records);
        File.WriteAllText(DbPath, json);
    }
   
}

public class PlayerData
{
    public string Name { get; set; }
    public int Score { get; set; }
    public int DeadShips { get; set; }

    public PlayerData(string name, int score, int deadShips)
    {
        Name = name;
        Score = score;
        DeadShips = deadShips;
    }

}
using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;


namespace sea_battle
{
    public class Data // Data Layer
    {
        private string DbPath { get; set; } = "";
        
        public Data(string dbPath = "records.json")
        {
            DbPath = dbPath;
        }

        public void AddRecord(PlayerData playerData)
        {
            List<PlayerData> records = GetRecords();
            records.Add(playerData);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true  // Включает форматирование с отступами
            };
            string json = JsonSerializer.Serialize(records, options);
            File.WriteAllText(DbPath, json);
        }

        private List<PlayerData> GetRecords()
        {
            if (!File.Exists(DbPath)) return new List<PlayerData>();

            string json = File.ReadAllText(DbPath);

            try
            {
                return JsonSerializer.Deserialize<List<PlayerData>>(json);
            }
            catch (JsonException)
            {
                return new List<PlayerData>();
            }
        }

        public List<PlayerData> GetRecordsByName(string playerName)
        {
            // Получить все записи
            List<PlayerData> records = GetRecords();

            // Использовать LINQ для фильтрации записей по имени пользователя
            List<PlayerData> filteredRecords = records.Where(record => record.Name == playerName).ToList();

            return filteredRecords;
        }
    }

    public class PlayerData
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Result { get; set; }

        public PlayerData() { }
        public PlayerData(string name, DateTime dateTime, string result)
        {
            Name = name;
            Date = dateTime.ToString("yyyy-MM-dd");
            Time = dateTime.ToString("HH:mm");
            Result = result;
        }
    }
}
using GoalGridApp.Data;
using System.Text.Json;

namespace GoalGridApp.Services
{
    public class JsonFileStorageService
    {
        private readonly string _filePath;

        public JsonFileStorageService()
        {
            _filePath = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");
        }

        
        public async Task SaveTasksAsync(Dictionary<DateTime, List<DailyTask>> weeklyTasks)
        {
            var serializableData = new Dictionary<string, List<DailyTask>>();
            foreach (var entry in weeklyTasks)
            {
                serializableData[entry.Key.ToString("yyyy-MM-dd")] = entry.Value;
            }

            var jsonString = JsonSerializer.Serialize(serializableData, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, jsonString);
        }

        
        public async Task<Dictionary<DateTime, List<DailyTask>>> LoadTasksAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new Dictionary<DateTime, List<DailyTask>>();
            }

            try
            {
                var jsonString = await File.ReadAllTextAsync(_filePath);
                var deserializedData = JsonSerializer.Deserialize<Dictionary<string, List<DailyTask>>>(jsonString);

                var loadedTasks = new Dictionary<DateTime, List<DailyTask>>();
                if (deserializedData != null)
                {
                    foreach (var entry in deserializedData)
                    {
                        if (DateTime.TryParse(entry.Key, out DateTime dateKey))
                        {
                            loadedTasks[dateKey.Date] = entry.Value;
                        }
                    }
                }
                return loadedTasks;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar tareas desde JSON: {ex.Message}");
                return new Dictionary<DateTime, List<DailyTask>>();
            }
        }
    }
}
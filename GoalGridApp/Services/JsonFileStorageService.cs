using GoalGridApp.Components.Pages;
using System.Text.Json;
using System.Text.Json.Serialization; // Necesario para JsonPropertyName (opcional, pero buena práctica)

namespace GoalGridApp.Services // Asegúrate de que el namespace sea correcto
{
    public class JsonFileStorageService
    {
        private readonly string _filePath;

        public JsonFileStorageService()
        {
            // Ruta donde se guardará el archivo JSON en el almacenamiento de la aplicación.
            // Esto es compatible con .NET MAUI en diferentes plataformas.
            _filePath = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");
        }

        public async Task SaveTasksAsync(Dictionary<DateTime, List<WeeklyPlanner.DailyTask>> weeklyTasks)
        {
            // Convertir las claves DateTime a string para que System.Text.Json las maneje correctamente.
            // System.Text.Json tiene problemas para serializar Dictionary<DateTime, ...> directamente.
            var serializableData = new Dictionary<string, List<WeeklyPlanner.DailyTask>>();
            foreach (var entry in weeklyTasks)
            {
                serializableData[entry.Key.ToString("yyyy-MM-dd")] = entry.Value;
            }

            var jsonString = JsonSerializer.Serialize(serializableData, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, jsonString);
            Console.WriteLine($"Tareas guardadas en: {_filePath}");
        }

        public async Task<Dictionary<DateTime, List<WeeklyPlanner.DailyTask>>> LoadTasksAsync()
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("Archivo tasks.json no encontrado, retornando diccionario vacío.");
                return new Dictionary<DateTime, List<WeeklyPlanner.DailyTask>>();
            }

            try
            {
                var jsonString = await File.ReadAllTextAsync(_filePath);
                // Deserializar a un diccionario con claves string y luego convertir a DateTime.
                var deserializedData = JsonSerializer.Deserialize<Dictionary<string, List<WeeklyPlanner.DailyTask>>>(jsonString);

                var loadedTasks = new Dictionary<DateTime, List<WeeklyPlanner.DailyTask>>();
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
                Console.WriteLine($"Tareas cargadas desde: {_filePath}");
                return loadedTasks;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar tareas desde JSON: {ex.Message}");
                // Opcional: eliminar el archivo si está corrupto
                // File.Delete(_filePath);
                return new Dictionary<DateTime, List<WeeklyPlanner.DailyTask>>();
            }
        }
    }
}
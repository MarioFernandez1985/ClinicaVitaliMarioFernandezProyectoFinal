
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using ClinicaVitaliApi.Models;

namespace ClinicaVitaliApi.Data
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }

    public class JsonRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _lock = new(1, 1);
        private static readonly JsonSerializerOptions _opts = new() { WriteIndented = true };

        public JsonRepository(IHostEnvironment env)
        {
            var dataDir = Path.Combine(env.ContentRootPath, "Data");
            Directory.CreateDirectory(dataDir);
            var fileName = typeof(T).Name.ToLower() switch
            {
                "paciente" => "pacientes.json",
                "medico" => "medicos.json",
                "cita" => "citas.json",
                "historial" => "historial.json",
                _ => $"{typeof(T).Name.ToLower()}s.json"
            };
            _filePath = Path.Combine(dataDir, fileName);
            if (!File.Exists(_filePath)) File.WriteAllText(_filePath, "[]");
        }

        public async Task<List<T>> GetAllAsync()
        {
            await _lock.WaitAsync();
            try
            {
                var json = await File.ReadAllTextAsync(_filePath);
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            finally { _lock.Release(); }
        }

        public async Task<T?> GetByIdAsync(Guid id) => (await GetAllAsync()).FirstOrDefault(x => x.Id == id);

        public async Task AddAsync(T entity)
        {
            await _lock.WaitAsync();
            try
            {
                var list = await GetAllAsync();
                if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
                list.Add(entity);
                await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(list, _opts));
            }
            finally { _lock.Release(); }
        }

        public async Task UpdateAsync(T entity)
        {
            await _lock.WaitAsync();
            try
            {
                var list = await GetAllAsync();
                var idx = list.FindIndex(x => x.Id == entity.Id);
                if (idx < 0) throw new KeyNotFoundException("Entity not found");
                list[idx] = entity;
                await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(list, _opts));
            }
            finally { _lock.Release(); }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _lock.WaitAsync();
            try
            {
                var list = (await GetAllAsync()).Where(x => x.Id != id).ToList();
                await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(list, _opts));
            }
            finally { _lock.Release(); }
        }
    }
}

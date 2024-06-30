using StackExchange.Redis;
using System.Linq.Expressions;
using System.Text.Json;

namespace Infrastructure.Utility.Caching
{
    public class CacheDistribution : ICacheDistribution
    {
        private IDatabase _redisDb { get; set; }

        public CacheDistribution(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }

        public IQueryable<T?>? GetDataByKey<T>(string key, Expression<Func<T, bool>>? expression = null)
        {
            var completeSet = _redisDb.HashGetAll(key);

            if (completeSet != null && completeSet.Length > 0)
            {
                var data = Array.ConvertAll(completeSet, val => JsonSerializer.Deserialize<T>(val.Value)).AsQueryable();
                if (expression != null)
                {
                    data = data.Where(expression);
                }
                return data;
            }
            return default;
        }

        public T? GetDataById<T>(string id, string key)
        {
            var data = _redisDb.HashGet(key, id);

            if (!string.IsNullOrEmpty(data))
            {
                return JsonSerializer.Deserialize<T>(data);
            }
            return default;
        }

        public void CreateData<T>(string key, string id, T data)
        {
            var serialData = JsonSerializer.Serialize(data);

            _redisDb.HashSet(key, new HashEntry[]
                {new HashEntry(id, serialData)});
        }

        public void UpdateData<T>(string key, string id, T data)
        {
            var getData = _redisDb.HashGet(key, id);
            if (!getData.IsNullOrEmpty)
            {
                _redisDb.HashDelete(key, id);
            }

            var serialData = JsonSerializer.Serialize(data);

            _redisDb.HashSet(key, new HashEntry[]
                {new HashEntry(id, serialData)});
        }

        public void DeleteData(string key, string id)
        {
            var getData = _redisDb.HashGet(key, id);
            if (!getData.IsNullOrEmpty)
            {
                _redisDb.HashDelete(key, id);
            }
        }
    }
}

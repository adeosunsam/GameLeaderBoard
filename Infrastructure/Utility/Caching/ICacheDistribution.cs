using System.Linq.Expressions;

namespace Infrastructure.Utility.Caching
{
    public interface ICacheDistribution
    {
        IQueryable<T?>? GetDataByKey<T>(string key, Expression<Func<T, bool>>? expression = null);
        T? GetDataById<T>(string key, string id);
        void CreateData<T>(string key, string id, T data);
        void UpdateData<T>(string key, string id, T data);
        void DeleteData(string key, string? id = null);
    }
}

using System;
using System.Threading.Tasks;

namespace Insurance.Proxy.Contracts
{
    public interface IHttpProxy<TEntity> where TEntity : class
    {
        Task<TEntity> GetEntityCollection(string endPoint);
    }
}

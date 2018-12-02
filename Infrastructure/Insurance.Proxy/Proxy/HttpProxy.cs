using System;
using System.Threading.Tasks;
using Insurance.Proxy.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Net.Http;

namespace Insurance.Proxy.Proxy
{
    public class HttpProxy<TEntity> : IHttpProxy<TEntity> where TEntity : class
    {
        private readonly IMemoryCache memoryCache;
        private readonly IHttpClientFactory httpClientFactory;

        public HttpProxy(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            this.httpClientFactory = httpClientFactory;
            this.memoryCache = memoryCache;
        }

        public async Task<TEntity> GetEntityCollection(string endPoint)
        {
            bool existEntityCollectionInCache = memoryCache.TryGetValue(typeof(TEntity), out TEntity entityListFromCache);

            if (existEntityCollectionInCache)
            {
                return entityListFromCache;
            }

            HttpClient client = this.httpClientFactory.CreateClient();
            string result = await client.GetStringAsync(endPoint);
            TEntity DtoResult = JsonConvert.DeserializeObject<TEntity>(result);

            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
            memoryCache.Set(typeof(TEntity), DtoResult, cacheEntryOptions);

            return DtoResult;
        }
    }
}

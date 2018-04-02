using System.Configuration;

namespace Logistics.Common.CacheManager
{
    public class ManagerCache
    {
        private string CacheType = ConfigurationManager.AppSettings["CacheType"];

        public ManagerCache(bool MemoryCache = false)
        {
            if (MemoryCache)
            {
                CacheType = "MemoryCache";
            }
        }
        public ICacheManager CacheService
        {
            get
            {
                if (string.IsNullOrEmpty(CacheType))
                {
                    CacheType = "MemoryCache";
                }
                switch (CacheType)
                {
                    case "MemoryCache":
                        return new CacheHelper();
                    default:
                        return new CacheHelper();//默认
                }
            }
        }
    }
}

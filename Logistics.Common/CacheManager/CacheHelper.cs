using Logistics.Common.CacheManager;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
namespace Logistics.Common
{
    public class CacheHelper:ICacheManager
    {
        private static object obj = new object();

        /// <summary>
        /// 缓存对象
        /// </summary>
        protected ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        /// <summary>
        /// 根据关键字获取或设置关联的值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">要获取值的关键字</param>
        /// <returns>关键字关联的值</returns>
        public virtual T Get<T>(string key)
        {
            return (T)Cache.Get(key);
        }
        /// <summary>
        /// 获取缓存，不存在则更新
        /// </summary>
        /// <typeparam name="T">获取对象类型</typeparam>
        /// <param name="Key">缓存Key</param>
        /// <param name="ExpirationType">缓存过期方式，滑动过期为1，绝对过期为2</param>
        /// <param name="fun">更新缓存方法</param>
        /// <param name="ExpiresSecond">缓存时间（秒）默认60秒</param>
        public T GetOrUpdate<T>(string Key, int ExpirationType, Func<T> fun = null, int ExpiresSecond = 60)
        {
            lock (obj)
            {
                T Value = Get<T>(Key);
                if (Value == null)
                {
                    Value = fun();
                    Set(Key, Value, ExpiresSecond, ExpirationType);
                }
                return Value;
            }
        }
        /// <summary>
        /// 获取缓存，不存在则更新，设置了绝对过期时间
        /// </summary>
        /// <typeparam name="T">获取对象类型</typeparam>
        /// <param name="Key">缓存Key</param>
        /// <param name="fun">更新缓存方法</param>
        /// <param name="ExpiresTime">到期时间</param>
        public T GetOrUpdateAsDate<T>(string Key, Func<T> fun, DateTime ExpiresTime)
        {
            lock (obj)
            {
                T Value = Get<T>(Key);
                if (Value == null)
                {
                    Value = fun();
                    Set(Key, Value, ExpiresTime);
                }
                return Value;
            }
        }
        /// <summary>
        /// 添加一个关键字和对象到缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="data">值</param>
        /// <param name="cacheTime">缓存时间（秒）</param>
        /// <param name="ExpirationType">缓存过期方式，滑动过期为1，绝对过期为2</param>
        public virtual void Set(string key, object data, int cacheTime, int ExpirationType)
        {
            if (data == null)
                return;

            var policy = new CacheItemPolicy();
            if (ExpirationType == 1)
            {
                policy.SlidingExpiration = TimeSpan.FromSeconds(cacheTime);
            }
            else
            {
                policy.AbsoluteExpiration = DateTime.Now.AddSeconds(cacheTime);
            }
            Cache.Set(new CacheItem(key, data), policy);
        }


        /// <summary>
        /// 添加一个缓存，设置了绝对过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="ExpiresTime"></param>
        public void Set(string key, object data, DateTime ExpiresTime)
        {
            if (data == null || ExpiresTime < DateTime.Now)
                return;

            var policy = new CacheItemPolicy();
            //设置过期时间
            policy.AbsoluteExpiration = ExpiresTime;
            Cache.Set(new CacheItem(key, data), policy);
        }
        /// <summary>
        /// 检查是否已缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public virtual bool IsSet(string key)
        {
            return (Cache.Contains(key));
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">key</param>
        public virtual void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// 根据模式移除缓存
        /// </summary>
        /// <param name="pattern">pattern</param>
        public virtual void RemoveByPattern(string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (KeyValuePair<string, object> item in Cache)
                if (regex.IsMatch(item.Key))
                    Remove(item.Key);
        }
        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public virtual void Clear()
        {
            foreach (var item in Cache)
                Remove(item.Key);
        }
    }
}

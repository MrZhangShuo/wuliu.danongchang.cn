using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logistics.Common.CacheManager
{
    public interface ICacheManager
    {
        /// <summary>
        /// 根据关键字获取缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 获取缓存，不存在则更新
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="Key">关键字</param>
        /// <param name="ExpirationType">缓存过期方式，滑动过期为1，绝对过期为2</param>
        /// <param name="fun">更新方法</param>
        /// <param name="ExpiresSecond">缓存时间（秒）默认60秒</param>
        /// <returns></returns>
        T GetOrUpdate<T>(string Key, int ExpirationType, Func<T> fun = null, int ExpiresSecond = 60);

        /// <summary>
        /// 获取缓存，不存在则更新
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="Key">关键字</param>
        /// <param name="fun">更新方法</param>
        /// <param name="ExpiresTime">绝对到期时间</param>
        /// <returns></returns>
        T GetOrUpdateAsDate<T>(string Key, Func<T> fun, DateTime ExpiresTime);


        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="data">值</param>
        /// <param name="cacheTime">缓存时间（秒）</param>
        /// <param name="ExpirationType">缓存过期方式，滑动过期为1，绝对过期为2</param>
        void Set(string key, object data, int cacheTime, int ExpirationType);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="data">值</param>
        /// <param name="ExpiresTime">过期时间</param>
        void Set(string key, object data, DateTime ExpiresTime);

        /// <summary>
        /// 缓存键是否已存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsSet(string key);
        /// <summary>
        /// 根据关键字移除缓存
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
        /// <summary>
        /// 根据匹配模式移除缓存
        /// </summary>
        /// <param name="pattern"></param>
        void RemoveByPattern(string pattern);
        /// <summary>
        /// 清空所有缓存
        /// </summary>
        void Clear();
    }
}

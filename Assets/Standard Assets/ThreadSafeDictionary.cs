/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;

namespace Helper
{
    public class ThreadSafeDictionary<TKey, TValue>
    {
        protected readonly Dictionary<TKey, TValue> _impl = new Dictionary<TKey, TValue>();
        public TValue this[TKey key]
        {
            get
            {
                lock (_impl)
                {
                    return _impl[key];
                }
            }
            set
            {
                lock (_impl)
                {
                    _impl[key] = value;
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            lock (_impl)
            {
                _impl.Add(key, value);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_impl)
            {
                return _impl.TryGetValue(key, out value);
            }
        }

        public bool Remove(TKey key)
        {
            lock (_impl)
            {
                return _impl.Remove(key);
            }
        }

        public void Clear()
        {
            lock (_impl)
            {
                _impl.Clear();
            }
        }
    }
}

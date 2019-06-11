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
    internal class EventPump : UnityEngine.MonoBehaviour
    {
        private static object s_Lock = new object();
        private Queue<Action> m_Queue = new Queue<Action>();

        public static EventPump Instance
        {
            get;
            private set;
        }

        public static void EnsureInitialized()
        {
            try
            {
                if (EventPump.Instance == null)
                {
                    lock (s_Lock)
                    {
                        if (EventPump.Instance == null)
                        {
                            UnityEngine.GameObject parent = new UnityEngine.GameObject("Kinect Desktop Event Pump");
                            EventPump.Instance = parent.AddComponent<Helper.EventPump>();
                            DontDestroyOnLoad(parent);
                        }
                    }
                }
            }
            catch
            {
                UnityEngine.Debug.LogError("Events must be registered on the main thread.");
                return;
            }
        }

        private void Update()
        {
            lock (m_Queue)
            {
                while (m_Queue.Count > 0)
                {
                    var action = m_Queue.Dequeue();
                    try
                    {
                        action.Invoke();
                    }
                    catch { }
                }
            }
        }

        private void OnApplicationQuit()
        {
            var sensor = Windows.Kinect.KinectSensor.GetDefault();
            if (sensor != null && sensor.IsOpen)
            {
                sensor.Close();
            }

            NativeObjectCache.Flush();
        }

        public void Enqueue(Action action)
        {
            lock (m_Queue)
            {
                m_Queue.Enqueue(action);
            }
        }
    }
}

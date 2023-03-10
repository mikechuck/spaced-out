using UnityEngine;
using Unity.Netcode;
using System;

namespace CustomSingletons
{
    public class Singleton<T> : NetworkBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var objs = FindObjectsOfType(typeof(T)) as T[];
                    if (objs.Length > 0) _instance = objs[0];
                    if (objs.Length > 1)
                    {
                        Debug.LogError("There is more than on " + typeof(T).Name + " in the scene.");
                    }
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = string.Format("_{0}", typeof(T).Name);
                        _instance = obj.AddComponent<T>();
                    }
                }
				Debug.Log("PM instance set");
                return _instance;
            }
        }
    }
}
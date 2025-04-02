using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static T m_Instance;
    public static T Instance
    {
        get
        {
            if (m_ShuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                                 "' already destroyed. Returning null.");
                return null;
            }

            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    // Search for existing instance.
                    m_Instance = (T)FindObjectOfType(typeof(T));

                    // Create new instance if one doesn't already exist.
                    if (m_Instance == null)
                    {
                        var singletonObject = new GameObject();
                        // Need to create a new GameObject to attach the singleton to.
                        m_Instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                        // Make instance persistent.
                        DontDestroyOnLoad(singletonObject);
                    }
                }
                else
                {

                }

                return m_Instance;
            }
        }
    }
    
    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }
    
    private void OnDestroy()
    {
        m_ShuttingDown = true;
    }
}

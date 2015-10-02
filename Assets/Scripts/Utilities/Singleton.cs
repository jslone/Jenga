using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    public bool Init(T instance)
    {
        Instance = instance;
        return true;
    }
}

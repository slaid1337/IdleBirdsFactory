using UnityEngine;

public class Singletone<T> : MonoBehaviour where T : MonoBehaviour 
{
    static T m_instance;
    [SerializeField] private bool _dontDestroy;

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType<T>();

                if (m_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(T).Name);
                    m_instance = singleton.AddComponent<T>();
                }
            }
            return m_instance;
        }
    }

    public virtual void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
        if (_dontDestroy)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
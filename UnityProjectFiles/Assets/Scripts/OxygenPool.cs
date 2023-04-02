using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//object pooling for oxygen
public class OxygenPool : MonoBehaviour
{
    public static OxygenPool SharedInstance;
    [SerializeField]
    public List<GameObject> pooledOxygen;
    [SerializeField]
    private GameObject objectToPool;
    private int amounToPool = 10;

    void Awake()
    {
        SharedInstance = this;
    }
    
    void Start()
    {
        pooledOxygen = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amounToPool; i++)
        {
            tmp = Instantiate(objectToPool, transform);
            tmp.SetActive(false);
            pooledOxygen.Add(tmp);
        }
    }

    public GameObject GetPooledOxygen()
    {
        for(int i = 0; i < amounToPool; i++)
        {
            if (!pooledOxygen[i].activeInHierarchy)
            {
                return pooledOxygen[i];
            }
        }
        return null;
    }
}


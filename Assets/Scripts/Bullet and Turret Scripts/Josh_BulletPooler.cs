using UnityEngine;
using System.Collections.Generic;

public class Josh_BulletPooler : MonoBehaviour
{
    public static Josh_BulletPooler Instance;
    public GameObject bulletPrefab;
    public int poolSize = 60;
    private List<GameObject> bulletPool = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject b = Instantiate(bulletPrefab);
            b.SetActive(false);
            bulletPool.Add(b);
        }
    }

    public GameObject GetBullet()
    {
        foreach (GameObject b in bulletPool)
        {
            if (!b.activeInHierarchy)
            {
                b.SetActive(true);
                return b;
            }
        }
        return null;
    }

    public void ReturnBullet(GameObject b)
    {
        b.SetActive(false);
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Spawner : MonoBehaviour {

    public GameObject[] prefabs;
    private float spawnTime = 5F;
    public Transform[] spawnPoints;
    private List<Enemy> enemies = new List<Enemy>();
    

    void Start()
    {

        StartCoroutine(Timer()); //карутина, генерация врага происходит каждые 5 секунд
             
    }

   

    void CreateEnemy()
    {
        Vector3 randomPos = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

        if (enemies.Count < 5)
        {
            GameObject enemy = Instantiate(prefabs[Random.Range(0, prefabs.Length)], randomPos, transform.rotation) as GameObject;

            enemies.Add(enemy.GetComponent<Enemy>());
        }

        else
        {
            Enemy e = GetPooledEnemie();
            if (e != null)
            {
                e.Create(randomPos);
            }

        }                         
    }

    IEnumerator Timer()//создание врагов с заданой частатой
    {
        while (true)
        {
            CreateEnemy();
            yield return new WaitForSeconds(spawnTime);
        }
    }

    Enemy GetPooledEnemie()//пул врагов
    {
        List<Enemy> pooled = enemies.Where(e => !e.gameObject.activeInHierarchy).ToList();
        if (pooled.Count == 0)
        { return null; }
        return pooled[Random.Range(0, pooled.Count)];
    }  
}
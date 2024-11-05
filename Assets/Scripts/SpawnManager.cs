using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    float enemyTimer;
    float enemySpawnTime;

    public GameObject wave;
    float waveTimer;
    float waveSpawnTime;

    public GameObject chest;
    public Transform[] itemSpawnPoints;
    float itemTimer;
    float itemSpawnTime;

    private void Start()
    {
        enemyTimer = 0;
        enemySpawnTime = Random.Range(0f, 3f);

        waveTimer = 0;
        waveSpawnTime = Random.Range(0f, 5f);

        itemTimer = 0;
        itemSpawnTime = Random.Range(5f, 7f);
    }

    void Update()
    {
        if (GameManager.instance.isDead || GameManager.instance.isCleared)
        {
            return;
        }

        enemyTimer += Time.deltaTime;

        if (enemyTimer >= enemySpawnTime || GameManager.instance.poolManager.activeCount <= 5)
        {
            if (GameManager.instance.poolManager.activeCount >= 20)
            {
                return;
            }
            SpawnEnemy();
            enemyTimer = 0;
        }

        waveTimer += Time.deltaTime;

        if (waveTimer >= waveSpawnTime)
        {
            if (wave.activeSelf == false)
            {
                SpawnWave();
            }
            waveTimer = 0;
        }

        itemTimer += Time.deltaTime;

        if (itemTimer >= itemSpawnTime)
        {
            SpawnItem();
            itemTimer = 0;
        }
    }

    void SpawnEnemy()
    {
        GameManager.instance.poolManager.activeCount++;
        int currentLevel = GameManager.instance.level;
        int p = Random.Range(0, 10);
        int getLevel = 0;
        if (p < 5)
        {
            getLevel = Random.Range(Mathf.Max(0, currentLevel - 1), currentLevel + 1);
        }
        else
        {
            getLevel = Random.Range(currentLevel, Mathf.Min(GameManager.instance.maxLevel, currentLevel + 1) + 1);
        }
        GameObject enemy = GameManager.instance.poolManager.Get(getLevel);

        if (enemy.GetComponent<EnemyController>().level == 0)
        {
            float randX = Random.Range(-8, 9);
            enemy.transform.position = new Vector2(randX, 7.5f);
        }
        else
        {
            float randX = Random.Range(0, 2) == 0 ? -12 : 12;
            float randY = Random.Range(-3f, 5f);

            enemy.transform.position = new Vector2(randX, randY);
        }
        enemySpawnTime = Random.Range(0f, 3f);
    }

    void SpawnItem()
    {
        List<int> result = new List<int>();

        for (int i = 0; i < itemSpawnPoints.Length; i++)
        {
            int index = Random.Range(0, i + 1);
            result.Insert(index, i);
        }

        for (int i = 0; i < result.Count; i++)
        {
            int index = result[i];
            SpriteRenderer renderer = itemSpawnPoints[index].gameObject.GetComponentInChildren<SpriteRenderer>();

            if (renderer == null)
            {
                GameObject item = Instantiate(chest, itemSpawnPoints[index]);
                break;
            }
            else if (renderer.gameObject.activeSelf == false)
            {
                renderer.gameObject.SetActive(true);
                break;
            }
        }
    }

    void SpawnWave()
    {
        wave.transform.position = new Vector2(Random.Range(-8, 9), wave.transform.position.y);
        wave.SetActive(true);
        waveSpawnTime = Random.Range(0f, 5f);
    }
}

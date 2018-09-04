using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public string poolName;
    public GameObject prefab;
    public int quantity;

    GameObject[] pool;
    int index;
    Vector2 originalPosition = Vector2.up * 100; 

    void Start()
    {
        pool = new GameObject[quantity];
        for (int i = 0; i < quantity; i++)
        {
            pool[i] = Instantiate(prefab, originalPosition, Quaternion.identity, transform);
            pool[i].SetActive(false);

            Enemy enemy = pool[i].GetComponent<Enemy>();
            if(enemy) enemy.pool = this;
        }
    }

    public GameObject GetEnemy(Vector2 spawnPosition, Size spawnSize)
    {
        GameObject _object = pool[index];
        _object.transform.position = spawnPosition;
        _object.SetActive(true);

        Enemy enemy = _object.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.SetSize(spawnSize);
            enemy.Launch();
        }

        index = (index + 1) % quantity;
        return _object;
    }

    public void Return(GameObject _object)
    {
        _object.transform.position = originalPosition;
        _object.SetActive(false);
    }
}

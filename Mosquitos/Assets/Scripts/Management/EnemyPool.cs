using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public GameObject prefab;
    public int quantity;

    Spawner cicleManager;
    GameObject[] pool;
    int index;
    Vector2 originalPosition = Vector2.up * 100; 

    void Start()
    {
        cicleManager = Spawner.instance;

        pool = new GameObject[quantity];
        for (int i = 0; i < quantity; i++)
        {
            pool[i] = Instantiate(prefab, originalPosition, Quaternion.identity, transform);
            pool[i].SetActive(false);

            Enemy enemy = pool[i].GetComponent<Enemy>();
            if(enemy) enemy.Init(this);
        }
    }

    public GameObject GetEnemy(Vector2 position, float angle)
    {
        GameObject _object = pool[index];
        _object.transform.position = position;
        _object.transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        _object.SetActive(true);

        Enemy enemy = _object.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.Launch();
        }

        index = (index + 1) % quantity;
        cicleManager.ChangeQuantity(1);
        return _object;
    }

    public void Return(GameObject _object)
    {
        _object.transform.position = originalPosition;
        _object.SetActive(false);
        cicleManager.ChangeQuantity(-1);
    }
}

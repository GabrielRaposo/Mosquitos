using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Size {
    P,
    M,
    G
}

public class Enemy : MonoBehaviour {

    public float speed;
    public Size size;
    [HideInInspector] public EnemyPool pool;

    virtual public void Launch()
    {
        Debug.Log("Launch() sem override.");
    }

    public void SetSize(Size size)
    {
        this.size = size;
        switch (size)
        {
            case Size.P: transform.localScale = Vector3.one * 1f;   break;
            case Size.M: transform.localScale = Vector3.one * 2f;   break;
            case Size.G: transform.localScale = Vector3.one * 3f;   break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            //take player health
            if (pool) { pool.Return(gameObject); }
        }
        //reagir com repelente
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Arena"))
        {
            if (pool) { pool.Return(gameObject); }
        }
    }
}

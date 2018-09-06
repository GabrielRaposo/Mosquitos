using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour {

    public float speed;
    public Size size;
    EnemyPool pool;

    public void Init(EnemyPool pool)
    {
        this.pool = pool;
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

    virtual public void Launch() { }

    virtual public void Trigger() { }

    virtual public void Disable()
    {
        if (pool) { pool.Return(gameObject); }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            //take player health
            Disable();
        } else
        if (collision.CompareTag("EventTrigger")) {
            Trigger();
        }
        //interagir com repelente
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Arena")) {
            Disable();
        } 
    }


}

public enum Size
{
    P,
    M,
    G
}

public enum EnemyType
{
    Random, 
    Aim,
    Chaser
}
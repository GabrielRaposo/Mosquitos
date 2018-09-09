using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour {

    public float speed;
    EnemyPool pool;
    protected bool fleeing;

    public void Init(EnemyPool pool)
    {
        this.pool = pool;
    }

    virtual public void Launch()
    {
        fleeing = false;
    }

    virtual public void Trigger() { }

    virtual public void Disable()
    {
        if (pool) { pool.Return(gameObject); }
    }

    void Flee()
    {
        if (fleeing) return;

        fleeing = true;
        StopAllCoroutines();
        //animation - startle

        //Mira contra o centro
        Vector3 fleeFrom = Vector3.zero;
        if (Player.instance) fleeFrom = Player.instance.transform.position;
        transform.rotation = RaposUtil.LookAtPosition(transform.position, fleeFrom);
        transform.Rotate(Vector3.forward * 180);

        //Move na direção gerada
        Vector3 movementIntensity = Vector3.up * 7;
        GetComponent<Rigidbody2D>().velocity = RaposUtil.RotateVector(movementIntensity, transform.rotation.eulerAngles.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            Player player = collision.GetComponent<Player>();
            if (player) player.TakeDamage();
            Flee();
        } else
        if (collision.CompareTag("EventTrigger")) {
            Trigger();
        } else 
        if (collision.CompareTag("Repeller")) {
            Flee();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Arena")) {
            Disable();
        } 
    }

}
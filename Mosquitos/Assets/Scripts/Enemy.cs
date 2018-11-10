using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour {

    public float speed;
    public ParticleSystem scareEffect;
    public ParticleSystem launchEffect;

    private EnemyPool pool;
    protected bool fleeing;

    public void Init(EnemyPool pool)
    {
        this.pool = pool;
        speed *= PlayerAgeData.difficultyScaler;
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

    virtual protected IEnumerator Flee()
    {
        //Trava no lugar por alguns segundos
        fleeing = true;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Vector3 fleeFrom = Vector3.zero;
        if (Rescuee.instance) fleeFrom = Rescuee.instance.transform.position;
        Animator animator = GetComponent<Animator>();

        animator.SetTrigger("Scare");
        if (scareEffect) scareEffect.Play();
        //play effect
        yield return new WaitForSeconds(.5f);

        //Mira contra o centro
        transform.rotation = RaposUtil.LookAtPosition(transform.position, fleeFrom);
        transform.Rotate(Vector3.forward * 180);

        //Move na direção gerada
        Vector3 movementIntensity = Vector3.up * 7;
        GetComponent<Rigidbody2D>().velocity = RaposUtil.RotateVector(movementIntensity, transform.rotation.eulerAngles.z);
        animator.SetTrigger("Flee");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EventTrigger")) {
            Trigger();
        } else 
        if (collision.CompareTag("Repellent")) {
            StopAllCoroutines();
            StartCoroutine(Flee());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Arena")) {
            Disable();
        } 
    }

}
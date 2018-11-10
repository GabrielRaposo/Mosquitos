using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChaserEnemy : Enemy {

    Rescuee target;
    Rigidbody2D rb;
    bool follow;

    public override void Launch()
    {
        base.Launch();

        //começa olhando pro player
        rb = GetComponent<Rigidbody2D>();
        target = Rescuee.instance;
        transform.rotation = RaposUtil.LookAtPosition(transform.position, target.transform.position);
        follow = true;
        GetComponent<Animator>().SetTrigger("Shoot");

        StartCoroutine(UnfollowAfterTime(7));
    }
	
	void Update ()
    {
        if (fleeing) return;
        if (follow)
        {
            Vector3 posDiff = target.transform.position - transform.position;
            transform.DORotate(new Vector3(0, 0, (Mathf.Atan2(posDiff.y, posDiff.x) * Mathf.Rad2Deg) + 270), 5);
        }
        Vector3 movementIntensity = Vector3.up * speed;
        rb.velocity = RaposUtil.RotateVector(movementIntensity, transform.rotation.eulerAngles.z);
    }

    override protected IEnumerator Flee()
    {
        follow = false;
        transform.DOKill();
        yield return base.Flee();
    }

    public override void Disable()
    {
        StopAllCoroutines();
        follow = false;
        base.Disable();
    }

    IEnumerator UnfollowAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        follow = false;
    }
}

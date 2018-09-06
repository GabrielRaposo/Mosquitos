using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChaserEnemy : Enemy {

    Player player;
    Rigidbody2D rb;
    bool follow;

    public override void Launch()
    {
        rb = GetComponent<Rigidbody2D>();

        //começa olhando pro player
        player = Player.instance;
        transform.rotation = RaposUtil.LookAtPosition(transform.position, player.transform.position);
        follow = true;

        StartCoroutine(DisableTimer(7));
    }
	
	void Update ()
    {
        if (follow)
        {
            Vector3 posDiff = player.transform.position - transform.position;
            transform.DORotate(new Vector3(0, 0, (Mathf.Atan2(posDiff.y, posDiff.x) * Mathf.Rad2Deg) + 270), 5);
        }
        Vector3 movementIntensity = Vector3.up * speed;
        rb.velocity = RaposUtil.RotateVector(movementIntensity, transform.rotation.eulerAngles.z);
    }

    public override void Disable()
    {
        StopAllCoroutines();
        follow = false;
        base.Disable();
    }

    IEnumerator DisableTimer(float time)
    {
        yield return new WaitForSeconds(time);
        follow = false;
    }
}

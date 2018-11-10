using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AimEnemy : Enemy {

    Rescuee target;
    bool aim;
    Rigidbody2D _rigidbody;
    Coroutine windUpcoroutine;

    public override void Launch()
    {
        base.Launch();

        target = Rescuee.instance;
        _rigidbody = GetComponent<Rigidbody2D>();

        //Lança para a direção que está olhando
        Vector3 movementIntensity = Vector3.up * speed / 2;
        if(_rigidbody) _rigidbody.velocity = RaposUtil.RotateVector(movementIntensity, transform.rotation.eulerAngles.z);
    }

    private void Update()
    {
        if (aim)
        {
            Vector3 posDiff = target.transform.position - transform.position;
            transform.DORotate(new Vector3(0, 0, (Mathf.Atan2(posDiff.y, posDiff.x) * Mathf.Rad2Deg) + 270), 2);
        }
    }

    override protected IEnumerator Flee()
    {
        aim = false;
        transform.DOKill();
        yield return base.Flee();
    }

    public override void Trigger()
    {
        if(windUpcoroutine == null)
        {
            GetComponent<Animator>().SetTrigger("WindUp");
            windUpcoroutine = StartCoroutine(WindUpAndRelease());
        }
    }

    IEnumerator WindUpAndRelease()
    {
        yield return new WaitForSeconds(.3f);
        _rigidbody.velocity = Vector3.zero;

        aim = true;
        yield return new WaitForSeconds(1);
        aim = false;

        Vector3 movementIntensity = Vector3.up * speed;
        _rigidbody.velocity = RaposUtil.RotateVector(movementIntensity, transform.rotation.eulerAngles.z);
        GetComponent<Animator>().SetTrigger("Shoot");
        if (launchEffect != null)
        {
            launchEffect.Play();
        }
    }

    public override void Disable()
    {
        StopAllCoroutines();
        aim = false;
        base.Disable();
    }
}

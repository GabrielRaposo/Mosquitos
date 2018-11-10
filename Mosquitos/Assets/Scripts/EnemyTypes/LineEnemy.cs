using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LineEnemy : Enemy
{
    Rigidbody2D _rigidbody;
    Coroutine windUpcoroutine;

    public override void Launch()
    {
        base.Launch();

        _rigidbody = GetComponent<Rigidbody2D>();

        //Lança para a direção que está olhando
        Vector3 movementIntensity = Vector3.up * speed / 2;
        if (_rigidbody) _rigidbody.velocity = RaposUtil.RotateVector(movementIntensity, transform.rotation.eulerAngles.z);
    }

    public override void Trigger()
    {
        if (windUpcoroutine == null)
        {
            GetComponent<Animator>().SetTrigger("WindUp");
            windUpcoroutine = StartCoroutine(WindUpAndRelease());
        }
    }

    IEnumerator WindUpAndRelease()
    {
        _rigidbody.velocity = Vector3.zero;

        yield return new WaitForSeconds(1);

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
        base.Disable();
    }
}

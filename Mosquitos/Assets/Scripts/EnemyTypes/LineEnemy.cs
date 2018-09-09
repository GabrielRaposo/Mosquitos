using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LineEnemy : Enemy
{
    Player player;
    Rigidbody2D _rigidbody;

    public override void Launch()
    {
        base.Launch();

        player = Player.instance;
        _rigidbody = GetComponent<Rigidbody2D>();

        //Lança para a direção que está olhando
        Vector3 movementIntensity = Vector3.up * speed / 2;
        if (_rigidbody) _rigidbody.velocity = RaposUtil.RotateVector(movementIntensity, transform.rotation.eulerAngles.z);
    }

    public override void Trigger()
    {
        StartCoroutine(WindUpAndRelease());
    }

    IEnumerator WindUpAndRelease()
    {
        _rigidbody.velocity = Vector3.zero;

        yield return new WaitForSeconds(1);

        Vector3 movementIntensity = Vector3.up * speed;
        _rigidbody.velocity = RaposUtil.RotateVector(movementIntensity, transform.rotation.eulerAngles.z);
    }

    public override void Disable()
    {
        StopAllCoroutines();
        base.Disable();
    }
}

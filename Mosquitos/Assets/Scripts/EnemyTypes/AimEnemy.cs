using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AimEnemy : Enemy {

    Player player;
    bool aim;
    Rigidbody2D _rigidbody;

    //-----temp stuff-----
    SpriteRenderer _renderer;
    Color savedColor;
    //--------------------

    public override void Launch()
    {
        base.Launch();

        player = Player.instance;
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        savedColor = _renderer.color;

        //Lança para a direção que está olhando
        Vector3 movementIntensity = Vector3.up * speed / 2;
        if(_rigidbody) _rigidbody.velocity = RaposUtil.RotateVector(movementIntensity, transform.rotation.eulerAngles.z);
    }

    private void Update()
    {
        if (fleeing) return;
        if (aim)
        {
            Vector3 posDiff = player.transform.position - transform.position;
            transform.DORotate(new Vector3(0, 0, (Mathf.Atan2(posDiff.y, posDiff.x) * Mathf.Rad2Deg) + 270), 2);
        }
    }

    public override void Trigger()
    {
        StartCoroutine(WindUpAndRelease());
    }

    IEnumerator WindUpAndRelease()
    {
        _renderer.color = Color.black;
        _rigidbody.velocity = Vector3.zero;

        aim = true;
        yield return new WaitForSeconds(1);
        aim = false;

        _renderer.color = savedColor;
        Vector3 movementIntensity = Vector3.up * speed;
        _rigidbody.velocity = RaposUtil.RotateVector(movementIntensity, transform.rotation.eulerAngles.z);
    }

    public override void Disable()
    {
        StopAllCoroutines();
        aim = false;
        _renderer.color = savedColor;
        base.Disable();
    }
}

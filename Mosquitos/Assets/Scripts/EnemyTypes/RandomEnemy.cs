using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemy : Enemy {

    public override void Launch()
    {
        base.Launch();

        //Mira para o centro, errando um pouco às vezes
        transform.rotation = RaposUtil.LookAtPosition(transform.position, Vector3.zero);
        float angleOffset = Random.Range(-25f, 25f);
        transform.Rotate(Vector3.forward * angleOffset);

        //Gera velocidade na direção definida
        Vector3 movementIntensity = Vector3.up * speed;
        GetComponent<Rigidbody2D>().velocity = RaposUtil.RotateVector(movementIntensity, transform.rotation.eulerAngles.z);
    }

}

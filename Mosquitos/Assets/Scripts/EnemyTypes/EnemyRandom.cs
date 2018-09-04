using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandom : Enemy {

    public override void Launch()
    {
        //Mira para o centro, errando um pouco às vezes
        transform.rotation = RaposUtil.LookAtPosition(transform.position, Vector3.zero);
        float angleOffset = Random.Range(-25f, 25f);
        transform.Rotate(Vector3.forward * angleOffset);

        //Gera velocidade na direção definida
        float localSpeed = speed; // / ((int)size + 1);
        Vector3 movementIntensity = Vector3.up * localSpeed;
        float angle = transform.rotation.eulerAngles.z;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb) {
            rb.velocity = RaposUtil.RotateVector(movementIntensity, angle);
        } 
    }

}

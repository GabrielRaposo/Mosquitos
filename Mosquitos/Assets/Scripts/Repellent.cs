using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repellent : MonoBehaviour {

    public SpriteRenderer mainSprite;
    public SpriteRenderer whiteOverlay;
    public SpriteRenderer shadowSprite;
    [Space(20)]
    public ParticleSystem backEffect;
    public ParticleSystem collectEffect;
    public ParticleSystem sprayEffect;

    private Collider2D _collider;
    private Animator _animator;

    public void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
    }

    public IEnumerator SpawnAnimation()
    {
        _collider.enabled = false;
        backEffect.Play();
        yield return new WaitForSeconds(1);
        _collider.enabled = true;
    }

    public IEnumerator Collect(Rescuee collector, GameManager gameManager)
    {
        _collider.enabled = false;

        backEffect.Stop();
        whiteOverlay.enabled = true;
        shadowSprite.enabled = false;

        collectEffect.Play();
        _animator.SetTrigger("Vanish");

        yield return new WaitForSeconds(2);

        //collector.enabled = false;
        sprayEffect.transform.position = collector.transform.position;
        sprayEffect.Play();
        yield return new WaitForSeconds(.6f);
        sprayEffect.transform.position = collector.transform.position;
        sprayEffect.Play();

        yield return new WaitForSeconds(1);
        //collector.enabled = true;

        collector.SetProtection();
        gameManager.GotRepellent();
    }
}

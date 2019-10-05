using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Rescuee : MonoBehaviour
{
    public GameManager gameManager;
    [Space(10)]
    [Header("Gameplay")]
    public float baseSpeed = .5f;
    public Collider2D protectionHitbox;

    [Header("Visual References")]
    public SpriteRenderer visualComponent;
    public SpriteRenderer fadeFeverBorder;
    public SymptomState symptomState;

    [Header("Effects")]
    public ParticleSystem hitEffect;
    public ParticleSystem protectionEffect;

    public bool isFollowing { get; private set; }
    private Symptom symptom;
    private Transform target;
    private EventSystem eventSystem;
    private float speed;

    private Coroutine headacheCoroutine;

    public bool invincible { get; private set; }
    public static Rescuee instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        eventSystem = EventSystem.current;
        speed = baseSpeed;
    }

    public void Follow(Transform target)
    {
        this.target = target;
        isFollowing = true;
        visualComponent.transform.localScale = Vector3.one * 1.5f;
        visualComponent.transform.DOScale(1.3f, .3f);
    }

    public void Unfollow()
    {
        isFollowing = false;
    }

    private void Update()
    {
        if (isFollowing)
        {
            transform.position += Vector3.Normalize(target.position - transform.position) * speed;
            if (Vector3.Distance(target.position, transform.position) < speed) transform.position = target.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) {
            if (invincible) return;
            TakeDamage();
        } else
        if (collision.CompareTag("Repellent")) {
            Repellent repellent = collision.GetComponent<Repellent>();
            if (repellent)
            {
                repellent.StartCoroutine(repellent.Collect(this, gameManager));
            }
        }
    }

    public void TakeDamage()
    {
        hitEffect.Play();
        Handheld.Vibrate();
        if (gameManager) gameManager.ReactToDamage();
        isFollowing = false;
    }

    public void SetProtection()
    {
        ClearSymptoms();
        invincible = true;
        protectionHitbox.enabled = true;
        protectionEffect.Play();
    }

    public void CallInvincibilityState()
    {
        StartCoroutine(InvincibilityState());
    }

    public IEnumerator InvincibilityState()
    {
        invincible = true;
        eventSystem = EventSystem.current; 
        eventSystem.enabled = false;
        yield return new WaitForSecondsRealtime(.1f);
        Time.timeScale = 1; 
        eventSystem.enabled = true;
        
        for(int i = 0; i < 60; i++)
        {
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            visualComponent.enabled = (i % 2 == 0) ? false : true; 
        }
        invincible = false;
    }

    public void SetSymptom(Symptom symptom)
    {
        switch (symptom)
        {
            case Symptom.Fever:
                speed = baseSpeed / 5;
                fadeFeverBorder.enabled = true;
                //if already in
                break;

            case Symptom.Headache:
                float delay = (this.symptom == Symptom.Headache) ? 3 : 6;
                if (headacheCoroutine != null) StopCoroutine(headacheCoroutine);
                headacheCoroutine = StartCoroutine(Headaches(delay));
                break;

            case Symptom.Nauseated:
                CustomImageEffect dizzyEffect = Camera.main.GetComponent<CustomImageEffect>();
                if (dizzyEffect) dizzyEffect.enabled = true;
                break;
        }
        this.symptom = symptom;

        symptomState.State = symptom;
    }

    private IEnumerator Headaches(float baseDelay)
    {
        while (true)
        {
            float time = Random.Range(baseDelay - 1, baseDelay + 1);
            yield return new WaitForSeconds(time);

            isFollowing = false;
            symptomState.IntensifyState();
            yield return Tremble();
            symptomState.ToneDownState();
        }
    }

    private void ClearSymptoms()
    {
        speed = baseSpeed;
        fadeFeverBorder.enabled = false;
        if (headacheCoroutine != null) StopCoroutine(headacheCoroutine);
        CustomImageEffect dizzyEffect = Camera.main.GetComponent<CustomImageEffect>();
        if (dizzyEffect) dizzyEffect.enabled = false;

        symptom = Symptom.None;
        symptomState.State = symptom;
    }

    private IEnumerator Tremble()
    {
        float trembleForce = .2f;
        float decreaseFactor = .0025f;

        Transform camera = Camera.main.transform;
        Vector3 cameraOriginalPosition = camera.position;
        camera.position += Vector3.left * (trembleForce / 2);

        for (int i = 0; i < trembleForce / decreaseFactor; i++)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            camera.position += (i%2 == 0 ? Vector3.right : Vector3.left) * trembleForce;
            trembleForce -= decreaseFactor;
        }
    }
}

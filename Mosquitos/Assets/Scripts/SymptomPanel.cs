using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymptomPanel : MonoBehaviour
{
    const float HIDDEN_Y_POSITION = 550;

    public RawImage backgroundFade;
    public RectTransform panel;
    public WheelOfInfortune wheel;
    public Button proceedButton;

    [Header("Information panels")]
    public DialogBox textBox;
    public GameObject symptomsIndexes;
    public SymptomDisplayBox symptomDisplayBox;

    private GameManager caller;
    private int callCount;
    private int symptomQuantity;
    private bool proceedTrigger;

    void Start ()
    {
        HideComponents();
	}

    private void HideComponents()
    {
        panel.localPosition = Vector3.up * HIDDEN_Y_POSITION;

        Color color = backgroundFade.color;
        color.a = 0;
        backgroundFade.color = color;

        textBox.gameObject.SetActive(false);
        symptomsIndexes.SetActive(false);
        symptomDisplayBox.gameObject.SetActive(false);

        proceedButton.gameObject.SetActive(false);
    }

    public void Call(GameManager caller)
    {
        this.caller = caller;

        HideComponents();

        backgroundFade.gameObject.SetActive(true);
        panel.gameObject.SetActive(true);

        StartCoroutine(GenerateSintomSequence());
    }

    private IEnumerator GenerateSintomSequence()
    {
        int targetY = 0;
        float speed = 20;
        float ratio = speed / (HIDDEN_Y_POSITION - targetY);
        float colorIncrease = ratio * .5f; 
        Color color = backgroundFade.color;

        RectTransform wheelTransform = wheel.GetComponent<RectTransform>();
        wheelTransform.localEulerAngles = Vector3.back * 20;
        while (panel.localPosition.y > targetY)
        {
            yield return new WaitForEndOfFrame();
            panel.localPosition += Vector3.down * speed;
            color.a += colorIncrease;
            backgroundFade.color = color;
        }
        panel.localPosition = Vector3.up * targetY;

        if(callCount < 1)
        {
            textBox.SetDialog("Essa não! Um mosquito te picou!", 1);
            textBox.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(1);
            proceedButton.gameObject.SetActive(true);
            yield return new WaitUntil(() => proceedTrigger);
            proceedTrigger = false;
            textBox.gameObject.SetActive(false);

            //show correlated sintoms
            textBox.SetDialog("Você sabia que a picada de um mosquito pode te trazer diversos males além da coceira?", 2);
            textBox.gameObject.SetActive(true);
            symptomsIndexes.SetActive(true);
            yield return new WaitForSecondsRealtime(1);
            proceedButton.gameObject.SetActive(true);
            yield return new WaitUntil(() => proceedTrigger);
            proceedTrigger = false;
            textBox.gameObject.SetActive(false);
            symptomsIndexes.SetActive(false);
        } else {
            textBox.SetDialog("Você foi picado de novo...", 1);
            textBox.gameObject.SetActive(true);
            symptomsIndexes.SetActive(true);
            yield return new WaitForSecondsRealtime(1);
            proceedButton.gameObject.SetActive(true);
            yield return new WaitUntil(() => proceedTrigger);
            proceedTrigger = false;
            textBox.gameObject.SetActive(false);
            symptomsIndexes.SetActive(false);
        }
        callCount++;

        yield return SpinTheWheel(wheelTransform);
        Symptom symptom = wheel.GetResult();
        if(symptom != Symptom.None) symptomQuantity++;
        Debug.Log("Symptom: " + symptom);

        Vector3 originalScale = wheelTransform.localScale;
        wheelTransform.localScale += Vector3.one * .1f;
        while(wheelTransform.localScale.x > originalScale.x)
        {
            yield return new WaitForEndOfFrame();
            wheelTransform.localScale -= Vector3.one * .02f;
        }
        yield return new WaitForSecondsRealtime(.3f);
        symptomDisplayBox.gameObject.SetActive(true);
        symptomDisplayBox.SetValue(symptom);

        yield return new WaitForSecondsRealtime(2);
        symptomDisplayBox.gameObject.SetActive(false);

        speed *= 2;
        while (panel.localPosition.y < HIDDEN_Y_POSITION)
        {
            yield return new WaitForEndOfFrame();
            panel.localPosition += Vector3.up * speed;
            color.a -= colorIncrease;
            backgroundFade.color = color;
        }
        HideComponents();

        backgroundFade.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);

        caller.ReturnWithSymptom(symptom, (symptomQuantity > 2 || callCount > 4) ? true : false);
    }

    private IEnumerator SpinTheWheel(RectTransform t)
    {
        float baseRotation = 20f;

        for(int i = 0; i < 20; i ++)
        {
            yield return new WaitForEndOfFrame();
            t.Rotate(Vector3.back * (baseRotation / 50));
        }

        int duration = Random.Range(60, 120); 
        for(int i = 0; i < duration; i++)
        {
            yield return new WaitForEndOfFrame();
            t.Rotate(Vector3.forward * baseRotation);
        }
        while(baseRotation > 0)
        {
            yield return new WaitForEndOfFrame();
            t.Rotate(Vector3.forward * baseRotation);
            baseRotation -= .2f;
        }
    }

    public void Proceed()
    {
        proceedTrigger = true;
        proceedButton.gameObject.SetActive(false);
    }
}

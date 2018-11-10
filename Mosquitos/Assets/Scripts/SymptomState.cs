using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymptomState : MonoBehaviour {
    [Header("References")]
    public SpriteRenderer visualComponent;
    public SpriteRenderer smileComponent;
    [Space(10)]

    [Header("Healthy")]
    public Color healthyColor;
    public Sprite healthySmile;

    [Header("Fever")]
    public Color feverColor;
    public Sprite feverSmile;

    [Header("Headache")]
    public Color headacheColor;
    public Sprite headacheSmile;
    public Sprite painedSmile;

    [Header("Nauseated")]
    public Color nauseatedColor;
    public Sprite nauseatedSmile;

    private Symptom state;

    public Symptom State
    {
        get
        {
            return state;
        }

        set
        {
            state = value;
            switch (state)
            {
                case Symptom.Fever:
                    visualComponent.color = feverColor;
                    smileComponent.sprite = feverSmile;
                    break;

                case Symptom.Headache:
                    visualComponent.color = headacheColor;
                    smileComponent.sprite = headacheSmile;
                    break;

                case Symptom.Nauseated:
                    visualComponent.color = nauseatedColor;
                    smileComponent.sprite = nauseatedSmile;
                    break;
            }
        }
    }

    public void IntensifyState()
    {
        switch (state)
        {
            default:
            case Symptom.Headache:
                smileComponent.sprite = painedSmile;
                break;
        }
    }

    public void ToneDownState()
    {
        switch (state)
        {
            default:
            case Symptom.Headache:
                smileComponent.sprite = headacheSmile;
                break;
        }
    }

}

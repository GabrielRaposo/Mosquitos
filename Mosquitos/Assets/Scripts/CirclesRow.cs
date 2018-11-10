using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CirclesRow : MonoBehaviour {

    public RectTransform[] circles;

    public void ShowRow(float time)
    {
        foreach(RectTransform circle in circles)
        {
            circle.localScale = Vector3.zero;
            circle.DOScale(1, time);
        }
    }

    public void HideRow(float time)
    {
        foreach (RectTransform circle in circles)
        {
            circle.localScale = Vector3.one;
            circle.DOScale(0, time);
        }
    }
}

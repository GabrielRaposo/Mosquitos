using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogBox : MonoBehaviour
{
    public TextMeshProUGUI display;
    private RectTransform panel;

	public void SetDialog(string s, int size)
    {
        panel = GetComponent<RectTransform>();
        if (panel)
        {
            panel.sizeDelta = new Vector2(915, 120 * size);
            panel.anchoredPosition = Vector3.up * (45 - (20 * (size-1)));
        }
        display.text = s;
    }
}

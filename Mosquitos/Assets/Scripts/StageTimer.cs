using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageTimer : MonoBehaviour {

    TextMeshProUGUI display;

    private void Start()
    {
        display = GetComponent<TextMeshProUGUI>();
        display.text = string.Empty;
    }

    public IEnumerator Set(GameManager caller, int time)
    {
        display = GetComponent<TextMeshProUGUI>();

        while (time > 0)
        {
            display.text = time.ToString();
            yield return new WaitForSeconds(1);
            time--;
        }
        display.text = time.ToString();

        caller.EndTimer();
    }
}

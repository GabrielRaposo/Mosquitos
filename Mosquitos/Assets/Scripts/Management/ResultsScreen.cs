using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsScreen : MonoBehaviour {

    public GameObject textDisplay1;
    public GameObject textDisplay2;
    public GameObject infoPanel;

    public IEnumerator Call()
    {
        yield return new WaitForSeconds(1);
        textDisplay1.SetActive(true);
        yield return new WaitForSeconds(2);
        textDisplay2.SetActive(true);
        yield return new WaitForSeconds(3);
        infoPanel.SetActive(true);
        textDisplay1.SetActive(false);
        textDisplay1.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerAgeData : MonoBehaviour {

    public TMP_InputField inputField;
	public static float difficultyScaler = 1;

    private string savedText = string.Empty;

	public void ValidateAge()
    {
        if(inputField.text.Length > 2 || inputField.text.Contains("-"))
        {
            inputField.text = savedText;
        }
        savedText = inputField.text;
    }

    public void ParseAge()
    {
        int age = int.TryParse(inputField.text, out age) ? age : 9;
        if (age < 10 || age > 35)
        {
            difficultyScaler = .7f;
            Debug.Log("Easy Mode");
        }
        else
        {
            difficultyScaler = 1;
            Debug.Log("Hard Mode");
        }
    }
}

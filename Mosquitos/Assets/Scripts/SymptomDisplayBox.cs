using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SymptomDisplayBox : MonoBehaviour {

    public TextMeshProUGUI resultDisplay;
	
	public void SetValue(Symptom symptom)
    {
        switch (symptom)
        {
            case Symptom.Fever:
                resultDisplay.text = "Você está com febre...";
                break;

            case Symptom.Headache:
                resultDisplay.text = "Você está com dor de cabeça...";
                break;

            case Symptom.Nauseated:
                resultDisplay.text = "Você está enjoado...";
                break;

            case Symptom.None:
                resultDisplay.text = "Você escapou desta vez! \nMais cuidado na próxima!";
                break;
        }
    }
}

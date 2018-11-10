using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Symptom
{
    None,
    Fever,
    Headache, 
    Nauseated,
    Death
}
public class WheelOfInfortune : MonoBehaviour {

    [System.Serializable]
    public struct Data
    {
        public Symptom symptom;
        [Range(0, 100)] public int value;
        public Color displayColor;
    }
    public Data[] datas;
    [Space(20)]
    public GameObject slicePrefab;

    private List<Image> slices;

	void Start ()
    {
        //Cria fatias de sintomas
        float angle = 0;
        slices = new List<Image>();
        for (int i = 0; i < datas.Length; i++)
        {
            GameObject s = Instantiate(slicePrefab, transform.position, Quaternion.Euler(Vector3.forward * angle), transform);
            Image slice = s.GetComponent<Image>();
            if (slice)
            {
                float value = (float)datas[i].value / 100;
                slice.fillAmount = value;
                slice.GetComponent<RectTransform>().rotation = Quaternion.Euler(Vector3.back * angle);
                angle += (value * 360);
                slice.color = datas[i].displayColor;
                slices.Add(slice);
            }
        }

        //Cria a parte "saudável"
        if(angle < 360)
        {
            GameObject s = Instantiate(slicePrefab, transform.position, Quaternion.Euler(Vector3.forward * angle), transform);
            Image slice = s.GetComponent<Image>();
            if (slice)
            {
                float value = (360 - angle) / 360;
                slice.fillAmount = value;
                slice.GetComponent<RectTransform>().rotation = Quaternion.Euler(Vector3.back * angle);
                slice.color = Color.white;
                slices.Add(slice);
            }
        }
        slicePrefab.SetActive(false);
    }

	public Symptom GetResult()
    {
        float angle = GetComponent<RectTransform>().localRotation.eulerAngles.z;
        Debug.Log("angle: " + angle);
        angle %= 360;
        angle /= 3.6f;

        int aux = 0;
        foreach (Data d in datas)
        {
            aux += d.value;
            Debug.Log("angle: " + angle + ", value: " + aux);
            if (angle < aux)
            {
                return d.symptom;
            }
        }

        return Symptom.None;
    }
}

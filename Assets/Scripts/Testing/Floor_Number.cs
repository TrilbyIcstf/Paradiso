using UnityEngine;
using TMPro;

public class Floor_Number : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        text.text = "Floor: " + GameManager.instance.EL.GetFloorNumber();
    }
}

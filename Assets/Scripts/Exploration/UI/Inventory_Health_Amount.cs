using UnityEngine;
using TMPro;

public class Inventory_Health_Amount : MonoBehaviour
{
    public void UpdateHealth()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text = GameManager.instance.PM.GetCurrentHealth() + " / " + GameManager.instance.PM.GetMaxHealth();
    }
}

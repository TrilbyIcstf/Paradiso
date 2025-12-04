using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Upgrade_Description_Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject[] upgradeObjects = new GameObject[2];
    [SerializeField]
    private TextMeshProUGUI[] upgradeTexts = new TextMeshProUGUI[2];
    [SerializeField]
    private Image[] upgradeIcons = new Image[2];

    public void SetUpgrade(string text, Sprite icon, int pos)
    {
        if (pos >= this.upgradeObjects.Length) { return; }
        this.upgradeObjects[pos].SetActive(true);
        this.upgradeTexts[pos].text = text;
        this.upgradeIcons[pos].sprite = icon;
    }
}

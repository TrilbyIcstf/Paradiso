using System.Collections.Generic;
using UnityEngine;

public class Info_Box_Controller : MonoBehaviour
{
    private List<GameObject> infoBoxes = new List<GameObject>();
    private List<Effect_Info_Box> infoBoxScripts = new List<Effect_Info_Box>();

    public void AddInfoBox(GameObject infoBox)
    {
        infoBoxes.Add(infoBox);
        infoBoxScripts.Add(infoBox.GetComponent<Effect_Info_Box>());
    }

    private void Awake()
    {
        SetPos();
    }

    private void Update()
    {
        SetPos();
    }

    private void SetPos()
    {
    }

    private GameObject FirstBox()
    {
        return this.infoBoxes[0];
    }
}

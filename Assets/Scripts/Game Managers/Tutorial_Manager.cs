using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Manager : ManagerBehavior
{
    [SerializeField]
    private List<GameObject> tutorialBoxes = new List<GameObject>();

    private int pos = 0;

    private GameObject currentBox;

    public void StartTutorial()
    {
        this.pos = 0;
        CreateTutorialBox();
    }

    public void TutorialNext()
    {
        Destroy(this.currentBox);
        this.pos++;
        if (this.pos < this.tutorialBoxes.Count)
        {
            CreateTutorialBox();
        }
    }

    public void TutorialBack()
    {
        Destroy(this.currentBox);
        this.pos--;
        if (this.pos >= 0)
        {
            CreateTutorialBox();
        }
    }

    private void CreateTutorialBox()
    {
        GameObject boxHolder = GameObject.FindGameObjectWithTag("TutorialHolder");
        this.currentBox = Instantiate(this.tutorialBoxes[this.pos], boxHolder.transform);
    }
}

using UnityEngine;

public class Tutorial_Controller : MonoBehaviour
{
    public void Next()
    {
        GameManager.instance.TU.TutorialNext();
    }

    public void Back()
    {
        GameManager.instance.TU.TutorialBack();
    }
}

using UnityEngine;

public class Camera_Manager : MonoBehaviour
{
    Camera mainCamera;
    private float currentAspect = 16 / 9;

    private const float TargetWidth = 16f;
    private const float TargetHeight = 9f;

    // Start is called before the first frame update
    void Start()
    {
        this.mainCamera = GetComponent<Camera>();
        FitToScreen();
    }

    private void Update()
    {
        if (mainCamera.aspect != currentAspect)
        {
            FitToScreen();
            this.currentAspect = mainCamera.aspect;
        }
    }

    void FitToScreen()
    {
        float aspect = (float)Screen.width / (float)Screen.height;
        float targetAspect = TargetWidth / TargetHeight;
        //Debug.Log(aspect);
        //Debug.Log(targetAspect);

        if (aspect > targetAspect)
        {
            mainCamera.orthographicSize = TargetWidth;
        }
        else
        {
            float difference = targetAspect / aspect;
            mainCamera.orthographicSize = TargetWidth * difference;
        }
    }
}

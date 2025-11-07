using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the camera and game aspect ratio
/// </summary>
public class Camera_Manager : MonoBehaviour
{
    Camera mainCamera;

    private const float TargetWidth = 16.0f;
    private const float TargetHeight = 9.0f;

    static RenderTexture GameBoyTexture;

    // Start is called before the first frame update
    void Start()
    {
        this.mainCamera = GetComponent<Camera>();
        FitToScreen();

        if (GameBoyTexture != null)
        {
            this.mainCamera.targetTexture = GameBoyTexture;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("p") && GameBoyTexture == null)
        {
            GameBoyTexture = new RenderTexture(160, 90, 16);
            GameBoyTexture.filterMode = FilterMode.Point;
            this.mainCamera.targetTexture = GameBoyTexture;

            GameObject displayTex = new GameObject("Display", typeof(RawImage));
            GameObject canvas = new GameObject("Canvas", typeof(Canvas));
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            displayTex.transform.SetParent(canvas.transform);
            RawImage raw = displayTex.GetComponent<RawImage>();
            raw.texture = GameBoyTexture;
            raw.rectTransform.anchorMin = Vector2.zero;
            raw.rectTransform.anchorMax = Vector2.one;
            raw.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            raw.rectTransform.anchoredPosition = Vector2.zero;
            raw.rectTransform.sizeDelta = Vector2.zero;
            DontDestroyOnLoad(canvas);
        }
    }

    void FitToScreen()
    {
        float aspect = (float)Screen.width / (float)Screen.height;
        float targetAspect = TargetWidth / TargetHeight;

        float scaleHeight = aspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            Rect rect = this.mainCamera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2;

            this.mainCamera.rect = rect;
        } else
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = this.mainCamera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            this.mainCamera.rect = rect;
        }
    }
}

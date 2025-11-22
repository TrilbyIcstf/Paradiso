using System.Collections.Generic;
using UnityEngine;

public class Info_Box_Controller : MonoBehaviour
{
    private List<GameObject> infoBoxes = new List<GameObject>();
    private List<Effect_Info_Box> infoBoxScripts = new List<Effect_Info_Box>();

    private const float BOX_PADDING = 0.5f;

    public void AddInfoBox(GameObject infoBox)
    {
        infoBoxes.Add(infoBox);
        infoBoxScripts.Add(infoBox.GetComponent<Effect_Info_Box>());
        MoveBoxes();
        SetPos();
    }

    private void MoveBoxes()
    {
        foreach (GameObject box in this.infoBoxes)
        {
            box.transform.position = gameObject.transform.position;
        }

        switch (this.infoBoxes.Count)
        {
            case 2:
                {
                    SpriteRenderer sr = FirstBox().GetComponent<SpriteRenderer>();
                    float height = sr.bounds.size.y;
                    float dist = (BOX_PADDING / 2) + (height / 2);
                    Vector3 pos = this.infoBoxes[0].transform.position;
                    pos.y += dist;
                    this.infoBoxes[0].transform.position = pos;
                    pos = this.infoBoxes[1].transform.position;
                    pos.y -= dist;
                    this.infoBoxes[1].transform.position = pos;
                    break;
                }
            case 3:
                {
                    SpriteRenderer sr = FirstBox().GetComponent<SpriteRenderer>();
                    float height = sr.bounds.size.y;
                    float dist = BOX_PADDING + height;
                    Vector3 pos = this.infoBoxes[1].transform.position;
                    pos.y -= dist;
                    this.infoBoxes[1].transform.position = pos;
                    pos = this.infoBoxes[2].transform.position;
                    pos.y += dist;
                    this.infoBoxes[2].transform.position = pos;
                    break;
                }
        }
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
        if (this.infoBoxes.Count == 0) { return; }
        SpriteRenderer boxSprite = FirstBox().GetComponent<SpriteRenderer>();
        float halfWidth = boxSprite.bounds.size.x / 2;

        Vector3 mousePos = Input.mousePosition;
        float screenCenter = Screen.width / 2;
        bool mouseLeftHalf = mousePos.x < screenCenter;

        Vector3 adjustedMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        adjustedMousePos.z = 0;
        adjustedMousePos.x += (halfWidth + 0.5f) * (mouseLeftHalf ? 1.0f : -1.0f);
        adjustedMousePos.y += DetermineAdjust();

        gameObject.transform.position = adjustedMousePos;
    }

    private float DetermineAdjust()
    {
        Camera cam = Camera.main;
        Vector2 bounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Mathf.Abs(mousePos.y) + StackHeight() > bounds.y)
        {
            return -(Mathf.Abs(mousePos.y) + StackHeight() - bounds.y) * Mathf.Sign(mousePos.y);
        }

        return 0;
    }

    private float StackHeight() => this.infoBoxes.Count switch
    {
        1 => SingleHeight(),
        2 => DoubleHeight(),
        3 => TripleHeight(),
        _ => 0.0f
    };

    private float SingleHeight()
    {
        SpriteRenderer sr = FirstBox().GetComponent<SpriteRenderer>();
        float height = sr.bounds.size.y;
        return (height / 2) + BOX_PADDING;
    }

    private float DoubleHeight()
    {
        SpriteRenderer sr = FirstBox().GetComponent<SpriteRenderer>();
        float height = sr.bounds.size.y;
        return height + (BOX_PADDING * 1.5f);
    }

    private float TripleHeight()
    {
        SpriteRenderer sr = FirstBox().GetComponent<SpriteRenderer>();
        float height = sr.bounds.size.y;
        return (height * 1.5f) + (BOX_PADDING * 2);
    }

    private GameObject FirstBox()
    {
        return this.infoBoxes[0];
    }
}

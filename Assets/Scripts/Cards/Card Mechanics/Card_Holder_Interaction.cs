using UnityEngine;

public class Card_Holder_Interaction : MonoBehaviour
{
    [SerializeField]
    private int position;

    private float snapDistance = 1.25f;

    private void Awake()
    {
        GameManager.instance.CUI.SetCardHolder(this.gameObject, position);
    }

    public bool shouldSnap(Vector2 pointPosition)
    {
        if (Vector2.Distance(pointPosition, this.gameObject.transform.position) <= this.snapDistance)
        {
            return true;
        }
        return false;
    }
}

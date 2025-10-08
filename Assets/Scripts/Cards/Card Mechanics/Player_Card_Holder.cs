using UnityEngine;

public class Player_Card_Holder : Card_Holder
{
    private float snapDistance = 1.25f;

    private void Awake()
    {
        GameManager.instance.CUI.SetCardHolder(this.gameObject, this.position);
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

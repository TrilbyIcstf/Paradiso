using UnityEngine;

/// <summary>
/// Child class for player field positions
/// </summary>
public class Player_Card_Holder : Card_Holder
{
    private float snapDistance = 1.25f;

    private void Awake()
    {
        GameManager.instance.CUI.SetCardHolder(this.gameObject, this.position);
    }

    /// <summary>
    /// Checks if the point is close enough to the card holder to snap the card to it
    /// </summary>
    /// <param name="pointPosition">Point being compared to the card holder</param>
    /// <returns>Whether or not it should snap</returns>
    public bool shouldSnap(Vector2 pointPosition)
    {
        if (Vector2.Distance(pointPosition, this.gameObject.transform.position) <= this.snapDistance)
        {
            return true;
        }
        return false;
    }


}

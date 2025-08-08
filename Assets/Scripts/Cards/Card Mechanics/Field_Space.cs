using UnityEngine;

public class Field_Space
{
    private int position;
    private Card_Base card;

    public Field_Space(int position)
    {
        this.position = position;
        this.card = null;
    }

    public Card_Base GetCard()
    {
        return card;
    }

    public void SetCard(Card_Base card)
    {
        this.card = card;
    }
}

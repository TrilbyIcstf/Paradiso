using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Player_Deck_Manager : ManagerBehavior
{
    [SerializeField]
    private Queue<Card_Base> playerDeck = new Queue<Card_Base>();

    public Card_Base DrawTopCard()
    {
        return this.playerDeck.Dequeue();
    }

    public bool DeckIsEmpty()
    {
        return this.playerDeck.Count <= 0;
    }

    public void TestRandomDeck(int size)
    {
        this.playerDeck = new Queue<Card_Base>();

        for (int i = 0; i < size; i++)
        {
            this.playerDeck.Enqueue(Card_Base.RandomizeStats());
        }
    }

    public void SetDeck(Queue<Card_Base> val)
    {
        this.playerDeck = val;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void SetDeck(List<Card_Base> val)
    {
        this.playerDeck = new Queue<Card_Base>(val);
    }

    public int DeckSize()
    {
        return this.playerDeck.Count;
    }

    public void ShuffleDeck()
    {
        List<Card_Base> deck = this.playerDeck.ToList();
        int count = DeckSize();

        for (int i = 0; i < count - 1; i++)
        {
            int r = Random.Range(i, count);
            Card_Base tempCard = deck[i];
            deck[i] = deck[r];
            deck[r] = tempCard;
        }

        this.playerDeck = new Queue<Card_Base>(deck);
    }
}

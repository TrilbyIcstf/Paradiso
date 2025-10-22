using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player_Manager : ManagerBehavior
{
    private int maxHealth = 300;
    private int currentHealth = 300;

    [SerializeField]
    private List<Item_Base> playerItems = new List<Item_Base>();

    private Dictionary<string, Card_Base> playerDeck = new Dictionary<string, Card_Base>();
    private int deckIDCounter = 0;

    public void ActivatePassiveItems(EffectTiming timing, PassiveEffectParameters passParams)
    {
        List<Item_Passive> passiveItems = GetPassiveItems().Where(i => i.IsCorrectTiming(timing)).ToList();
        foreach (Item_Passive item in passiveItems)
        {
            if (GM.IBM.WillTriggerPassive(item.item, passParams))
            {
                GM.IBM.TriggerPassiveItem(item.item, passParams);
            }
        }
    }

    public bool WillPassiveTrigger(EffectTiming timing, PassiveEffectParameters passParams)
    {
        List<Item_Passive> passiveItems = GetPassiveItems().Where(i => i.IsCorrectTiming(timing)).ToList();
        foreach (Item_Passive item in passiveItems)
        {
            if (GM.IBM.WillTriggerPassive(item.item, passParams))
            {
                return true;
            }
        }
        return false;
    }
    
    public int GetMaxHealth()
    {
        return this.maxHealth;
    }

    public int GetCurrentHealth()
    {
        return this.currentHealth;
    }

    public List<Item_Base> GetItems()
    {
        return this.playerItems;
    }

    public List<Item_Active> GetActiveItems()
    {
        return this.playerItems.Where(i => i is Item_Active).Select(i => (Item_Active)i).ToList();
    }

    public List<Item_Passive> GetPassiveItems()
    {
        return this.playerItems.Where(i => i is Item_Passive).Select(i => (Item_Passive)i).ToList();
    }

    public List<Card_Base> GetDeck()
    {
        return this.playerDeck.Values.ToList();
    }

    public Card_Base GetCard(string id)
    {
        return this.playerDeck[id];
    }

    public bool UpdateCard(string id, Card_Base card)
    {
        if (this.playerDeck.ContainsKey(id))
        {
            this.playerDeck[id] = card;
            return true;
        }
        return false;
    }

    public void SetMaxHealth(int val)
    {
        this.maxHealth = val;
    }

    public void SetCurrentHealth(int val)
    {
        this.currentHealth = Mathf.Min(val, this.maxHealth);
    }

    public void TestRandomDeck(int size)
    {
        this.playerDeck = new Dictionary<string, Card_Base>();

        for (int i = 0; i < size; i++)
        {
            Card_Base randCard = Card_Base.RandomizeStats();
            randCard.SetID(i.ToString());
            this.playerDeck[i.ToString()] = randCard;
            this.deckIDCounter = i;
        }
    }
}

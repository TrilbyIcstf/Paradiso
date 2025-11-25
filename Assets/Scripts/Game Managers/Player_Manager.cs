using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages the player's stats and tracks base deck
/// </summary>
public class Player_Manager : ManagerBehavior
{
    private int maxHealth = 300;
    private int currentHealth = 300;

    private int maxEnergy = 30;

    private float energyRegen = 0.08f;

    [SerializeField]
    private List<Item_Base> playerItems = new List<Item_Base>();
    private List<Item> discardedItems = new List<Item>();
    // Tracks a picked up item that hasn't been accepted yet
    private Item_Base tentativeItem;

    private Dictionary<string, Card_Base> playerDeck = new Dictionary<string, Card_Base>();
    private int deckIDCounter = 0;

    /// <summary>
    /// Triggers passive items at the specified timing
    /// </summary>
    /// <param name="timing">The passive timing to trigger at</param>
    /// <param name="passParams">Passive effect parameters</param>
    public void ActivatePassiveItems(EffectTiming timing, PassiveEffectParameters passParams)
    {
        List<Item_Passive> passiveItems = GetPassiveItems().Where(i => i.IsCorrectTiming(timing)).ToList();
        foreach (Item_Passive item in passiveItems)
        {
            if (GM.IBM.WillTriggerPassive(item.GetItemType(), passParams))
            {
                GM.IBM.TriggerPassiveItem(item.GetItemType(), passParams);
            }
        }
    }

    /// <summary>
    /// Checks if any passive effects from items will trigger during the specified timing
    /// </summary>
    /// <param name="timing">The passive timing to check</param>
    /// <param name="passParams">Passive effect parameters</param>
    /// <returns>True if a passive will trigger, false otherwise</returns>
    public bool WillPassiveTrigger(EffectTiming timing, PassiveEffectParameters passParams)
    {
        List<Item_Passive> passiveItems = GetPassiveItems().Where(i => i.IsCorrectTiming(timing)).ToList();
        foreach (Item_Passive item in passiveItems)
        {
            if (GM.IBM.WillTriggerPassive(item.GetItemType(), passParams))
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

    public int GetMaxEnergy()
    {
        return this.maxEnergy;
    }

    public float GetEnergyRegen()
    {
        return this.energyRegen;
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

    public Item_Base GetTentativeItem()
    {
        return this.tentativeItem;
    }

    public Item_Active GetTentativeActive()
    {
        if (this.tentativeItem is Item_Active)
        {
            return (Item_Active)this.tentativeItem;
        }
        return null;
    }

    public bool IsTentativeActive()
    {
        return this.tentativeItem is Item_Active;
    }

    public List<Item> GetDiscardedItems()
    {
        return this.discardedItems;
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

    public void AddMaxHealth(int val)
    {
        this.maxHealth += val;
    }

    public void HealHealth(int val)
    {
        this.currentHealth = Mathf.Min(this.currentHealth + val, this.maxHealth);
    }

    public void AddMaxEnergy(int val)
    {
        this.maxEnergy += val;
    }

    public void AddEnergyRegen(float val)
    {
        this.energyRegen += val;
    }

    public void AddItem(Item_Base item)
    {
        this.playerItems.Add(item);
        TriggerPickupPassive(item.GetItemType());
    }

    public void SetTentativeItem(Item_Base item)
    {
        this.tentativeItem = item;
    }

    public void AddTentativeItem()
    {
        this.playerItems.Add(this.tentativeItem);
        TriggerPickupPassive(this.tentativeItem.GetItemType());
        this.tentativeItem = null;
    }

    private void TriggerPickupPassive(Item item)
    {
        GM.IBM.TriggerItemPickup(item, null);
    }

    public void DiscardTentativeItem()
    {
        this.discardedItems.Add(this.tentativeItem.GetItemType());
        this.tentativeItem = null;
    }

    public void DiscardForTentativeItem(Item discardItem)
    {
        Item_Base itemToDiscard = this.playerItems.FirstOrDefault(i => i.GetItemType() == discardItem);

        if (itemToDiscard != null)
        {
            this.discardedItems.Add(discardItem);
            this.playerItems.Remove(itemToDiscard);
            AddTentativeItem();
        }
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

    public void SetBasicStartingDeck()
    {
        this.playerDeck = Starting_Decks.BasicStartingDeck();
        this.deckIDCounter = this.playerDeck.Count;
    }
}

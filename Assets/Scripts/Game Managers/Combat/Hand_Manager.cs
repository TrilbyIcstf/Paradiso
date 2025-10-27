using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages the hand and keeps cards within it
/// </summary>
public abstract class Hand_Manager : ManagerBehavior
{
    internal List<GameObject> hand = new List<GameObject>();

    private const float HandGravityForce = 10.0f;
    private const float CardGravityForce = 8.0f;

    private float maxDist = 4.5f;

    private int handLimit = 8;

    void FixedUpdate()
    {
        // Player's cards
        Vector2[] positions = CardPositions();
        Vector2[] forces = new Vector2[positions.Length];

        forces = CalculateCardForces(forces, positions);
        forces = CalculateHandForces(forces, positions);

        for (int i = 0; i < forces.Length; i++)
        {
            var gravityScript = this.hand[i].GetComponent<Card_Gravity>();
            gravityScript.ApplyGravityForce(forces[i]);
        }
    }

    private Vector2[] CalculateCardForces(Vector2[] forces, Vector2[] positions)
    {
        for (int i = 0; i < this.hand.Count; i++)
        {
            for (int j = i + 1; j < this.hand.Count; j++)
            {
                Vector2 pos1 = positions[i];
                Vector2 pos2 = positions[j];
                float distance = Mathf.Max(Vector2.Distance(pos1, pos2), 1.0f);

                if (distance < this.maxDist)
                {
                    forces[i] += (pos1 - pos2).normalized * (1 - (distance / this.maxDist)) * CardGravityForce;
                    forces[j] += (pos2 - pos1).normalized * (1 - (distance / this.maxDist)) * CardGravityForce;
                }
            }
        }

        return forces;
    }

    private Vector2[] CalculateHandForces(Vector2[] forces, Vector2[] positions)
    {
        Combat_Area_Marker playerHand = GetHandArea();

        if (playerHand == null)
        {
            return forces;
        }

        for (int i = 0; i < this.hand.Count; i++)
        {

            Vector2 cardPos = positions[i];
            if (playerHand.DistanceFromArea(cardPos) > 0.25f)
            {
                Vector2 closestPos = playerHand.ClosestPoint(cardPos);
                float distance = Vector2.Distance(cardPos, closestPos);
                Vector2 handDirection = (closestPos - cardPos).normalized;
                Vector2 gravityForce = handDirection * Mathf.Max((distance / 7), 0.5f) * HandGravityForce;
                forces[i] += gravityForce;
            }
        }

        return forces;
    }

    internal abstract Combat_Area_Marker GetHandArea();

    public Vector2[] CardPositions()
    {
        Vector2[] positions = this.hand.Select(c => (Vector2)c.transform.position).ToArray();

        return positions;
    }

    public void SetSortingOrder(GameObject topCard)
    {
        List<Card_UI> scriptList = GetUIScripts();
        List<Card_UI> sortedHand = scriptList.OrderBy(c => c.GetSortingOrder()).ToList();
        Card_UI topScript = topCard.GetComponent<Card_UI>();
        int oldOrder = topScript.GetSortingOrder();

        int order = 1;
        foreach (Card_UI card in sortedHand)
        {
            if (card.GetSortingOrder() != oldOrder)
            {
                card.SetSortingOrder(order);
                order++;
            }
        }
        topScript.SetSortingOrder(order);
    }

    private List<Card_UI> GetUIScripts()
    {
        return this.hand.Select(c => c.GetComponent<Card_UI>()).ToList();
    }

    public void DrawToHand(GameObject card)
    {
        GM.CUI.DrawToHand(card);
        AddCard(card);
    }

    public void DrawToEnemyHand(GameObject card)
    {
        GM.CUI.DrawToEnemyHand(card);
        AddCard(card);
    }

    public void AddCard(GameObject card)
    {
        this.hand.Add(card);
        SetSortingOrder(card);
    }

    public void RemoveCard(GameObject card)
    {
        this.hand.Remove(card);
    }

    public void ResetHand()
    {
        foreach (GameObject card in this.hand)
        {
            Destroy(card);
        }

        this.hand = new List<GameObject>();
    }

    public GameObject PickRandomCard()
    {
        int randomPos = PickRandomCardPos();
        if (randomPos == -1) { return null; }
        return this.hand[randomPos];
    }

    public int PickRandomCardPos()
    {
        if (this.hand.Count == 0) { return -1; }
        return UnityEngine.Random.Range(0, this.hand.Count);
    }

    public List<int> PickRandomCardsPos(int amount, Predicate<GameObject> filter = null)
    {
        List<int> indexList = new List<int>();
        if (filter != null)
        {
            for (int i = 0; i < this.hand.Count; i++)
            {
                if (filter(this.hand[i]))
                {
                    indexList.Add(i);
                }
            }
        } else
        {
            indexList = Enumerable.Range(0, this.hand.Count).ToList();
        }
        if (amount > indexList.Count) { amount = indexList.Count; }
        if (amount == indexList.Count)
        {
            return indexList;
        }

        List<int> randList = new List<int>();
        do
        {
            int randInt = UnityEngine.Random.Range(0, amount);
            int randIndex = indexList[randInt];
            if (!randList.Contains(randIndex))
            {
                randList.Add(randIndex);
            }
        } while (randList.Count < amount);

        return randList;
    }

    public int HandSize()
    {
        return this.hand.Count;
    }

    public bool AtHandLimit()
    {
        return this.hand.Count >= this.handLimit;
    }

    public GameObject GetCard(int pos)
    {
        if (pos >= this.hand.Count) { return null; }
        return this.hand[pos];
    }
}

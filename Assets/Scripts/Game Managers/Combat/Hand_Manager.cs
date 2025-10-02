using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Hand_Manager : MonoBehaviour
{
    internal List<GameObject> hand = new List<GameObject>();

    public float gravityStrength = 5.0f;
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
                    forces[i] += (pos1 - pos2).normalized * (1 - (distance / this.maxDist)) * this.gravityStrength;
                    forces[j] += (pos2 - pos1).normalized * (1 - (distance / this.maxDist)) * this.gravityStrength;
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
            if (playerHand.DistanceFromHand(cardPos) > 0.25f)
            {
                Vector2 closestPos = playerHand.ClosestPoint(cardPos);
                float distance = Vector2.Distance(cardPos, closestPos);
                Vector2 handDirection = (closestPos - cardPos).normalized;
                Vector2 gravityForce = handDirection * Mathf.Max((distance / 7), 0.5f) * this.gravityStrength * 2;
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

    public void AddCard(GameObject card)
    {
        this.hand.Add(card);
        SetSortingOrder(card);
    }

    public void RemoveCard(GameObject card)
    {
        this.hand.Remove(card);
    }

    public GameObject PickRandomCard()
    {
        int randomPos = PickRandomCardPos();
        return this.hand[randomPos];
    }

    public int PickRandomCardPos()
    {
        return Random.Range(0, this.hand.Count);
    }

    public int HandSize()
    {
        return this.hand.Count;
    }

    public bool AtHandLimit()
    {
        return this.hand.Count >= this.handLimit;
    }
}

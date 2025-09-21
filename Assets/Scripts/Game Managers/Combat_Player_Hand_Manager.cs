using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Combat_Player_Hand_Manager : MonoBehaviour
{
    public List<GameObject> playerHand = new List<GameObject>();
    private Combat_Area_Marker handArea;

    public float gravityStrength = 5.0f;
    private float maxDist = 4.5f;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2[] positions = CardPositions();
        Vector2[] forces = new Vector2[positions.Length];

        forces = CalculateCardForces(forces, positions);
        forces = CalculateHandForces(forces, positions);

        for (int i = 0; i < forces.Length; i++)
        {
            var gravityScript = this.playerHand[i].GetComponent<Card_Gravity>();
            gravityScript.ApplyGravityForce(forces[i]);
        }
    }

    private Vector2[] CalculateCardForces(Vector2[] forces, Vector2[] positions)
    {
        for (int i = 0; i < this.playerHand.Count; i++)
        {
            for (int j = i + 1; j < this.playerHand.Count; j++)
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
        Combat_Area_Marker playerHand = GameManager.instance.CUI.uiCoordinator.PlayerHandArea();
        for (int i = 0; i < this.playerHand.Count; i++)
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

    public Vector2[] CardPositions()
    {
        Vector2[] positions = this.playerHand.Select(c => (Vector2)c.transform.position).ToArray();

        return positions;
    }

    public void SetHandArea(Combat_Area_Marker script)
    {
        this.handArea = script;
    }

    public void AddCard(GameObject card)
    {
        this.playerHand.Add(card);
    }

    public void RemoveCard(GameObject card)
    {
        this.playerHand.Remove(card);
    }
}

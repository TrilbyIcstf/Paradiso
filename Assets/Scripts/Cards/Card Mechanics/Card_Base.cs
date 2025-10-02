using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/New Card", order = 3)]
[System.Serializable]
public class Card_Base : ScriptableObject
{
    public string cardName;

    public int power;
    public int defense;

    public CardElement element;
}

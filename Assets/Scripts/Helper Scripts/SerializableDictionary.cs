using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A dictionary that can be edited through the Unity editor
/// </summary>
/// <typeparam name="TKey">Var type of the key</typeparam>
/// <typeparam name="TValue">Var type of the value</typeparam>
[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<SerializableKeyValuePair<TKey, TValue>> _serializableList = new List<SerializableKeyValuePair<TKey, TValue>>();

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        Clear();
        foreach (var serializedPair in _serializableList)
        {
            if (serializedPair.Key != null)
            {
                this[serializedPair.Key] = serializedPair.Value;
            }
        }
    }
}

[System.Serializable]
public class SerializableKeyValuePair<TKey, TValue>
{
    public TKey Key;
    public TValue Value;

    public SerializableKeyValuePair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    // stupid, but unity won't parse properties to json, so use public variables
    // but set values with the set function because otherwise the observers will
    // not update

    public ItemID ID;
    public int Amount;
    public int ItemCost = 1;

    public void Set(ItemID id, int amount, int cost = 1)
    {
        ID = id;
        Amount = amount;
    }

    public void Add(int amount = 1)
    {
        Amount += amount;
    }
}

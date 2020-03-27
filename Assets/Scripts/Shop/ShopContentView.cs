using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopContentView : MonoBehaviour
{
    [SerializeField] ShopEntry prefab;
    [SerializeField] int amount;

    ItemType type = ItemType.All;

    public ShopGui shop;

    void clearEntries()
    {
        foreach(Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void Populate(List<InventoryItem> stock, Transaction mode)
    {
        clearEntries();

        for (int i = 0; i < stock.Count; i++)
        {
            if (stock[i].ID == ItemID.Empty) continue; // just to be safe, but should not happen

            var newEntry = Instantiate(prefab, transform);
            newEntry.SetUp(stock[i], mode, shop);
        }
    }

}

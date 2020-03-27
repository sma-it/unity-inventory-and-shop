using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JsonWrapper
{
    // unity cannot serialize a string directly, so we need to wrap it inside
    // another class
    public List<InventoryItem> Items;
    public int Funds;
}

public class Inventory
{
    List<InventoryItem> items;
    public List<InventoryItem> Items => items;

    int funds;
    public int Funds => funds;

    public const int SIZE = 12;

    private Inventory()
    {
        load();
    }

    private static Inventory instance = null;

    public static Inventory GetInstance()
    {
        if (instance == null) instance = new Inventory();
        return instance;
    }

    private void load()
    {
        try
        {
            string data = System.IO.File.ReadAllText(FilePath.Inventory);
            var wrapper = JsonUtility.FromJson<JsonWrapper>(data);
            items = wrapper.Items;
            funds = wrapper.Funds;

            if (items.Count != SIZE)
            {
                throw new Exception("Inventory invalid");
            }
        } catch(Exception e)
        {
            Debug.Log(e.Message);
            items = new List<InventoryItem>();
            for(int i = 0; i < SIZE; i++)
            {
                items.Add(new InventoryItem
                {
                    ID = ItemID.Empty,
                    Amount = 0,
                });
            }
            funds = 0;
        }
        notifyObservers();
    }

    public void Save()
    {
        try
        {
            var wrapper = new JsonWrapper
            {
                Items = items,
                Funds = funds,
            };
            string output = JsonUtility.ToJson(wrapper, true);
            System.IO.File.WriteAllText(FilePath.Inventory, output);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

    }

    public bool HasRoomForItem(ItemID id)
    {
        bool hasRoom = false;
        items.ForEach((item) => {
            if (item.ID == id || item.ID == ItemID.Empty) hasRoom = true;
        });
        return hasRoom;
    }

    public void AddItem(ItemID id, int amount = 1)
    {
        for(int i = 0; i < items.Count; i++) {
            if (items[i].ID == id)
            {
                // we got this item in the list, just increase the amount
                items[i].Add(amount);
                notifyObservers();
                Save();
                return;
            } 
        }

        // if we get here, the item was not found. Add on the first empty spot
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == ItemID.Empty)
            {
                items[i].Set(id, amount);
                notifyObservers();
                Save();
                return;
            }
        }
    }

    public void RemoveItem(InventoryItem item)
    {
        item.Set(ItemID.Empty, 0);
        notifyObservers();
        Save();
    }

    public void RemoveItem(ItemID id, int amount)
    {
        var item = items.Find((f) => f.ID == id);
        if (item != null)
        {
            item.Amount -= amount;
            if (item.Amount <= 0)
            {
                item.ID = ItemID.Empty;
                item.Amount = 0;
            }
            notifyObservers();
            Save();
        }
    }

    public void AddFunds(int amount)
    {
        funds += amount;
        notifyObservers();
        Save();
    }

    public void RemoveFunds(int amount)
    {
        funds -= amount;
        notifyObservers();
        Save();
    }

    List<IInventoryObserver> observers = new List<IInventoryObserver>();

    public void registerObserver(IInventoryObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public void removeObserver(IInventoryObserver observer)
    {
        if (observers.Contains(observer))
        {
            observers.RemoveAll((f) => f == observer);
        }
    }

    void notifyObservers()
    {
        observers.ForEach((observer) => observer.ReloadData());
    }
}

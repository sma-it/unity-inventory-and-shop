using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopGui : MonoBehaviour, IInventoryObserver
{
    [SerializeField] ToggleButton BuyButton;
    [SerializeField] ToggleButton SellButton;
    [SerializeField] ShopContentView contentView;
    [SerializeField] Text ShopNameText;
    [SerializeField] string ShopName;
    [SerializeField] ItemType StoreType;
    [SerializeField] Text CashText;

    List<InventoryItem> storeItems;
    public List<InventoryItem> StoreItems => storeItems;

    int cash;
    public int Cash
    {
        get => cash;
        set
        {
            cash = value;
            CashText.text = "$ " + cash;
        }
    }

    Transaction mode;

    // Start is called before the first frame update
    void Start()
    {
        ShopNameText.text = ShopName;
        contentView.shop = this;

        load();
        ActivateBuyMode();
        Inventory.GetInstance().registerObserver(this);
    }

    ~ShopGui()
    {
        Inventory.GetInstance().removeObserver(this);
    }

    // implements Inventory Observer
    public void ReloadData()
    {
        if (mode == Transaction.UserSells)
        {
            PopulateWithUserInventory();
        }
    }

    public void ActivateBuyMode()
    {
        mode = Transaction.UserBuys;
        BuyButton.Focus();
        SellButton.UnFocus();
        appreciateItems(storeItems, Transaction.UserBuys);
        contentView.Populate(storeItems, Transaction.UserBuys);
    }

    public void ActivateSellMode()
    {
        mode = Transaction.UserSells;
        BuyButton.UnFocus();
        SellButton.Focus();

        List<InventoryItem> items = new List<InventoryItem>();
        PopulateWithUserInventory();
    }

    public void PopulateWithUserInventory()
    {
        List<InventoryItem> items = new List<InventoryItem>();
        var inventory = Inventory.GetInstance();

        for (int i = 0; i < inventory.Items.Count; i++)
        {
            if (inventory.Items[i].ID == ItemID.Empty) continue;
            items.Add(inventory.Items[i]);
        }

        appreciateItems(items, Transaction.UserSells);
        contentView.Populate(items, Transaction.UserSells);
    }

    private void load()
    {
        try
        {
            string data = System.IO.File.ReadAllText(FilePath.Shop(ShopName));
            var wrapper = JsonUtility.FromJson<JsonWrapper>(data);
            storeItems = wrapper.Items;
            Cash = wrapper.Funds;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            storeItems = new List<InventoryItem>();
            for (ItemID id = 0; id < ItemID.Empty; id++)
            {
                if (StoreType == ItemType.All || ItemInfo.getItemType(id) == StoreType)
                {
                    storeItems.Add(new InventoryItem
                    {
                        ID = id,
                        Amount = UnityEngine.Random.Range(1, 20),
                    });
                }
                
            }
            Cash = 100;
            Save();
        }
    }

    public void Save()
    {
        try
        {
            var wrapper = new JsonWrapper
            {
                Items = storeItems,
                Funds = cash,
            };
            string output = JsonUtility.ToJson(wrapper, true);
            System.IO.File.WriteAllText(FilePath.Shop(ShopName), output);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void SellToUser(InventoryItem item, int amount)
    {
        var inventory = Inventory.GetInstance();
        int cost = amount * item.ItemCost;

        if (inventory.Funds < cost) return;

        // adjust amount
        item.Amount -= amount;
        Cash += cost;
        if(item.Amount <= 0)
        {
            storeItems.Remove(item);
        }

        // update user inventory
        inventory.AddItem(item.ID, amount);
        inventory.RemoveFunds(cost);

        // visual store update
        appreciateItems(storeItems, Transaction.UserBuys);
        contentView.Populate(storeItems, Transaction.UserBuys);
    }

    public void BuyFromUser(InventoryItem item, int amount)
    {
        var cost = amount * item.ItemCost;
        if (Cash < cost) return;
        Cash -= cost;

        // add to shop
        InventoryItem target = storeItems.Find((f) => f.ID == item.ID);
        if (target != null)
        {
            target.Amount += amount;
        } else
        {
            target = new InventoryItem();
            target.Set(item.ID, amount);
            storeItems.Add(target);
        }

        // update user inventory
        Inventory.GetInstance().RemoveItem(item.ID, amount);
        Inventory.GetInstance().AddFunds(cost);
    }

    // find the right price for every item
    void appreciateItems(List<InventoryItem> items, Transaction transaction)
    {
        items.ForEach((item) => {
            var value = ItemInfo.getBaseValue(item.ID);
            var type = ItemInfo.getItemType(item.ID);

            if (transaction == Transaction.UserBuys)
            {
                if(StoreType == ItemType.All)
                {
                    // general store, prices are higher
                    item.ItemCost = (int)Math.Ceiling(value * 1.6);
                } else if (StoreType == type)
                {
                    // this shop is specialized this kind of items and sells them cheaper
                    item.ItemCost = (int)Math.Ceiling(value * 1.3);
                } else
                {
                    // the shop wants to get rid of this item
                    item.ItemCost = value;
                }
                if (item.Amount > 20)
                {
                    // overstocked!! Sell cheaper
                    item.ItemCost = (int)Math.Ceiling(item.ItemCost * 0.8);
                }
            } else
            {
                if(StoreType == ItemType.All)
                {
                    // general store does not give great prices
                    item.ItemCost = (int)Math.Floor(value * 0.5);
                } else if (StoreType == type)
                {
                    // this shop is interested in this item
                    item.ItemCost = (int)Math.Floor(value * 0.8);
                } else
                {
                    // the shop does not want the item
                    item.ItemCost = (int)Math.Floor(value * 0.4);
                }
                StoreItems.ForEach((storeItem) =>
                {
                    if (storeItem.ID == item.ID && storeItem.Amount > 20)
                    {
                        // overStocked!! The shop is not that interested now.
                        item.ItemCost = (int)Math.Floor(item.ItemCost * 0.8);
                    } 
                });
            }
        });
    }
}

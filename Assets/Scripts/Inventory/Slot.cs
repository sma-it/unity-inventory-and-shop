using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] Text amountText;
    [SerializeField] Image image;

    private InventoryItem inventoryItem;

    public void LinkToInventoryItem(InventoryItem item)
    {
        inventoryItem = item;
        ReloadData();
    }

    public void ReloadData()
    {
        if (inventoryItem.Amount == 0)
        {
            this.amountText.text = "";
            image.sprite = Resources.Load<Sprite>(ItemID.Empty.ToString());
        }
        else
        {
            amountText.text = inventoryItem.Amount.ToString();
            image.sprite = Resources.Load<Sprite>(inventoryItem.ID.ToString());
        }
    }

    public void onClick()
    {
        if (InventoryGui.DeleteMode)
        {
            if (inventoryItem != null)
            {
                inventoryItem.Set(ItemID.Empty, 0);
                Inventory.GetInstance().RemoveItem(inventoryItem);
            }
        }
    }

}

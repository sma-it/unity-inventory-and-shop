using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Item : MonoBehaviour
{
    [SerializeField] ItemID ItemID;

    private void Start()
    {
        Image image = GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>(ItemID.ToString());
    }

    public void PickupItem()
    {
        var inventory = Inventory.GetInstance();
        if (inventory.HasRoomForItem(ItemID))
        {
            inventory.AddItem(ItemID);
        }
    }
}

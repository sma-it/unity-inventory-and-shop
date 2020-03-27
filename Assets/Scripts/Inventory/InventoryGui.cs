using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGui : MonoBehaviour, IInventoryObserver
{
    public static bool DeleteMode = false;

    [SerializeField] Toggle deleteItems;
    [SerializeField] Text fundsText;
    [SerializeField] Slot[] slots;
    // Start is called before the first frame update
    void Start()
    {
        var inventory = Inventory.GetInstance();
        for(int i = 0; i < slots.Length && i < inventory.Items.Count; i++)
        {
            slots[i].LinkToInventoryItem(inventory.Items[i]);
            inventory.registerObserver(this);
        }
        fundsText.text = Inventory.GetInstance().Funds.ToString();

        deleteItems.onValueChanged.AddListener(delegate
        {
            DeleteMode = deleteItems.isOn;
        });
    }

    public void ReloadData()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ReloadData();
        }
        fundsText.text = Inventory.GetInstance().Funds.ToString();
    }

    ~InventoryGui()
    {
        Inventory.GetInstance().removeObserver(this);
    }
}

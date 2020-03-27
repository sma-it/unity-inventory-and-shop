using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Transaction
{
    UserBuys,
    UserSells,
}

public class ShopEntry : MonoBehaviour
{
    [SerializeField] public Image icon;
    [SerializeField] public Text nameText;
    [SerializeField] public Text amountText;
    [SerializeField] public Text transactionAmountText;
    [SerializeField] public Text transactionButtonText;
    [SerializeField] public Text transactionCostText;

    InventoryItem item;
    int transactionAmount;
    Transaction transaction;
    ShopGui shop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUp(InventoryItem item, Transaction transaction, ShopGui shop)
    {
        this.item = item;
        this.shop = shop;

        icon.sprite = Resources.Load<Sprite>(item.ID.ToString());
        nameText.text = item.ID.ToString();
        amountText.text = item.Amount.ToString();
        
        setTransaction(transaction);
        setTransactionAmount(1);
        setTransactionCost();
    }

    void setTransaction(Transaction transaction)
    {
        this.transaction = transaction;
        switch(transaction)
        {
            case Transaction.UserBuys:
                transactionButtonText.text = "Buy";
                break;
            case Transaction.UserSells:
                transactionButtonText.text = "Sell";
                break;
        }
    }

    void setTransactionAmount(int value)
    {
        transactionAmount = value;
        transactionAmountText.text = transactionAmount.ToString();
    }

    void setTransactionCost()
    {
        transactionCostText.text = "$ " + item.ItemCost * transactionAmount;
    }

    public void IncreaseAmount()
    {
        if (transactionAmount < item.Amount)
        {
            setTransactionAmount(transactionAmount + 1);
            setTransactionCost();
        }
    }

    public void DecreaseAmount()
    {
        if (transactionAmount > 1)
        {
            setTransactionAmount(transactionAmount - 1);
            setTransactionCost();
        }
    }

    public void OnTransactionClick()
    {
        if (transaction == Transaction.UserBuys)
        {
            shop.SellToUser(item, transactionAmount);
        } else
        {
            shop.BuyFromUser(item, transactionAmount);
        }
    }
}

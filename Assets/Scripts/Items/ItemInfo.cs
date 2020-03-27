using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ItemID
{
    Flint,
    Tinder,
    Wood,
    Coal,
    Iron,
    Torch,
    Cloth,
    Meat,
    Seeds,
    Corn,
    Bowl,
    Rabbit,
    Chisel,
    Apple,
    Chestnuts,
    Bucket,
    Rod,
    Fish,
    Sword,
    Bow,
    Arrow,
    Drumstick,
    Chalk,
    String,
    Bugs,
    Empty,
}

public enum ItemType
{
    Food,
    Weapon,
    Utility,
    Resource,
    All,
    None,
}

public class ItemInfo
{

    public static ItemType getItemType(ItemID id)
    {
        switch(id)
        {
            case ItemID.Tinder:
            case ItemID.Wood:
            case ItemID.Coal:
            case ItemID.Iron:
            case ItemID.Cloth:
            case ItemID.Chalk:
                return ItemType.Resource;

            case ItemID.Flint:
            case ItemID.Torch:
            case ItemID.Bowl:
            case ItemID.Bucket:
            case ItemID.Chisel:
            case ItemID.Rod:
            case ItemID.String:
                return ItemType.Utility;

            case ItemID.Arrow:
            case ItemID.Bow:
            case ItemID.Sword:
                return ItemType.Weapon;

            case ItemID.Meat:
            case ItemID.Seeds:
            case ItemID.Corn:
            case ItemID.Rabbit:
            case ItemID.Apple:
            case ItemID.Chestnuts:
            case ItemID.Fish:
            case ItemID.Bugs:
            case ItemID.Drumstick:
                return ItemType.Food;

            default: return ItemType.None;
        }
    }

    public static int getBaseValue(ItemID id)
    {
        switch (id)
        {
            case ItemID.Flint: return 5;
            case ItemID.Tinder: return 2;
            case ItemID.Wood: return 8;
            case ItemID.Coal: return 7;
            case ItemID.Iron: return 15;
            case ItemID.Torch: return 25;
            case ItemID.Cloth: return 35;
            case ItemID.Meat: return 27;
            case ItemID.Seeds: return 2;
            case ItemID.Corn: return 6;
            case ItemID.Bowl: return 50;
            case ItemID.Rabbit: return 30;
            case ItemID.Chisel: return 50;
            case ItemID.Apple: return 3;
            case ItemID.Chestnuts: return 2;
            case ItemID.Bucket: return 40;
            case ItemID.Rod: return 55;
            case ItemID.Fish: return 9;
            case ItemID.Sword: return 2000;
            case ItemID.Bow: return 2800;
            case ItemID.Arrow: return 6;
            case ItemID.Drumstick: return 18;
            case ItemID.Chalk: return 4;
            case ItemID.String: return 12;
            case ItemID.Bugs: return 4;

            default: return 0;
        }
    }

}

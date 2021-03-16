using System;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class Item:ScriptableObject
    {
        private string ItemName { get; set; }//Names
        private string ItemDescription { get; set; }//And description
        private Sprite _inventoryIcon;//Some items have sprites
        public virtual void Use()
        {
            //Some items can be used
        }
    }

    public class PlayerInventory
    {
        //To be implemented
        private Dictionary<int, Item> _items;//_items[id]=Item_id
    }
}
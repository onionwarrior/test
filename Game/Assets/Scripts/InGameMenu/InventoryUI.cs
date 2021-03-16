using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{


    private Inventory inventory;
	private Transform itemSlotContainer;
	private Transform itemSlotTemplate;


    private void Awake()
	{
		itemSlotContainer = transform.Find("itemSlotContainer");

		itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
	}

	public void GetInventory(Inventory inventory,bool refresh = true)
	{
		this.inventory = inventory;
		if(refresh)	
			RefreshInventoryItems();
	}

	public void RefreshInventoryItems()
	{

		int x = 0, y = 0;
		float cellSize = 25f;
		foreach (Item item in inventory.GetItemList())
		{
			RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
			itemSlotRectTransform.gameObject.SetActive(true);
			itemSlotRectTransform.anchoredPosition = new Vector2(x * cellSize, y * cellSize);
			x++;
			if(x > 4)
			{
				x = 0;
				y--;
			}
		}
	}
}

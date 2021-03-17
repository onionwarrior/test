using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class WorldItems : MonoBehaviour
{

    private GameObject player;
    private PlayerScript playerScript;


    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
    }



    private void OnMouseDown()
	{
        if(Vector3.Distance(player.transform.position, transform.position) < 5f)
		{
            playerScript.inventory.AddItem(new Item { itemType = Item.ItemType.Cube1, amount = 1 });
            playerScript.RefreshItemsUI();   
            Destroy(gameObject);
        }

    }
}

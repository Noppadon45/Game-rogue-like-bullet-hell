using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInventory p = collision.GetComponent<PlayerInventory>();
        if (p)
        {
            bool randombool = Random.Range(0, 2) == 0;

            OpenTreasureChest(p, randombool);

            Destroy(gameObject);
        }
    }

    public void OpenTreasureChest(PlayerInventory inventory, bool isHighterTier)
    {
        foreach (PlayerInventory.Slot s in inventory.WeaponSlot)
        {
            Weapon w = s.item as Weapon;
            if (w.data.evolutionData == null)
            {
                continue;
            }
            foreach (ItemData.Evolution e in w.data.evolutionData)
            {
                if (e.condition == ItemData.Evolution.Condition.treasureChest)
                {
                    bool attempt = w.AttemptEvolution(e, 0);
                    if (attempt)
                    {
                        return;
                    }
                }
            }
        }
    }
}

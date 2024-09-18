using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    Inventory Inventory;
    // Start is called before the first frame update
    void Start()
    {
        Inventory = FindObjectOfType<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OpenChest();
            Destroy(gameObject);
        }
    }

    public void OpenChest()
    {
        if (Inventory.GetPossibleEvolution().Count <= 0)
        {
            Debug.LogWarning("No Available Evolve");
        }

        WeaponEvolution toEvolve = Inventory.GetPossibleEvolution()[Random.Range(0, Inventory.GetPossibleEvolution().Count)];
        Inventory.EvolveWeapon(toEvolve);
    }
}

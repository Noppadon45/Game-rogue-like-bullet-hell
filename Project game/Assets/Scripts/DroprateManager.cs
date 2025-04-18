using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroprateManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string Name;
        public GameObject ItemPrefab;
        public float Droprate;
    }

    public List<Drops> drops;

    void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) //When stop game stop loading drop item
        {
            return;
        }
        // Generate a random number between 0 and 100 to determine which items to drop
        float randomnumber = UnityEngine.Random.Range(0f, 100f);
        List<Drops> possibledrop = new List<Drops>();

        // Loop through each drop and check if it meets the drop rate condition
        foreach (Drops rate in drops)
        {
            if (randomnumber <= rate.Droprate)
            {
                possibledrop.Add(rate);
            }
        }
        //Check possible drop rate
        if (possibledrop.Count > 0)
        {
            // Randomly choose one drop from the possible drops list
            Drops drops = possibledrop[UnityEngine.Random.Range(0, possibledrop.Count)];
            // Instantiate the selected item at the position of the object with the drop
            Instantiate(drops.ItemPrefab, transform.position, Quaternion.identity);
        }
       
    }
}

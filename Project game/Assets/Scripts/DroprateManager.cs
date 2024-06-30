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
        float randomnumber = UnityEngine.Random.Range(0f, 100f);
        List<Drops> possibledrop = new List<Drops>();

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
            Drops drops = possibledrop[UnityEngine.Random.Range(0, possibledrop.Count)];
            Instantiate(drops.ItemPrefab, transform.position, Quaternion.identity);
        }
       
    }
}

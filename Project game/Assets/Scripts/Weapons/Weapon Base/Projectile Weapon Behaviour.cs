using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

//Script Projectile Behaviour [Place on prefab weapons projectiles]
public class ProjectileWeaponBehaviour : MonoBehaviour
{
    protected Vector3 direction;
    public float DestroyAfterSecound;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, DestroyAfterSecound);
    }

    // Update is called once per frame
    public void DirectionChecker(Vector3 Dir)
    {
        direction = Dir;

        float dirx = direction.x;
        float diry = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if (dirx < 0 && diry == 0) //left
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
        }
        else if (dirx > 0 && diry == 0) //right
        {
            scale.x = scale.x * 1;
            scale.y = scale.y * 1;
        }
        else if (dirx == 0 && diry > 0) //up
        {
            rotation.z = 90f;
        }
        else if (dirx == 0 && diry < 0) //down
        {
            rotation.z = -90f;
        }
        else if (dirx < 0 && diry > 0) //left up
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = -45f;
        }
        else if (dirx < 0 && diry < 0) //left down
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = 45f;
        }
        else if (dirx > 0 && diry > 0) //right up
        {
            scale.x = scale.x * 1;
            scale.y = scale.y * 1;
            rotation.z = 45f;
        }
        else if (dirx > 0 && diry < 0) //right down
        {
            scale.x = scale.x * 1;
            scale.y = scale.y * 1;
            rotation.z = -45f;
        }


        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
    }
}

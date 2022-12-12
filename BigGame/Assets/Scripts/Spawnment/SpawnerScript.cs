using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject gigaPrefab;
    public GameObject start;

    public void ButtonClick()
    {
        float x = start.transform.position.x;
        float y = Convert.ToSingle(0.13);
        float z = start.transform.position.z;
        SpawnObjectAtLocation(x, y, z);
    }

    private void SpawnObjectAtLocation(float posX, float posY, float posZ)
    {
        
        GameObject newObject;
        newObject = Instantiate(gigaPrefab, new Vector3(posX, posY, posZ), transform.rotation);
        newObject.transform.Rotate(0, 90, 0);
        newObject.transform.SetParent(GameObject.Find("land").transform, false);

    }
}

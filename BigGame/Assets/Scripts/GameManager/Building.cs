using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class Building : MonoBehaviour
{
    Camera cam;
    public LayerMask mask;
    public bool isBuilding=false;
    //private GameObject card;
    public GameObject parent;

    void Start()
    {
        cam = Camera.main;
    }

    public void StartBuilding(GameObject stucture, GameObject _card)
    {
        if (GetComponent<GameManager>().CanPlayerMove())
        {
            if (!isBuilding)
            {
                isBuilding = true;
                StartCoroutine(WhereToBuild(stucture,_card));
            }
            else
            {
                isBuilding = false;
            }
           
        }
    }
    public void Build(GameObject position, GameObject stucture)
    {
        Transform posi = position.transform;
        Destroy(position);
        GameObject building = Instantiate(stucture, posi.position,posi.rotation);
        building.transform.SetParent(parent.transform,true);
    }


    IEnumerator WhereToBuild(GameObject stucture,GameObject card)
    {
        while (isBuilding)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray,out hit,100,mask))
                {
                    Debug.Log("I hit something");
                    GameObject hitObject = hit.collider.gameObject;
                    Debug.Log(hitObject.name);
                    isBuilding=false;
                    Build(hitObject, stucture);
                    Destroy(card);
                }
                else
                {
                    isBuilding = false;
                }
            }
            yield return null;
        }

        yield return null;
    }


}

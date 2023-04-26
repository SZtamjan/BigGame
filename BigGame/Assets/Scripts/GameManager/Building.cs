using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using static GameManager;

public class Building : MonoBehaviour
{
    Camera cam;
    public LayerMask mask;
    public bool isBuilding = false;
    //private GameObject card;
    public GameObject parent;

    private GameObject halfTransparent;

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
                StartCoroutine(WhereToBuild(stucture, _card));
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
        GameObject building = Instantiate(stucture, posi.position, posi.rotation);
        building.transform.SetParent(parent.transform, true);
    }


    IEnumerator WhereToBuild(GameObject stucture, GameObject card)
    {
        halfTransparent = Instantiate(stucture, new Vector3(0f, -10f, 0f), transform.rotation);

        Renderer renderer = halfTransparent.GetComponent<Renderer>();
        Material[] materials = renderer.materials;
        renderer.shadowCastingMode = ShadowCastingMode.Off;

        foreach (Material mat in materials)
        {           
            mat.SetFloat("_Dissolve", 0.5f);
        }
        while (isBuilding)
        {
            Ray ray1 = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit1;
            if (Physics.Raycast(ray1, out raycastHit1, 100, mask))
            {
                GameObject hit1 = raycastHit1.collider.gameObject;
                Vector3 place = hit1.transform.position;
                place.y += 0.01f;
                halfTransparent.transform.position = place;
            }
            else
            {
                halfTransparent.transform.position = new Vector3(0f, -10f, 0f);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, mask))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    Debug.Log(hitObject.name);
                    isBuilding = false;
                    Build(hitObject, stucture);
                    //Destroy(card);
                }
                else
                {
                    isBuilding = false;
                }
            }
            yield return null;
        }
        Destroy(halfTransparent);

        yield return null;
    }


}

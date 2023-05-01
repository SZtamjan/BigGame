using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using static GameManager;

public class Building : MonoBehaviour
{
    public static Building Instance;
    Camera cam;
    public LayerMask mask;
    public bool isBuilding = false;

    public GameObject parent;
    private GameObject halfTransparent;

    [SerializeField] private List<GameObject> Budynki; // to jest przysz³oœæ do zapisywanie gry
    public List<BuildingsStats> buildingsStats; // a to jest lista z której mo¿na pytaæ budynki co robiæ

    public Animator animator;

    private void Awake()
    {
        Instance = this;


    }
    void Start()
    {
        Budynki = new List<GameObject>();
        cam = Camera.main;

    }

    public void StartBuilding(BuildingsScriptableObjects statsy)
    {
        if (GetComponent<GameManager>().CanPlayerMove())
        {
            if (!isBuilding)
            {
                isBuilding = true;
                StartCoroutine(WhereToBuild(statsy));
            }
            else
            {
                isBuilding = false;
            }

        }
    }
    public void Build(GameObject position, BuildingsScriptableObjects statsy)
    {
        Transform posi = position.transform;
        Destroy(position);
        GameObject building = Instantiate(statsy.Budynek, posi.position, posi.rotation);
        building.transform.SetParent(parent.transform, true);
        building.AddComponent<BuildingsStats>();
        var buldingStast = building.GetComponent<BuildingsStats>();
        buldingStast.putStats(statsy);
        Budynki.Add(building);
        buildingsStats.Add(buldingStast);
    }


    IEnumerator WhereToBuild(BuildingsScriptableObjects statsy)
    {
        halfTransparent = Instantiate(statsy.Budynek, new Vector3(0f, -10f, 0f), transform.rotation);
        animator.SetFloat("speed", 1);
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
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    halfTransparent.transform.position = new Vector3(0f, -10f, 0f);

                }
                else
                {
                    GameObject hit1 = raycastHit1.collider.gameObject;
                    Vector3 place = hit1.transform.position;
                    place.y += 0.01f;
                    halfTransparent.transform.position = place;
                }

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
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        GameObject hitObject = hit.collider.gameObject;
                        Debug.Log(hitObject.name);

                        Economy.Instance.Purchase(statsy.cost);
                        Build(hitObject, statsy);
                    }

                    isBuilding = false;

                }
                else
                {
                    //Je¿eli jest obiekt to przestañ budowaæ
                    EconomyConditions.Instance.ThereIsABuilding();
                    isBuilding = false;
                }
            }
            yield return null;
        }
        animator.SetFloat("speed", 0);
        Destroy(halfTransparent);

        yield return null;
    }


}

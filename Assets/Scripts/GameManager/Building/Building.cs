using System;
using System.Collections;
using System.Collections.Generic;
using Economy.EconomyActions;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using static GameManager;

public class Building : EconomyOperations
{
    public static Building Instance;
    Camera cam;
    [FormerlySerializedAs("mask")] public LayerMask buildingMask;
    public bool isBuilding = false;
    public GameEvent isBuildingEvent;
    public GameEvent justBuild;

    public GameObject parent;
    private GameObject halfTransparent;
    
    [SerializeField] private List<GameObject> budynki; // It stores all buildings placed by player
    public List<BuildingsStats> buildingsStats; // It stores what building does
    
    public List<GameObject> Budynks
    {
        get
        {
            return budynki;
        }
    }
    
    [Header(" ")]
    [Header("Limit Buildings")]
    [SerializeField] private int buildingLimitInTotal = 5;
    [SerializeField] private List<BuildingLimits> specificBuildingLimitList;

    [Header(" ")]
    [Header("Build Buildings")]
    public bool isColor = false;
    public GameEvent isBuildingEventTwo;
    
    [Header("Building Colors")]
    [ColorUsage(true, true)] public Color regularPlaceableColor; //green
    [ColorUsage(true, true)] public Color regularNotPlaceableColor; //gray
    
    [Header("Building Bloom")]
    [ColorUsage(true, true)] public Color placeableColor; //green
    [ColorUsage(true, true)] public Color notPlaceableColor; //gray
    
    //public Animator animator;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        budynki = new List<GameObject>();
        cam = Camera.main;
    }
    
    

    public void StartBuilding(BuildingsScriptableObjects statsy)
    {
        if (GetComponent<GameManager>().CanPlayerMove())
        {
            //Here i want to check if i didnt achieve the limit
            if (parent.transform.childCount !>= buildingLimitInTotal || IsBuildingLimitAchieved(statsy.whichBudynek))
            {
                //Tell what if limit is achieved
                EconomyConditions.Instance.BuildingLimitAchieved();
                Debug.Log("Limit is achieved");
            }
            else
            {
                if (!isBuilding)
                {
                    isBuilding = true;
                
                    isBuildingEvent.Raise();
                    isBuildingEventTwo.Raise();
                
                    StartCoroutine(WhereToBuild(statsy));
                }
                else
                {
                    isBuilding = false;
                }
            }
        }
        else
        {
            EconomyConditions.Instance.NotUrTurn();
        }
    }
    private void Build(GameObject position, BuildingsScriptableObjects statsy)
    {
        Transform posi = position.transform;
        GameObject building = Instantiate(statsy.Budynek, posi.position, Quaternion.identity);
        building.transform.SetParent(parent.transform, true);
        building.AddComponent<BuildingsStats>();
        var buldingStast = building.GetComponent<BuildingsStats>();
        buldingStast.putStats(statsy);
        buldingStast.terrainTypeThatWasThere = position;
        budynki.Add(building);
        buildingsStats.Add(buldingStast);
        justBuild.Raise();
        // Destroy(position);
        position.SetActive(false);
    }

    IEnumerator WhereToBuild(BuildingsScriptableObjects statsy)
    {
        InstantiateHalfTransparentBuilding(statsy);
        
        while (isBuilding)
        {
            MoveOrHideHalfTransparentBuilding();

            if (Input.GetMouseButtonDown(0))
            {
                CheckConditionsAndBuy(statsy);
                yield return new WaitForEndOfFrame();
                EndBuilding();
            }
            
            yield return null;
        }
        //animator.SetFloat("speed", 0);
        Destroy(halfTransparent);

        yield return null;
    }

    private void InstantiateHalfTransparentBuilding(BuildingsScriptableObjects statsy)
    {
        halfTransparent = Instantiate(statsy.Budynek, new Vector3(0f, -10f, 0f), Quaternion.identity);
        //animator.SetFloat("speed", 1);
        Renderer renderer = halfTransparent.GetComponent<Renderer>();
        Material[] materials = renderer.materials;
        renderer.shadowCastingMode = ShadowCastingMode.Off;

        foreach (Material mat in materials)
        {
            mat.SetFloat("_Dissolve", 0.5f);
        }
    }


    private void MoveOrHideHalfTransparentBuilding()
    {
        Ray ray1 = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit1;
        if (Physics.Raycast(ray1, out raycastHit1, 100, buildingMask))
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
    }

    private void CheckConditionsAndBuy(BuildingsScriptableObjects statsy)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, buildingMask))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                GameObject hitObject = hit.collider.gameObject;
                Debug.Log(hitObject.name);
                if(Purchase(statsy.resourcesCost)) Build(hitObject, statsy);
            }
        }
        else
        {
            //Je�eli jest obiekt to przesta� budowa�
            EconomyConditions.Instance.ThereIsABuilding();
        }
    }
    
    private void EndBuilding()
    {
        isBuilding = false;
        
        isBuildingEventTwo.Raise();
        isBuildingEvent.Raise();
    }

    private bool IsBuildingLimitAchieved(WhichBudynek _whichBudynek)
    {
        bool isLimitAchieved = false;
        
        //Checking if there is a limit for the building
        foreach (var bildink in specificBuildingLimitList)
        {
            if (bildink.jakiBudynek == _whichBudynek)
            {
                //Checking if limit for the building is achieved
                int iloscPostawionych = 0;

                foreach (var buildink in buildingsStats) //I'm iterating through placed buildings to count the amount of them
                {
                    if (bildink.jakiBudynek == buildink.thisBudynekIs)
                    {
                        iloscPostawionych++;
                    }
                }

                if (bildink.maxIlosc <= iloscPostawionych)
                {
                    isLimitAchieved = true;
                }
                else
                {
                    isLimitAchieved = false;
                }
            }
        }

        return isLimitAchieved;
    }

    public void RemoveBuilding(GameObject demolishedBuilding)
    {
        AddResources(demolishedBuilding.GetComponent<BuildingsStats>().ReturnResourcesSellValue());
        budynki.Remove(demolishedBuilding);
        buildingsStats.Remove(demolishedBuilding.GetComponent<BuildingsStats>());
        Destroy(demolishedBuilding);
    }
    
}

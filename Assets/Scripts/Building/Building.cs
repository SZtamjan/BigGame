using System;
using System.Collections;
using System.Collections.Generic;
using Economy.EconomyActions;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class Building : MonoBehaviour
{
    public static Building Instance;
    Camera cam;
    [FormerlySerializedAs("mask")] public LayerMask buildingMask;
    public bool isBuilding = false;
    public GameEvent isBuildingEvent;
    public GameEvent justBuild;

    [HideInInspector] public GameObject parent;
    private GameObject halfTransparent;

    [SerializeField] private List<GameObject> budynki; // It stores all buildings placed by player
    public List<BuildingController> buildingsStats; // It stores what building does
    
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

    //Components
    private GameManager _gameManager;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _gameManager = GameManager.Instance;
        budynki = new List<GameObject>();
        cam = Camera.main;
    }

    private void OnDisable()
    {
        GetComponent<BuildingInfoSendToDisplayer>().TurnOffWindow();
    }

    public void StartBuilding(BuildingsScriptableObjects statsy)
    {
        if (_gameManager.CanPlayerMove())
        {
            //Here i want to check if i didnt achieve the limit
            if (parent.transform.childCount! >= buildingLimitInTotal || IsBuildingLimitAchieved(statsy.whichBudynek))
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

                    //isBuildingEvent.Raise(); //chyba do usunięcia
                    //isBuildingEventTwo.Raise(); // to też

                    EventManager.Instance.BuldingColorChange(statsy.whichBudynek);


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
        GameObject building = Instantiate(statsy.budynekPrefab, posi.position, Quaternion.identity);
        building.transform.SetParent(parent.transform, true);
        building.AddComponent<BuildingController>();
        var buldingStast = building.GetComponent<BuildingController>();
        buldingStast.FillNewStatsToThisBuilding(statsy,0);
        buldingStast.ReturnTerrainTypeThatWasThere = position;
        budynki.Add(building);
        buildingsStats.Add(buldingStast);
        justBuild.Raise();
        // Destroy(position);
        position.SetActive(false);
    }

    IEnumerator WhereToBuild(BuildingsScriptableObjects statsy)
    {
        InstantiateHalfTransparentBuilding(statsy);
        
        //turn off component so upgrade window will NOT pop up - this is not intended and will glitch out
        if(halfTransparent != null) 
        {
            if (halfTransparent.TryGetComponent(out BuildingInfoSendToDisplayer buildingInfoSendToDisplayer))
            {
                buildingInfoSendToDisplayer.enabled = false;
            }
            else
            {
                Debug.LogError("ADD \'BuildingInfoSendToDisplayer\' SCRIPT TO THIS BUILDING PREFAB");
            }
        }
        
        while (isBuilding)
        {
            MoveOrHideHalfTransparentBuilding(statsy.whichBudynek);

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
        if (statsy.budynekPrefab != null)
        {
            halfTransparent = Instantiate(statsy.budynekPrefab, new Vector3(0f, -10f, 0f), Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("PODEPNIJ PREFAB BUDYNKU W SO BUDYNKU MOZE, CO?");
        }
        //animator.SetFloat("speed", 1);
        Renderer renderer = halfTransparent.GetComponent<Renderer>();
        Material[] materials = renderer.materials;
        renderer.shadowCastingMode = ShadowCastingMode.Off;

        foreach (Material mat in materials)
        {
            mat.SetFloat("_Dissolve", 0.5f);
        }
    }


    private void MoveOrHideHalfTransparentBuilding(WhichBudynek whichBudynek)
    {
        //Hide DisplayBuildingInfo on start building
        DisplayBuildingInfo.Instance.gameObject.SetActive(false);
        
        Ray ray1 = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit1;
        if (Physics.Raycast(ray1, out raycastHit1, 100, buildingMask))
        {
            var hitObject = raycastHit1.collider.gameObject;

            var tagCheck = ColorChange.Instance.colorRules.CheckType(whichBudynek, hitObject.tag);

            if (EventSystem.current.IsPointerOverGameObject() || !tagCheck)
            {
                HidehalfTransparent(halfTransparent);
            }
            else
            {
                Vector3 place = hitObject.transform.position;
                place.y += 0.01f;
                halfTransparent.transform.position = place;
            }

        }
        else
        {
            HidehalfTransparent(halfTransparent);

        }
    }

    private void HidehalfTransparent(GameObject halfTransparent)
    {
        halfTransparent.transform.position = new Vector3(0f, -10f, 0f);
    }

    private void CheckConditionsAndBuy(BuildingsScriptableObjects statsy)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, buildingMask))
        {
            GameObject hitObject = hit.collider.gameObject;
            var tagCheck = ColorChange.Instance.colorRules.CheckType(statsy.whichBudynek, hit.collider.gameObject.tag);

            if (!tagCheck)
            {
                EconomyConditions.Instance.HereIsNotAPlaceToBuild();
            }
            else if (!EventSystem.current.IsPointerOverGameObject() && tagCheck)
            {


                Debug.Log(hitObject.name);
                if(EconomyOperations.Purchase(statsy.buildingLevelsList[0].thisLevelCost)) Build(hitObject, statsy);
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

        //isBuildingEvent.Raise(); //chyba do usunięcia
        //isBuildingEventTwo.Raise(); // to też
        EventManager.Instance.BuldingColorChange(null);
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
        EconomyOperations.AddResources(demolishedBuilding.GetComponent<BuildingController>().ResourcesCurrentSell);
        //Place previous ground
        demolishedBuilding.GetComponent<BuildingController>().ReturnTerrainTypeThatWasThere.SetActive(true);
        budynki.Remove(demolishedBuilding);
        buildingsStats.Remove(demolishedBuilding.GetComponent<BuildingController>());
        Destroy(demolishedBuilding);
    }

}

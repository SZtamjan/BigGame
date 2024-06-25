using UnityEngine;

public class TurnOffBuildingMenuScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TurnOffBuildingMenu();
        }
    }

    public void TurnOffBuildingMenu()
    {
        UIController.Instance.BuildingCardsChangeShow(false);
    }
    
}

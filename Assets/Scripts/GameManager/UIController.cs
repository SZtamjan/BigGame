using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Mono.Cecil;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    #region Variables

    //Components
    [Header("Components")] 
    [SerializeField] private Camera _camera;
    private CardManager _cardManager;
    private PlayerMovement _playerMovement;
    
    [Header("Menus")]
    [SerializeField] private GameObject QuickMenu;
    [SerializeField] private List<GameObject> menus;

    [Header("Cards")]
    [SerializeField] private GameObject DeckCards;
    [SerializeField] private GameObject FakeCard;
    [SerializeField] private GameObject CardsToDrawViewer;
    [SerializeField] private GameObject CardsToDrawViewerScroller;
    [SerializeField] private Image CardsToDrawViewerBackground;
    private float _DeckCardsWith;
    [SerializeField] private GameObject BuildingsCards;
    [SerializeField] private bool BuildingsCardShowing = false;
    [SerializeField] private List<GameObject> DrawableCards = new List<GameObject>();

    [Header("Buttons")]
    [SerializeField] private Button NextTurnButton;
    [SerializeField] private Button buildMenuButton;
    [SerializeField] private Button deckButton;
    
    [Header("Display Resources")]
    [SerializeField] private TextMeshProUGUI showGold;
    [SerializeField] private TextMeshProUGUI showStone;
    [SerializeField] private TextMeshProUGUI showWood;
    [SerializeField] private TextMeshProUGUI showFood;
    
    [Header("Display Other")]
    [SerializeField] private TextMeshProUGUI ShowTurn;
    [SerializeField] private TextMeshProUGUI ShowEndDisplay;

    [Header("Warning settings")]
    [SerializeField] private TextMeshProUGUI ShowEconomyWarming;
    [SerializeField] private float textFullAlpha = 2f;
    [SerializeField] private float fadeDuration = 2f;
    private float _currentAlpha;
    private float _targetAlpha = 0f;

    [Header("Castles HP")]
    [SerializeField] private Slider PlayerCastle;
    [SerializeField] private Slider ComputerCastle;

    [Header("Sound Oprions")]
    [SerializeField] private Sprite isOn;
    [SerializeField] private Sprite isOff;
    [SerializeField] private Image _muteButton;

    [Header("Background Mechanics")] 
    [SerializeField] private GameObject buildingMenu;
    [SerializeField] private GameObject trashCan;

    [FormerlySerializedAs("popUpDisplayDisabledForTurns")]
    [Header("Buildings")] 
    [SerializeField] private TextMeshProUGUI cursorFloatingWindowTitle;
    [SerializeField] private TextMeshProUGUI cursorFloatingWindowDescription;
    [SerializeField] private TextMeshProUGUI cursorFloatingWindowGold;
    [SerializeField] private TextMeshProUGUI cursorFloatingWindowFood;
    [SerializeField] private TextMeshProUGUI cursorFloatingWindowWood;
    [SerializeField] private TextMeshProUGUI cursorFloatingWindowStone;

    //Coroutines
    private Coroutine warningMessage;
    
    #endregion

    #region BaseMethods
    
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //Initialize components
        _cardManager = CardManager.instance;
        _playerMovement = PlayerMovement.instance;

        BuildingsCards.SetActive(false);
        BuildingsCardShowing = false;
        QuickMenu.SetActive(false);
        ShowEconomyWarming.alpha = 0f;
        ShowEndDisplay.gameObject.SetActive(false);
        _DeckCardsWith = DeckCards.GetComponent<RectTransform>().rect.width;

        
        //SetUpCardsToDraw();
        SpawnCardsInDrawableViewer();
    }

    #endregion

    #region TurnButton 

    public void TurnButtonAction()
    {
        GameManager.Instance.PlayerTurnEnd();
    }

    public void TurnButtonDisable()
    {
        NextTurnButton.interactable = false;
    }

    public void TurnButtonActivate()
    {
        NextTurnButton.interactable = true;
    }

    #endregion

    #region Buttons

    public void SwitchLockBuildMenuButton()
    {
        if (buildMenuButton.enabled)
        {
            buildMenuButton.enabled = false;
            return;
        }

        buildMenuButton.enabled = true;
    }
    
    

    #endregion

    #region Cards

    public GameObject HereCardsAre()
    {
        return DeckCards;
    }

    public void ArrangeCards()
    {
        CardManager.instance.CardInHand.RemoveAll(item=>item==null);
        int i = 0;
        int x = CardManager.instance.CardInHand.Count()+1;
        foreach (var item in CardManager.instance.CardInHand)
        {
            i++;
            float posX = (x - i) * (_DeckCardsWith / x);
            item.GetComponent<RectTransform>().anchoredPosition = new Vector3(posX, 88f, 0);
            item.GetComponent<UnitCardMover>().NewStartPos();
        }
    }
    
    public void SwitchActiveCardsToDrawViewer()
    {
        StartCoroutine(ViewerAnimation());
        ChangeZoomLock();
    }

    private void ChangeZoomLock()
    {
        _playerMovement.GetComponent<CamZoom>().ChangeZoomLock();
    }
    
    //It teleports to -2000 to prevent deck-cards being unclickable
    private IEnumerator ViewerAnimation()
    {
        
        if (Math.Round(CardsToDrawViewerScroller.transform.localPosition.y) == -2000 && CardsToDrawViewerBackground.color.a == 0)
        {
            //In
            CardsToDrawViewerScroller.transform.DOLocalMoveY(-1000f, .0f);
            
            CardsToDrawViewerScroller.transform.DOLocalMoveY(0, .5f).SetEase(Ease.OutBack);
            CardsToDrawViewerBackground.DOFade(.8f, .5f).onPlay = () =>
            {
                CardsToDrawViewerBackground.gameObject.SetActive(true);
            };
        }
        else if (Math.Round(CardsToDrawViewerScroller.transform.localPosition.y) == 0 && CardsToDrawViewerBackground.color.a == .8f)
        {
            //Out
            Tween myTween =  CardsToDrawViewerScroller.transform.DOLocalMoveY(-1000, .5f).SetEase(Ease.InBack);
            yield return myTween.WaitForPosition(.3f);
            CardsToDrawViewerBackground.DOFade(0f, .4f).onComplete = () =>
            {
                CardsToDrawViewerBackground.gameObject.SetActive(false);
            };
            
            yield return myTween.WaitForPosition(.8f);
            myTween.Kill();
            
            CardsToDrawViewerScroller.transform.DOLocalMoveY(-2000f, .0f);
        }
    }

    private void SpawnCardsInDrawableViewer()
    {
        List<CardScriptableObject> drawableCards = _cardManager.CollectionCardsToDraw;
        foreach (var newDrawable in drawableCards)
        {
            StartCoroutine(AddCardToDrawableViewer(newDrawable));
        }
    }

    public IEnumerator RemoveCardToDrawableViewer(CardScriptableObject newDrawable)
    {
        for (int i = 1; i <= CardsToDrawViewer.transform.childCount; i++)
        {
            yield return null;
            if (CardsToDrawViewer.transform.GetChild(i).GetComponent<FakeCard>().name == newDrawable.name)
            {
                Destroy(CardsToDrawViewer.transform.GetChild(i).gameObject);
                break;
            }
        }

        yield return null;
    }

    public IEnumerator AddCardToDrawableViewer(CardScriptableObject newDrawable)
    {
        GameObject currCard = Instantiate(FakeCard, CardsToDrawViewer.transform);
        currCard.GetComponent<FakeCard>().SetUpCard(newDrawable);
        yield return new WaitForEndOfFrame();
        
        SetFakeCardInViewer(currCard);
    }

    private void SetFakeCardInViewer(GameObject currDrawable)
    {
        for (int i = 1; i <= CardsToDrawViewer.transform.childCount; i++)
        {
            if (CardsToDrawViewer.transform.GetChild(i-1).GetComponent<FakeCard>().name == 
                currDrawable.GetComponent<FakeCard>().name)
            {
                currDrawable.transform.SetSiblingIndex(i-1);
                break;
            }
        }
    }

    public void RemoveCardFromViewer(GameObject removeThis) // To apply when added building removal
    {
        foreach (GameObject card in CardsToDrawViewer.transform)
        {
            if(card.GetComponent<FakeCard>().name == removeThis.GetComponent<FakeCard>().name)
                Destroy(card);
        }
    }

    public void SwitchTrashcanActive()
    {
        if (buildingMenu.activeSelf)
        {
            buildingMenu.SetActive(false);
            trashCan.SetActive(true);
        }
        else
        {
            buildingMenu.SetActive(true);
            trashCan.SetActive(false);
        }
    }

    public void EnableCardButton(bool enable)
    {
        foreach (var item in CardManager.instance.CardInHand)
        {
            item.GetComponent<Button>().enabled = enable;
        }
    }

    #endregion

    #region BuildingCards

    public void BuildingCardsChangeShow()
    {
        if (!BuildingsCardShowing)
        {
            BuildingsCards.SetActive(true);
            BuildingsCardShowing = true;
        }
        else
        {
            BuildingsCards.SetActive(false);
            BuildingsCardShowing = false;
        }
    }
    public void BuildingCardsChangeShow(bool state)
    {
        BuildingsCards.SetActive(state);
        BuildingsCardShowing = state;

    }

    #endregion

    #region Buildings

    public void CursorFloatingWindowInfoDisplay(ResourcesStruct res, int newText, string titleText)
    {
        cursorFloatingWindowTitle.text = titleText;
        cursorFloatingWindowDescription.text = PluralChecker(newText);
        
        cursorFloatingWindowGold.text = res.Gold.ToString();
        cursorFloatingWindowFood.text = res.Food.ToString();
        cursorFloatingWindowWood.text = res.Wood.ToString();
        cursorFloatingWindowStone.text = res.Stone.ToString();
    }
    
    public void CursorFloatingWindowInfoDisplay(ResourcesStruct res, string titleText)
    {
        cursorFloatingWindowTitle.text = titleText;
        cursorFloatingWindowDescription.text = "Resources that you will get";
        
        cursorFloatingWindowGold.text = res.Gold.ToString();
        cursorFloatingWindowFood.text = res.Food.ToString();
        cursorFloatingWindowWood.text = res.Wood.ToString();
        cursorFloatingWindowStone.text = res.Stone.ToString();
    }

    private string PluralChecker(int newText)
    {
        newText++;
        if (newText == 1)
        {
            return "Upgrade time: " + newText + " turn";
        }

        return "Upgrade time: " + newText + " turns";
    }

    #endregion

    #region CastleHp
    public void CastleHpSetMaxHealth(int health, bool playerCastle)
    {
        if (playerCastle)
        {
            PlayerCastle.maxValue = health;
            PlayerCastle.value = health;
        }
        else
        {
            ComputerCastle.maxValue = health;
            ComputerCastle.value = health;
        }
    }

    public void CastleHpSetHealth(int health, bool playerCastle)
    {
        if (playerCastle)
        {
            PlayerCastle.value = health;
        }
        else
        {
            ComputerCastle.value = health;
        }

    }


    #endregion

    #region Economy

    public void EconomyUpdateResources(ResourcesStruct resources)
    {
        showGold.text = resources.Gold.ToString();
        showStone.text = resources.Stone.ToString();
        showWood.text = resources.Wood.ToString();
        showFood.text = resources.Food.ToString();
    }

    #endregion

    #region QuickMenu
    public void QuickMenuChangeVis()
    {
        if (QuickMenu.gameObject.activeSelf)
        {
            QuickMenu.SetActive(false);
            menus[0].SetActive(true);
            menus[1].SetActive(false);
            menus[2].SetActive(false);
        }
        else
        {
            QuickMenu.SetActive(true);
            menus[0].SetActive(true);
            menus[1].SetActive(false);
            menus[2].SetActive(false);
        }

    }

    public void QuickMenuHideSettings()
    {
        menus[0].SetActive(true);
        menus[1].SetActive(false);
    }

    public void QuickMenuExitConfirm()
    {
        menus[2].SetActive(true);
    }
    public void QuickMenuExitExitConfirm()
    {
        menus[2].SetActive(false);
    }

    public void QuickMenuShowSettings()
    {
        menus[0].SetActive(false);
        menus[1].SetActive(true);
    }

    #endregion

    #region Warming
    public void WarmingShowWarming(string message)
    {
        ShowEconomyWarming.text = message;
        if (warningMessage == null)
        {
            warningMessage = StartCoroutine(WarningLenght());
        }
        else
        {
            StopCoroutine(warningMessage);
            warningMessage = null;
            
            warningMessage = StartCoroutine(WarningLenght());
        }
    }

    IEnumerator WarningLenght()
    {
        float elapsedTime = 0f;
        
        Vector2 movePos;
        Canvas parentCanvas = ShowEconomyWarming.transform.parent.GetComponent<Canvas>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            Input.mousePosition, parentCanvas.worldCamera,
            out movePos);

        ShowEconomyWarming.transform.position = parentCanvas.transform.TransformPoint(movePos);
        
        ShowEconomyWarming.alpha = 1;
        _currentAlpha = ShowEconomyWarming.alpha;

        yield return new WaitForSeconds(textFullAlpha);

        while (elapsedTime < fadeDuration)
        {
            float newAlpha = Mathf.Lerp(_currentAlpha, _targetAlpha, elapsedTime / fadeDuration);

            ShowEconomyWarming.alpha = newAlpha;
            yield return null;

            elapsedTime += Time.deltaTime;
        }

        warningMessage = null;
    }
    #endregion

    #region ShowTurn
    public void ShowTurnChangeNumber(string number)
    {
        ShowTurn.text = $"TURN \n{number}";
    }

    #endregion

    #region ShowEndDisplay

    public void ShowEndDisplayActivate(string message, bool winner)
    {
        ShowEndDisplay.gameObject.SetActive(true);
        ShowEndDisplay.text = message;
        if (winner)
        {
            SceneChange.instance.LoadOnClick();
        }
    }

    #endregion

    #region Mute

    public void MuteSwithMute()
    {
        if (_muteButton.sprite == isOn)
        {
            _muteButton.sprite = isOff;
            Mute();
            Debug.Log("Muted");
        }
        else
        {
            _muteButton.sprite = isOn;
            Unmute();
            Debug.Log("Unmuted");
        }
    }

    private void Mute()
    {

        AudioManager.instance.myMixer.SetFloat("masterMixer", -80);
    }

    private void Unmute()
    {
        AudioManager.instance.myMixer.SetFloat("masterMixer", PlayerPrefs.GetFloat("masterAudio"));
    }




    #endregion

    #region SaveSystem

    public void SaveCurrentLevel()
    {
        GameManager.Instance.SaveProgress();
    }

    public void ReloadScene()
    {
        GameManager.Instance.LoadProgress();
    }

    #endregion

}
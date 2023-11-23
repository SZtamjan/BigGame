using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [Header("Menus")]
    [SerializeField] private GameObject QuickMenu;
    [SerializeField] private List<GameObject> menus;

    [Header("Cards")]
    [SerializeField] private GameObject DeckCards;
    private float _DeckCardsWith;
    [SerializeField] private GameObject BuildingsCards;
    [SerializeField] private bool BuildingsCardShowing = false;

    [Header("Buttons")]
    [SerializeField] private Button NextTurnButton;

    [Header("Showing Somethings")]
    [SerializeField] private TextMeshProUGUI ShowFunds;
    [SerializeField] private TextMeshProUGUI ShowTurn;
    [SerializeField] private TextMeshProUGUI ShowEndDisplay;


    [Header("Warming settings")]
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


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        BuildingsCards.SetActive(false);
        BuildingsCardShowing = false;
        QuickMenu.SetActive(false);
        ShowEconomyWarming.alpha = 0f;
        ShowEndDisplay.gameObject.SetActive(false);
        _DeckCardsWith = DeckCards.GetComponent<RectTransform>().rect.width;
    }


    #region TurnButton 

    public void TurnButtonAction()
    {
        GameManager.instance.PlayerTurnEnd();
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
            item.GetComponent<RectTransform>().anchoredPosition = new Vector3(posX, 0, 0);
            item.GetComponent<SpawnUnitCard>().NewStartPos();
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

    public void EconomyUpdateCash(int cash)
    {
        ShowFunds.text = cash.ToString();
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
        StartCoroutine(WarningLenght());
    }

    IEnumerator WarningLenght()
    {
        float elapsedTime = 0f;
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
    }
    #endregion

    #region ShowTurn
    public void ShowTurnChangeNumber(string number)
    {
        ShowTurn.text = $"TURN: {number}";
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
        GameManager.instance.SaveProgress();
    }

    public void ReloadScene()
    {
        GameManager.instance.LoadProgress();
    }

    #endregion

}

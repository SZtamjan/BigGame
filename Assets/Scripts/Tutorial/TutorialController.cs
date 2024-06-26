﻿using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance;
    
    [Header("Ask for Tutorial - Turn On/Off Tutorial")]
    [SerializeField] private bool isTutorial;

    [ShowIf("isTutorial")] [Required("Tutorial Question Object CANNOT BE NULL")] [SerializeField] private GameObject tutorialQuestionObject;

    private bool _interactionContinue = false; 
    private bool _dialogContinue = false;
    
    [Header("Dialog setup")]
    [SerializeField] private List<TutorialSection> dialog;

    [Header("Settings")] 
    [SerializeField] private List<GameObject> turnOffForTutorial;

    private int _currentIteration;

    private Coroutine _interactionStepCor;
    private Coroutine _dialogStepCor;
    
    #region Properties

    public bool IsTutorial
    {
        get => isTutorial;
    }

    public bool AllowBuildingBypass
    {
        get => dialog[_currentIteration].allowBuilding;
    }

    public bool AllowUnitBypass
    {
        get => dialog[_currentIteration].allowUnits;
    }

    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            NextDialogFragment();
        }
    }

    public void AskForTutorial()
    {
        if(LackOfTutQuestionObject()) return;
        tutorialQuestionObject.SetActive(true);
    }

    public void SkipTutorial()
    {
        if(LackOfTutQuestionObject()) return;
        GameManager.Instance.UpdateGameState(GameManager.GameState.PlayerTurn);
        tutorialQuestionObject.SetActive(false);
    }

    public void RunTutorial()
    {
        if(LackOfTutQuestionObject()) return;
        tutorialQuestionObject.SetActive(false);
        StartCoroutine(GoThroughTutorial());
    }

    #region Dialog

    public IEnumerator GoThroughTutorial()
    {
        ChangeObjectsForTutPeriod(false);
        
        for (int i = 0; i < dialog.Count; i++)
        {
            _currentIteration = i;
                
            if (dialog[i].uiBackground != null)
            {
                dialog[i].uiBackground.SetActive(true);
            }

            if (dialog[i].zbikArt != null)
            {
                dialog[i].uiElementForZbik.gameObject.SetActive(true);
                dialog[i].uiElementForZbik.sprite = dialog[i].zbikArt;
            }
            
            for (int j = 0; j < dialog[i].dialogFragment.Count; j++)
            {
                if (dialog[i].displayDialogOnObject != null)
                {
                    dialog[i].displayDialogOnObject.gameObject.SetActive(true);
                    dialog[i].displayDialogOnObject.text = dialog[i].dialogFragment[j] + "\n" + dialog[i].additionalInfo;
                }

                
                if (j < dialog[i].dialogFragment.Count - 1)
                {
                    Debug.Log("Waiting for dialog to continue");
                    yield return new WaitUntil(() => _dialogContinue);
                }
                _dialogContinue = false;
            }

            if (dialog[i].uiBackground != null)
            {
                if (dialog[i].waitForDialog)
                {
                    Debug.Log("Waiting for dialog to continue");
                    yield return new WaitUntil(() => _dialogContinue);
                    _dialogContinue = false;
                }
                else
                {
                    Debug.Log("Waiting for interaction to happen");
                    yield return new WaitUntil(() => _interactionContinue);
                    _interactionContinue = false;
                }
                
                dialog[i].uiBackground.SetActive(false);
            }
            else
            {
                Debug.Log("Waiting for last dialog in fragment to continue");
                yield return new WaitUntil(() => _dialogContinue);
                _dialogContinue = false;
            }

            if (i+1 < dialog.Count)
            {
                if (dialog[i + 1].zbikArt != null)
                {
                    dialog[i].uiElementForZbik.sprite = null;
                    dialog[i].uiElementForZbik.gameObject.SetActive(false);
                }
            
                if (dialog[i+1].displayDialogOnObject != null)
                {
                    dialog[i].displayDialogOnObject.text = "";
                    dialog[i].displayDialogOnObject.gameObject.SetActive(false);
                }
            }
            else if (i + 1 == dialog.Count)
            {
                dialog[i].uiElementForZbik.sprite = null;
                dialog[i].displayDialogOnObject.text = "";
                
                dialog[i].displayDialogOnObject.gameObject.SetActive(false);
                dialog[i].uiElementForZbik.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("TO NIE POWINNO SIE STAC");
            }
        }

        ChangeObjectsForTutPeriod(true);
        GameManager.Instance.UpdateGameState(GameManager.GameState.PlayerTurn);
    }

    [Button]
    public void NextDialogFragment()
    { 
        if (_dialogStepCor == null) _dialogStepCor = StartCoroutine(TmpAllowNextDialog());
    }

    private IEnumerator TmpAllowNextDialog()
    {
        Debug.Log("Dialog fragment skip");
        _dialogContinue = true;
        yield return new WaitForSeconds(.1f);
        _dialogContinue = false;

        _dialogStepCor = null;
    }
    
    [Button]
    public void TutorialInteraction()
    {
        if(_interactionStepCor == null) _interactionStepCor = StartCoroutine(TmpAllowInteractionContinue());
    }

    private IEnumerator TmpAllowInteractionContinue()
    {
        Debug.Log("Interaction skip");
        _interactionContinue = true;
        yield return new WaitForSeconds(.1f);
        _interactionContinue = false;

        _interactionStepCor = null;
    }
    
    #endregion

    private bool LackOfTutQuestionObject()
    {
        if (tutorialQuestionObject == null)
        {
            Debug.LogError("BRAK PRZYPISANEGO OBIEKTU TutorialQuestion w TutorialController - obiekt powinien byc jako dziecko obiektu UI");
            return true;
        }

        return false;
    }

    private void ChangeObjectsForTutPeriod(bool isOn)
    {
        foreach (var objs in turnOffForTutorial)
        {
            objs.SetActive(isOn);
        }
    }
    
}

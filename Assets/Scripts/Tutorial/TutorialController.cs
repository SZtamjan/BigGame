using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance;
    
    [Header("Ask for Tutorial - Turn On/Off Tutorial")]
    [SerializeField] private bool isTutorial;

    private bool _interactionContinue = false; 
    private bool _dialogContinue = false;
    
    [SerializeField] private List<TutorialSection> dialog;

    private Coroutine _interactionStepCor;
    private Coroutine _dialogStepCor;
    
    #region Properties

    public bool IsTutorial
    {
        get => isTutorial;
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

    public IEnumerator GoThroughTutorial()
    {
        foreach (TutorialSection tutorialSection in dialog)
        {
            if (tutorialSection.uiBackground != null)
            {
                tutorialSection.uiBackground.SetActive(true);
            }

            for (int i = 0; i < tutorialSection.dialogFragment.Count; i++)
            {
                if (tutorialSection.displayDialogOnObject == null)
                {
                    Debug.LogError("Brak uzupelnionego \'displayDialogOnObject\' w liscie tutorial");
                    yield break;
                }

                tutorialSection.displayDialogOnObject.text = tutorialSection.dialogFragment[i];
                Debug.Log("Waiting for dialog to continue");
                if (i != tutorialSection.dialogFragment.Count - 1) yield return new WaitUntil(() => _dialogContinue);
                _dialogContinue = false;
            }

            if (tutorialSection.uiBackground != null)
            {
                Debug.Log("Waiting for interaction to happen");
                yield return new WaitUntil(() => _interactionContinue);
                _interactionContinue = false;
            
                tutorialSection.uiBackground.SetActive(false);
            }
        }

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
}

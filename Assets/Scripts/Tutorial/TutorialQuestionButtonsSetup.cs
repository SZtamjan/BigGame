using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialQuestionButtonsSetup : MonoBehaviour
{
    public void SkipTutorial()
    {
        TutorialController.Instance.SkipTutorial();
    }

    public void RunTutorial()
    {
        TutorialController.Instance.RunTutorial();
    }
}

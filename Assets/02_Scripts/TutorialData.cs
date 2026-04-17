using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TutorialData
{
    public Sprite tutorialImage;
    [TextArea(3, 5)]
    public string tutorialText;

    public bool showHand;
}

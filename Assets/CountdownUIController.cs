using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownUIController : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI countdownText = null;

    void Start() {
        LevelController.UpdateCountdown += ChangeCountdownText;    
    }

    void ChangeCountdownText(string text) {
        countdownText.text = text;
    }
}

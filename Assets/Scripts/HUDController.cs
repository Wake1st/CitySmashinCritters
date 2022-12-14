using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField]
    private Slider healthBarAmount = null;
    [SerializeField]
    private string timeTextMessage = null;
    [SerializeField]
    private TMPro.TextMeshProUGUI timeText = null;
    [SerializeField]
    private float sliderFillSpeed = 0.5f;
    private float timeTillFill = 0;
    
    private float currentHealthValue;
    private float newHealthValue = 100;
    private float timeInLevel;

    void Start()
    {
        CharacterHealthController.UpdateHealth += ChangeValue;
    }

    void Update()
    {
        AddTime();
        NewBarValue();
    }

    private void ChangeValue(int amount) {
        newHealthValue = amount;
        timeTillFill = 0;
    }

    private void AddTime() {
        if (currentHealthValue > 0) {
            timeInLevel += Time.deltaTime;
            timeText.text = timeTextMessage 
                + Math.Round(timeInLevel, 2).ToString();
        }
    }

    private void NewBarValue() {
        if (currentHealthValue != newHealthValue) {
            currentHealthValue = Mathf.Lerp(
                currentHealthValue, 
                newHealthValue, 
                timeTillFill
            );
            timeTillFill += sliderFillSpeed * Time.deltaTime;
        }

        healthBarAmount.value = currentHealthValue;
    }
}

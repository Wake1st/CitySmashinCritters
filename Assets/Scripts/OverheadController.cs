using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverheadController : MonoBehaviour
{
    [SerializeField]
    private Slider healthBarAmount = null;
    [SerializeField]
    private float sliderFillSpeed = 0.5f;
    [SerializeField]
    private Canvas canvas;

    private float timeTillFill = 0;
    private float currentHealthValue;
    private float newHealthValue = 50;

    void Start()
    {
        currentHealthValue = newHealthValue;
    }

    void Update()
    {
        NewBarValue();
    }

    void LateUpdate() {
        LookAtPlayer(); //  TODO: maybe just an event call - doesn't need constant update
    }

    private void LookAtPlayer() {
        canvas.transform.forward = -Camera.main.transform.forward;
    }

    public void ChangeValue(int amount) {
        newHealthValue = amount;
        timeTillFill = 0;
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

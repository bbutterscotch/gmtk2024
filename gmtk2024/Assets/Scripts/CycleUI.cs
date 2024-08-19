using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CycleUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text cycleText;
    CycleController cc;

    // Start is called before the first frame update
    void Start()
    {
        cc = FindObjectOfType<CycleController>();
        slider.minValue = 0;
        slider.maxValue = cc.cycleLength;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = cc.currentTime;
        cycleText.text = "Cycle " + cc.currentCycle.ToString();
    }
}

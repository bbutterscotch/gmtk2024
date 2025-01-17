using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CycleUI : MonoBehaviour
{
    [SerializeField] private TMP_Text cycleText;
    [SerializeField] private GameObject sliderFull;
    private Image sliderImage;
    private CycleController cc;

    // Start is called before the first frame update
    void Start()
    {
        cc = FindObjectOfType<CycleController>();
        sliderImage = sliderFull.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        cycleText.text = "Cycle " + cc.currentCycle.ToString();
        sliderImage.fillAmount = cc.currentTime / cc.cycleLength;
    }
}

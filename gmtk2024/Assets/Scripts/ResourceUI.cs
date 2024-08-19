using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nectarText;
    [SerializeField] private TMP_Text pollenText;
    [SerializeField] private TMP_Text honeyText;
    [SerializeField] private TMP_Text waxText;
    [SerializeField] private TMP_Text royalJellyText;
    [SerializeField] private TMP_Text beeCountText;
    private HiveResources hv;


    // Start is called before the first frame update
    void Start()
    {
        hv = FindObjectOfType<HiveResources>();
    }

    // Update is called once per frame
    void Update()
    {
        nectarText.text = hv.nectar.ToString();
        pollenText.text = hv.pollen.ToString();
        honeyText.text = hv.honey.ToString();
        waxText.text = hv.wax.ToString();
        royalJellyText.text = hv.royalJelly.ToString();
        beeCountText.text = hv.bees.ToString();
    }
}

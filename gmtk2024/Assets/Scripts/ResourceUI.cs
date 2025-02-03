using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nectarText;
    [SerializeField] private TMP_Text pollenText;
    [SerializeField] private TMP_Text honeyText;
    [SerializeField] private TMP_Text waxText;
    [SerializeField] private TMP_Text royalJellyText;
    [SerializeField] private TMP_Text beeCountText;

    [SerializeField] private GameObject[] hover;
    private HiveResources hv;

    [SerializeField] private EventReference hoverSound;


    // Start is called before the first frame update
    void Start()
    {
        hv = FindFirstObjectByType<HiveResources>();
        foreach (GameObject go in hover)
        {
            go.SetActive(false);
        }
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

    public void OnHoverButton(GameObject hover)
    {
        hover.SetActive(true);
        AudioController.instance.PlayOneShot(hoverSound, this.transform.position);
    }

    public void OnLeaveHoverButton(GameObject hover)
    {
        hover.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class OpenCloseUI : MonoBehaviour
{
    [Header("Resources")]
    public GameObject resourcesPanel;
    public GameObject resourcePanelClosed;
    public GameObject resourceAmounts;
    public GameObject resourceAmountsClosed;
    public GameObject resourceAmountsOpen;
    public float transformAmount = 0.2f;
    //public int resourceOpenY;
    //public int resourceClosedY;
    public bool resourcePanelOpen;

    [Header("Book")]
    public GameObject page0;
    public GameObject page1;
    public GameObject page2;
    public int page = 0;

    [SerializeField] private EventReference openPanelSound;
    [SerializeField] private EventReference closePanelSound;
    [SerializeField] private EventReference openBookSound;
    [SerializeField] private EventReference closeBookSound;

    // Start is called before the first frame update
    void Start()
    {
        //resourcesPanel.transform.position = new Vector3(resourcesPanel.transform.position.x, resourceClosedY, 0);
        resourcesPanel.SetActive(false);
        resourcePanelClosed.SetActive(true);
        page = 0;
        page0.SetActive(true);
        page1.SetActive(false);
        page2.SetActive(false);
    }

    public void ToggleResourcePanel() 
    {
        if (resourcePanelOpen) {
            //resourcePanelOpen = false;
            //resourcesPanel.transform.position = new Vector3(resourcesPanel.transform.position.x, resourceClosedY, 0);
            CloseResourcePanel();
        } else {
            //resourcePanelOpen = true;
            //resourcesPanel.transform.position = new Vector3(resourcesPanel.transform.position.x, resourceOpenY, 0);
            OpenResourcePanel();
        }
    }

    public void OpenResourcePanel() {
        AudioController.instance.PlayOneShot(openPanelSound, this.transform.position);
        resourcePanelOpen = true;
        resourcesPanel.SetActive(true);
        resourcePanelClosed.SetActive(false);
        int numChildren = resourceAmountsClosed.transform.childCount;
        for (int i = 0; i < numChildren; i++)
        {
            resourceAmountsClosed.transform.GetChild(0).SetParent(resourceAmountsOpen.transform);
        }
    }

    public void CloseResourcePanel() {
        AudioController.instance.PlayOneShot(closePanelSound, this.transform.position);
        resourcePanelOpen = false;
        resourcesPanel.SetActive(false);
        resourcePanelClosed.SetActive(true);
        int numChildren = resourceAmountsOpen.transform.childCount;
        for (int i = 0; i < numChildren; i++)
        {
            resourceAmountsOpen.transform.GetChild(0).SetParent(resourceAmountsClosed.transform);
        }
    }

    public void BookClick()
    {
        if (page == 0) {
            AudioController.instance.PlayOneShot(openBookSound, this.transform.position);
            page = 1;
            page0.SetActive(false);
            page1.SetActive(true);
            page2.SetActive(false);
        } else if (page == 1) {
            AudioController.instance.PlayOneShot(closeBookSound, this.transform.position);
            page = 2;
            page0.SetActive(false);
            page1.SetActive(false);
            page2.SetActive(true);
        } else if (page == 2) {
            AudioController.instance.PlayOneShot(closeBookSound, this.transform.position);
            page = 0;
            page0.SetActive(true);
            page1.SetActive(false);
            page2.SetActive(false);
        }
    }
}

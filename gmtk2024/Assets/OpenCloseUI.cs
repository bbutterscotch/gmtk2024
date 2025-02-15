using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCloseUI : MonoBehaviour
{
    [Header("Resources")]
    public GameObject resourcesPanel;
    public GameObject resourcePanelClosed;
    //public int resourceOpenY;
    //public int resourceClosedY;
    public bool resourcePanelOpen;

    [Header("Book")]
    public GameObject page0;
    public GameObject page1;
    public GameObject page2;
    public int page = 0;

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
        resourcePanelOpen = true;
        resourcesPanel.SetActive(true);
        resourcePanelClosed.SetActive(false);
    }

    public void CloseResourcePanel() {
        resourcePanelOpen = false;
        resourcesPanel.SetActive(false);
        resourcePanelClosed.SetActive(true);
    }

    public void BookClick()
    {
        if (page == 0) {
            page = 1;
            page0.SetActive(false);
            page1.SetActive(true);
            page2.SetActive(false);
        } else if (page == 1) {
            page = 2;
            page0.SetActive(false);
            page1.SetActive(false);
            page2.SetActive(true);
        } else if (page == 2) {
            page = 0;
            page0.SetActive(true);
            page1.SetActive(false);
            page2.SetActive(false);
        }
    }
}

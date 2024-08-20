using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class buttonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject info;
    public CellClick cellClick;

    void Start()
    {
        info.SetActive(false);
    }

    void Update()
    {
        if (cellClick.isPlacing) {
            info.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!cellClick.isPlacing) {
            info.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!cellClick.isPlacing) {
            info.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OutlineSelection : MonoBehaviour
{

    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;
    public Slider scaleSlider;
    [SerializeField] private float maxScale = 2.0f;
    [SerializeField] private float minScale = 0.2f;

    void Update()
    {
        // Highlight
        if (highlight != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
            highlight = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit)) //Make sure you have EventSystem in the hierarchy before using EventSystem
        {
            highlight = raycastHit.transform;
            if (highlight.CompareTag("Selectable") && highlight != selection)
            {
                if (highlight.gameObject.GetComponent<Outline>() != null)
                {
                    highlight.gameObject.GetComponent<Outline>().enabled = true;
                }
                else
                {
                    Outline outline = highlight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                    highlight.gameObject.GetComponent<Outline>().OutlineColor = Color.magenta;
                    highlight.gameObject.GetComponent<Outline>().OutlineWidth = 7.0f;
                }
            }
            else
            {
                highlight = null;
            }
        }

        // Selection
        if (Input.GetMouseButtonDown(0))
        {
            if (highlight)
            {
                if (selection != null)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                }
                selection = raycastHit.transform;
                selection.gameObject.GetComponent<Outline>().enabled = true;
                highlight = null;
                scaleSlider.minValue = selection.GetComponent<Scalable>().Scale * minScale;
                scaleSlider.maxValue = selection.GetComponent<Scalable>().Scale * maxScale;
                scaleSlider.value = selection.localScale.x;
            }
            //else
            //{
            //    if (selection)
            //    {
            //        selection.gameObject.GetComponent<Outline>().enabled = false;
            //        selection = null;
            //    }
            //}
        }

        if (selection != null)
        {
            selection.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
        }
    }
}

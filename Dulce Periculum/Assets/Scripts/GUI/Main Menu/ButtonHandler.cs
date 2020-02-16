using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    [Range(0, 1)]
    public  float  MAX_ALPHA;
    [Range(0, 1)]
    public  float  MIN_ALPHA;
    [Range(0, 10)]
    public  float  ALPHA_CHANGING_SPEED;

    private Button button;
    private bool   mouseOver = false;

    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        Color color = button.image.color;

        if (mouseOver)
        {
            if (color.a < MAX_ALPHA)
            {
                color.a            = Mathf.Lerp(color.a, MAX_ALPHA, Time.deltaTime * ALPHA_CHANGING_SPEED);
                button.image.color = color;
            }
        }
        else
        {
            if (color.a > MIN_ALPHA)
            {
                color.a            = Mathf.Lerp(color.a, MIN_ALPHA, Time.deltaTime * ALPHA_CHANGING_SPEED);
                button.image.color = color;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }
}

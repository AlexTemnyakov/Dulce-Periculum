using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButtonHandler : ButtonHandler
{

    void Update()
    {
        Color color = button.image.color;

        if (mouseOver)
        {
            if (color.a < MAX_ALPHA)
            {
                color.a            = Mathf.Lerp(color.a, MAX_ALPHA, ALPHA_CHANGING_SPEED);
                button.image.color = color;
            }
        }
        else
        {
            if (color.a > MIN_ALPHA)
            {
                color.a            = Mathf.Lerp(color.a, MIN_ALPHA, ALPHA_CHANGING_SPEED);
                button.image.color = color;
            }
        }
    }
}

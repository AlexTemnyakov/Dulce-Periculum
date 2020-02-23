using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroductionVideo : MonoBehaviour
{
    private GameObject   slide;
    private GameObject[] texts;
    private float        showTime    = 5f;
    private bool         waiting     = false;
    private int          currentText = -1;

    void Start()
    {
        slide = transform.GetChild(0).gameObject;
        texts = new GameObject[slide.transform.childCount];
        for (int i = 0; i < texts.Length; i++)
            texts[i] = slide.transform.GetChild(i).gameObject;
        slide.SetActive(true);
    }

    void Update()
    {
        if (waiting)
            return;

        currentText++;

        if (currentText >= texts.Length)
        {
            StartCoroutine(HidePanel());
        }
        else
        {
            StartCoroutine(ShowText());
        }
    }

    private IEnumerator ShowText()
    {
        waiting = true;

        GameObject textObj = texts[currentText];
        Text       text    = textObj.GetComponent<Text>();
        Color      tmp     = text.color;
        float      time;

        tmp.a      = 0;
        text.color = tmp;

        textObj.SetActive(true);

        time = 0;
        while (time < 1)
        {
            tmp = text.color;

            tmp.a      = Mathf.Lerp(0, 1, time);
            text.color = tmp;

            time += Time.deltaTime / showTime;

            yield return new WaitForEndOfFrame(); 
        }

        yield return new WaitForSeconds(showTime / 2);

        time = 0;
        while (time < 1)
        {
            tmp = text.color;

            tmp.a      = Mathf.Lerp(1, 0, time);
            text.color = tmp;

            time += Time.deltaTime / showTime;

            yield return null;
        }

        textObj.SetActive(false);

        waiting = false;
    }

    private IEnumerator HidePanel()
    {
        waiting = true;

        Image img = slide.GetComponent<Image>();
        Color tmp = img.color;
        float time;

        time = 0;
        while (time < 1)
        {
            tmp = img.color;

            tmp.a     = Mathf.Lerp(1, 0, time);
            img.color = tmp;

            time += Time.deltaTime / showTime;

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);

        waiting = false;
    }
}

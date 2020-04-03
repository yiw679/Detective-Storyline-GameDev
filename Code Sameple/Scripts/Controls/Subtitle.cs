using System.Collections;
using UnityEngine;
using TMPro;


public class Subtitle : MonoBehaviour
{
    public int fadeSpeed = 3;

    public GameObject textObj;

    public static Subtitle instance;

    private TextMeshProUGUI tmp;
    private CanvasGroup tmp_cg;
    private float m_Timer;
    private bool isShowingText = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        tmp = textObj.GetComponent<TextMeshProUGUI>();
        tmp_cg = textObj.GetComponent<CanvasGroup>();
        textObj.SetActive(false);
    }

    public void Show(string text, float duration)
    {
        if (!isShowingText)
        {
            tmp.SetText(text);
            StartCoroutine(FadeText(duration));
        }
    }

    IEnumerator FadeText(float duration)
    {
        float startTime = Time.time;
        float complete_percentage = 0;

        textObj.SetActive(true);
        isShowingText = true;
        while (complete_percentage < 1)
        {
            complete_percentage = fadeSpeed * (Time.time - startTime);
            tmp_cg.alpha = Mathf.Lerp(0, 1, complete_percentage);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(duration);

        startTime = Time.time;
        complete_percentage = 0;
        while (complete_percentage < 1)
        {
            complete_percentage = fadeSpeed * (Time.time - startTime);
            tmp_cg.alpha = Mathf.Lerp(1, 0, complete_percentage);
            yield return new WaitForEndOfFrame();
        }
        isShowingText = false;
        textObj.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject TextContaioner;
    public GameObject TextPrefab;

    private List<FloatingText> floatingTexts = new List<FloatingText>();
    private void Update()
    {
        foreach (FloatingText text in floatingTexts)
        {
            text.UpdateFloatingText();
        }
    }

    public void Show(string msg, int fontsize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        FloatingText floatingText = GetFloatingText();

        floatingText.txt.text = msg;
        floatingText.txt.fontSize = fontsize;
        floatingText.txt.color = color;
        floatingText.Go.transform.position = Camera.main.WorldToScreenPoint(position); // transerfor world space(game) to screen space(ui)
        floatingText.Motion = motion;
        floatingText.Duration = duration;

        floatingText.Show();
    }

    private FloatingText GetFloatingText()
    {
        FloatingText txt = floatingTexts.Find(k => !k.Active);

        if(txt == null)
        {
            txt = new FloatingText();
            txt.Go = Instantiate(TextPrefab);
            txt.Go.transform.SetParent(TextContaioner.transform);
            txt.txt = txt.Go.GetComponent<Text>();

            floatingTexts.Add(txt);
        }
        return txt;
    }
}

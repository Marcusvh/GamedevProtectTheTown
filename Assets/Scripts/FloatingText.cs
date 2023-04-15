using UnityEngine;
using UnityEngine.UI;

public class FloatingText
{
    public bool Active;
    public GameObject Go;
    public Text txt;
    public Vector3 Motion;
    public float Duration;
    public float LastShown;

    public void Show()
    {
        Active = true;
        LastShown = Time.time;
        Go.SetActive(Active);
    }
    public void Hide()
    {
        Active = false;
        Go.SetActive(Active);
    }

    public void UpdateFloatingText()
    {
        if (!Active) return;
        if (Time.time - LastShown > Duration) Hide();

        Go.transform.position += Motion * Time.deltaTime;
    }
}

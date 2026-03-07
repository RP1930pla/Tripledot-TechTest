using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(RectTransform))]
public class NotchDetector : MonoBehaviour
{
    public RectTransform rectTransform;
    public Vector2 posWithoutNotch = new Vector2(0, -224);
    public Vector2 posWithNotch = new Vector2(0, -458);
    private void Start()
    {
        rectTransform = GetComponent<RectTransform> ();
        float aspect = (Screen.currentResolution.height / Screen.safeArea.height);
        if (aspect > 1.03f)
        {
            rectTransform.anchoredPosition = posWithNotch;
        }
        else 
        {
            rectTransform.anchoredPosition = posWithoutNotch;
        }
    }
}

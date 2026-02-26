using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(RectTransform))]
public class NotchDetector : MonoBehaviour
{
    RectTransform rectTransform;
    public Vector2 posWithoutNotch = new Vector2(0, -224);
    public Vector2 posWithNotch = new Vector2(0, -458);
    private void OnAwake()
    {
        rectTransform = GetComponent<RectTransform> ();
        if(Screen.currentResolution.height > Screen.safeArea.height)
        {
            rectTransform.anchoredPosition = posWithNotch;
        }
        else 
        {
            rectTransform.anchoredPosition = posWithoutNotch;
        }
    }
}

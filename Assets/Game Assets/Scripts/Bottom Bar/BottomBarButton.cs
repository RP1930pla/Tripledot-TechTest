using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;

[RequireComponent(typeof(RectTransform))]
public class BottomBarButton : MonoBehaviour
{
    [HideInInspector]
    public bool buttonEnabled = false;
    public bool locked = false;

    public BottomBarView buttonController;
    public RectTransform rectTransform;
    public RectTransform childRectTransform;
    public AnimationCurve fadeCurve;



    public Image image;

    public float rotation = 20f;
    public float timePerRotation = 0.1f;
    public AnimationCurve rotationCurve = AnimationCurve.Linear(0, 0, 1, 1);


    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        childRectTransform.GetComponentInChildren<RectTransform>();
        image.GetComponent<Image>();

        if (buttonController != null)
        {
            buttonController.Subscribe(this);
        }
    }

    private void OnDisable()
    {
        if (buttonController != null)
        {
            buttonController.Unsubscribe(this);
        }
    }

    public void SwapStates() 
    {
        if (buttonEnabled)
        {
            CloseButton();
        }
        else 
        {
            OpenButton();
        }
    }

    public void RotateLocked()
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DORotate(new Vector3(0, 0, rotation), timePerRotation).SetEase(rotationCurve));
        sequence.Append(rectTransform.DORotate(new Vector3(0, 0, -rotation), timePerRotation).SetEase(rotationCurve));
        sequence.Append(rectTransform.DORotate(new Vector3(0, 0, 0), timePerRotation).SetEase(rotationCurve));
        sequence.Play();
    }

    public void CloseButton() 
    {
        buttonEnabled = false;
        rectTransform.DOSizeDelta(new Vector2(0f, 667.457f), 0.3f);
        childRectTransform.DOLocalMoveY(0f, 0.3f);
        image.DOFade(0, 0.3f).SetEase(fadeCurve);

        buttonController.CheckForNoContent();

    }

    public void OpenButton() 
    {
        if (locked == false)
        {
            buttonController.CloseOtherEnabledButtons();
            buttonEnabled = true;

            //Do Tweening Stuff
            rectTransform.DOSizeDelta(new Vector2(946.8f, 667.457f), 0.3f);
            childRectTransform.DOLocalMoveY(190.49f, 0.3f);
            image.DOFade(1, 0.3f).SetEase(fadeCurve);
            //

            buttonController.ContentActivated();

        }
        else 
        {
            RotateLocked();
        }

    }

    
}

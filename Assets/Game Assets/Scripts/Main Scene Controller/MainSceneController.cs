using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    [Header("Settings Menu References")]
    public GameObject uiSettings;
    public RectTransform uiSettingsPopUp;
    public AnimationCurve uiSettingsAnimationCurve;
    public Material uiSettingsBlurMaterial;
    public RawImage uiSettingsScreenshotRawImage;
    [SerializeField]
    private Texture2D screenShot;

    [Header("Main UI References")]
    public GraphicRaycaster uiMainMenuGraphicRaycaster;

    [Header("Level Complete UI References")]
    public GameObject uiLevelComplete;
    public float uiLevelCompleteReveal = 0;
    public GraphicRaycaster uiLevelCompleteGraphicRayCaster;
    public AnimationCurve uiRevealCurve;
    private static int revealShaderID = Shader.PropertyToID("_Reveal");

    IEnumerator RecordFrame() 
    {
        yield return new WaitForEndOfFrame();
        var screenTex = ScreenCapture.CaptureScreenshotAsTexture();
        screenShot = new Texture2D(screenTex.width, screenTex.height, TextureFormat.RGB24, true);
        screenShot.SetPixels32(screenTex.GetPixels32());
        screenShot.Apply();
        uiSettingsScreenshotRawImage.texture = screenShot;

        Destroy(screenTex);
        OpenUISettingsPanel();
    }

    public void OpenUISettingsPanel() 
    {
        uiSettings.SetActive(true);
        uiSettingsPopUp.DOScale(0.46795f, 0.2f).SetEase(uiSettingsAnimationCurve);
        uiMainMenuGraphicRaycaster.enabled = false;
        uiSettingsBlurMaterial.DOFloat(1f, "_Radius", 0.5f).SetEase(uiSettingsAnimationCurve);
    }

    public void OpenSettings() 
    {
        StartCoroutine(RecordFrame());
    }

    public void ClosePopUp() 
    {
        uiSettings.SetActive(false);
        Destroy(screenShot);
    }

    public void CloseSettings() 
    {
        TweenCallback tweenCallback;
        tweenCallback = new TweenCallback(ClosePopUp);
        Tween tween = uiSettingsPopUp.DOScale(0.0f, 0.2f).SetEase(uiSettingsAnimationCurve).OnComplete(tweenCallback);
        uiSettingsBlurMaterial.DOFloat(0f, "_Radius", 0.5f).SetEase(uiSettingsAnimationCurve);

        uiMainMenuGraphicRaycaster.enabled = true;
    }


    Tween DoGlobalShaderTransition(float endValue, float duration)
    {
        Tween t = DOTween.To(() => Shader.GetGlobalFloat(revealShaderID), x => Shader.SetGlobalFloat(revealShaderID, x), endValue, duration);
        return t;
    }

    IEnumerator RevealLevelCompleteUI() 
    {
        Tween revealTween = DOTween.To(() => uiLevelCompleteReveal, x => uiLevelCompleteReveal = x, 1, 1f).OnUpdate(UpdateLevelCompleteShaderProperty).SetEase(uiRevealCurve);
        yield return revealTween.WaitForCompletion();
        uiLevelCompleteGraphicRayCaster.enabled = true;
    }

    IEnumerator HideLevelCompleteUI() 
    {
        Tween revealTween = DOTween.To(() => uiLevelCompleteReveal, x => uiLevelCompleteReveal = x, 0, 1f).OnUpdate(UpdateLevelCompleteShaderProperty).SetEase(uiRevealCurve);
        yield return revealTween.WaitForCompletion();
        uiLevelComplete.SetActive(false);
        uiMainMenuGraphicRaycaster.enabled = true;
    }

    void UpdateLevelCompleteShaderProperty() 
    {
        Shader.SetGlobalFloat(revealShaderID, uiLevelCompleteReveal);
    }

    public void CompleteLevel() 
    {
        uiMainMenuGraphicRaycaster.enabled = false;
        uiLevelComplete.SetActive(true);
        StartCoroutine(RevealLevelCompleteUI());
    }

    public void ReturnToHomeScreen() 
    {
        StartCoroutine(HideLevelCompleteUI());
    }


}

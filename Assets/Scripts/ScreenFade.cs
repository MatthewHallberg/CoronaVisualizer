using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenFade : MonoBehaviour {

    [HideInInspector]
    public bool isOpen;

    public bool defaultOff;

    readonly float fadeSpeed = 10f;

    float desiredAlpha;
    CanvasGroup canvasGroup;

    protected virtual void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        if (defaultOff) CloseScreen();
        else OpenScreen();
    }

    void ToggleScreen(bool active) {
        isOpen = active;
        desiredAlpha = active ? 1 : 0;
        if (canvasGroup != null) {
            canvasGroup.interactable = active;
            canvasGroup.blocksRaycasts = active;
        }
    }

    public bool IsOpen() {
        return desiredAlpha.Equals(1);
    }

    public void OpenScreen() {
        ToggleScreen(true);
    }

    public void CloseScreen(bool immediate = false) {
        ToggleScreen(false);
        if (immediate && canvasGroup != null) {
            desiredAlpha = 0;
            canvasGroup.alpha = 0;
        }
    }

    protected virtual void Update() {
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, desiredAlpha, Time.deltaTime * fadeSpeed);
    }
}

using UnityEngine;
using UnityEngine.Events;

public class ClickHandler : MonoBehaviour {

    [SerializeField]
    UnityEvent ClickEvent;

    [SerializeField]
    Animation anim;

    void OnMouseDown() {
        Debug.Log("Invoking Click Event...");
        anim.Play();
        ClickEvent.Invoke();
    }
}

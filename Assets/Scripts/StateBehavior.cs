using System.Collections;
using TMPro;
using UnityEngine;

public class StateBehavior : MonoBehaviour {

    const float SPEED = 6f;

    [SerializeField]
    TextMeshPro stateText;
    [SerializeField]
    TextMeshPro tested;
    [SerializeField]
    TextMeshPro positive;
    [SerializeField]
    TextMeshPro deaths;

    Vector3 desiredScale = Vector3.zero;
    bool isDestroying;
    StateTransform stateTransform;

    void Awake() {
        desiredScale = Vector3.zero;
        stateTransform = GetComponentInParent<StateTransform>();
    }

    void Update() {
        if (isDestroying && Vector3.Distance(transform.localScale,desiredScale) < .02f) {
            Destroy(gameObject);
        }
        transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * SPEED);
    }

    public void GotCollision(Transform state) {
        if (isDestroying) {
            return;
        }

        bool selected = state == transform;

        Vector3 LargeScale = Vector3.one * MapController.MAX_SCALE;
        Vector3 SmallScale = Vector3.one * MapController.MIN_SCALE;

        desiredScale = selected ? LargeScale : SmallScale;

        //moved selected state up
        stateTransform.SetSelected(selected);
    }

    public void DestroyElement() {
        isDestroying = true;
        desiredScale = Vector3.zero;
        transform.SetAsLastSibling();
    }

    public void Init(StateData data) {
        stateText.text = data.name;
        tested.text = data.tested;
        positive.text = data.positive;
        deaths.text = data.deaths;
        transform.localScale = Vector3.zero;
        StartCoroutine(DelayScaleRoutine());
    }

    IEnumerator DelayScaleRoutine() {
        yield return new WaitForSeconds(1f);
        desiredScale = Vector3.one * MapController.MIN_SCALE;
    }
}

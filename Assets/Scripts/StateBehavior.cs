using TMPro;
using UnityEngine;

public class StateBehavior : MonoBehaviour {

    const float SPEED = 6f;

    [SerializeField]
    TextMeshPro stateText;
    [SerializeField]
    TextMeshPro stateNumbers;

    Vector3 desiredScale = Vector3.zero;
    float distance;
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

        if (selected) {
            desiredScale = Vector3.one * MapController.Instance.MAX_SCALE * 1.1f;
        } else {
            //calculate distance from selected state and scale accordingly
            distance = Vector3.Distance(transform.position, state.position);
            float mult = ExtensionMethods.Remap(distance,
                MapController.Instance.MIN_DIST, MapController.Instance.MAX_DIST,
                MapController.Instance.MAX_SCALE, MapController.Instance.MIN_SCALE);
            mult = Mathf.Clamp(mult, MapController.Instance.MIN_SCALE, MapController.Instance.MAX_SCALE);
            desiredScale = Vector3.one * mult;
        }

        //moved selected state up
        stateTransform.SetSelected(selected);
    }

    public void DestroyElement() {
        isDestroying = true;
        desiredScale = Vector3.zero;
        transform.SetAsLastSibling();
    }

    public void Init(StateData data, MapController.SelectedState desiredState) {
        stateText.text = data.name;
        stateNumbers.text = SetNumbers(data, desiredState);
        transform.localScale = Vector3.zero;
        desiredScale = Vector3.one * MapController.Instance.MIN_SCALE;
    }

    string SetNumbers(StateData data, MapController.SelectedState desiredState) {
        switch (desiredState) {
        case MapController.SelectedState.TESTED:
            return data.tested;
        case MapController.SelectedState.POSITIVE:
            return data.positive;
        default:
            return data.deaths;
        }
    }
}

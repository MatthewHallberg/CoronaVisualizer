using TMPro;
using UnityEngine;

public class StateBehavior : MonoBehaviour {

    const float SPEED = 5f;

    [SerializeField]
    TextMeshPro stateText;
    [SerializeField]
    TextMeshPro stateNumbers;

    Vector3 desiredScale = Vector3.zero;
    bool isColliding;
    bool isDestroying;

    void Awake() {
        desiredScale = Vector3.zero;
    }

    void Update() {
        if (isDestroying) {
            desiredScale = Vector3.zero;
            if (Vector3.Distance(transform.localScale,desiredScale) < .1f) {
                Destroy(gameObject);
            }
        } else {
            desiredScale = isColliding ? Vector3.one * MapController.Instance.MAX_SCALE : Vector3.one * MapController.Instance.MIN_SCALE;
            isColliding = false;
        }
        transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * SPEED);
    }

    public void GotCollision() {
        isColliding = true;
        desiredScale = Vector3.one * MapController.Instance.MAX_SCALE;
    }

    public void DestroyElement() {
        isDestroying = true;
        desiredScale = Vector3.zero;
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

using UnityEngine;

public class BarBehavior : MonoBehaviour {

    const float SPEED = 4f;

    Vector3 desiredScale;

    void Start() {
        desiredScale = transform.localScale;
    }

    void Update() {
        transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * SPEED);
    }

    public void SetScale(float scale) {
        desiredScale.y = scale;
    }
}

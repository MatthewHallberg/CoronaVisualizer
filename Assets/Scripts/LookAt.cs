using UnityEngine;

public class LookAt : MonoBehaviour {

    Transform cam;
    Vector3 targetAngle = Vector3.zero;

    void Start() {
        cam = Camera.main.transform;
    }

    void Update() {
        transform.LookAt(cam);
        targetAngle = transform.localEulerAngles;
        targetAngle.z = targetAngle.y;
        transform.localEulerAngles = targetAngle;
    }
}

using UnityEngine;

public class LookAt : MonoBehaviour {

    Transform cam;
    Vector3 camAngle;

    void Start() {
        cam = Camera.main.transform;
    }

    void Update() {
        transform.LookAt(cam);
        camAngle = transform.localEulerAngles;
        camAngle.y = 0;
        transform.localEulerAngles = camAngle;
    }
}

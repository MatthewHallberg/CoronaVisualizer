using UnityEngine;

public class CameraDirection : MonoBehaviour {

    Transform cam;

    void Start() {
        cam = Camera.main.transform;
    }

    void Update() {
        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit)) {
            GotHit(hit.collider.gameObject);
        }
    }

    void GotHit(GameObject go) {
        if (go.transform.childCount > 0) {
            StateBehavior stateBehavior = go.transform.GetChild(0).GetComponent<StateBehavior>();
            if (stateBehavior != null) {
                stateBehavior.GotCollision();
            }
        }
    }
}

using TMPro;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DistanceLine : MonoBehaviour {

    const float FEET_CONVERSION = 3.281f;

    readonly Vector3 camOffset = new Vector3(0, -.1f, 0);
    readonly Vector3 textOffset = new Vector3(0, .06f, 0);

    float minText = .01f;
    float maxText = .75f;
    float maxDistance = 10;
    float textDistance = .5f;

    [SerializeField]
    TextMeshPro distanceText;
    [SerializeField]
    Transform person;

    Transform cam;
    LineRenderer lineRenderer;

    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        cam = Camera.main.transform;
    }

    void Update() {

        //get camera and person position
        Vector3 camPos = cam.position + camOffset;
        Vector3 personPos = person.position;

        //find distance from camera to person
        float distanceFeet = Vector3.Distance(camPos, personPos) * FEET_CONVERSION;
        distanceText.text = distanceFeet.ToString("0.0") + "ft";

        float clampedDistance = Mathf.Clamp(distanceFeet, 0, maxDistance - 1);

        //plot point on line
        textDistance = ExtensionMethods.Remap(clampedDistance, 0f, maxDistance, maxText, minText);
        Vector3 textPos = Vector3.Lerp(camPos, personPos, textDistance);
        distanceText.transform.parent.position = textPos + textOffset;

        lineRenderer.SetPosition(0, camPos);
        lineRenderer.SetPosition(1, personPos);

        if (Damage.Instance != null && distanceFeet < 6) {
            Damage.Instance.TakeDamage();
        } else {
            Damage.Instance.StopDamage();
        }
    }
}

using UnityEngine;

public class StateTransform : MonoBehaviour {

    const float MOVE_SPEED = 7f;
    const float COL_SPEED = 5f;

    Vector3 startPos;
    Vector3 selectedPos;
    Vector3 desiredPos;

    Renderer rend;
    Color desiredColor;
    Color currColor;

    void Start() {
        startPos = transform.localPosition;
        selectedPos = startPos + new Vector3(0, 0, .08f);
        desiredPos = startPos;
        rend = GetComponent<Renderer>();
        currColor = Color.white;
    }

    void Update() {
        transform.localPosition = Vector3.Lerp(transform.localPosition, desiredPos, Time.deltaTime * MOVE_SPEED);
        //currColor = Color.Lerp(currColor, desiredColor, Time.deltaTime * COL_SPEED);
        //rend.material.SetColor("_BaseColor", currColor);
    }

    public void SetSelected(bool selected) {
        desiredPos = selected ? selectedPos : startPos;
    }

    public void SetColor(float percentValue) {
        percentValue = Mathf.Pow(percentValue, 1f / 3f);
        percentValue *= 2;
        percentValue = Mathf.Clamp(percentValue, 0, 1f);
        //set color
        desiredColor.r = percentValue;
        desiredColor.g = 1f - percentValue;
        desiredColor.b = 1f - percentValue;
        desiredColor.a = 1f;
        rend.material.SetColor("_BaseColor", desiredColor);
    }
}

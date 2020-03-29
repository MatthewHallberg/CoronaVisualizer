using System.Collections;
using UnityEngine;

public class Animate : MonoBehaviour {

    const float SPEED = 5f;

    Vector3 desiredScale;

    public virtual void Awake() {
        transform.localScale = Vector3.zero;
        MakeSmall();
    }

    void Update() {
        transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * SPEED);
    }

    public void MakeSmall () {
        desiredScale = Vector3.zero;
    }

    public void MakeBig() {
        StartCoroutine(MakeBigRoutine());
    }

    IEnumerator MakeBigRoutine() {
        desiredScale = Vector3.zero;

        while (transform.localScale.x > .02f) {
            yield return new WaitForSeconds(.1f);
        }
        desiredScale = Vector3.one;
    }
}

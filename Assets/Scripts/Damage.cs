using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Damage : Singleton<Damage> {

    const float SPEED = 6f;
    const float WAIT_TIME = .5f;

    Image image;
    Color desiredColor;

    void Start() {
        image = GetComponent<Image>();
        desiredColor = Color.white;
        desiredColor.a = 0;
    }

    void Update() {
        image.color = Color.Lerp(image.color, desiredColor, Time.deltaTime * SPEED);
    }

    public void TakeDamage() {
        if (DamageCoroutine == null) {
            DamageCoroutine = StartCoroutine(DamageRoutine());
        }
    }

    public void StopDamage() {
        if (DamageCoroutine != null) {
            StopCoroutine(DamageCoroutine);
            DamageCoroutine = null;
            desiredColor.a = 0;
        }
    }

    Coroutine DamageCoroutine;
    IEnumerator DamageRoutine() {
        while (true) {
            desiredColor.a = .5f;
            yield return new WaitForSeconds(WAIT_TIME);
            desiredColor.a = 1f;
            yield return new WaitForSeconds(WAIT_TIME);
        }
    }
}

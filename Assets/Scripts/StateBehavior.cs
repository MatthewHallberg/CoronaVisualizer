using TMPro;
using UnityEngine;

public class StateBehavior : MonoBehaviour {

    [SerializeField]
    TextMeshPro stateText;
    [SerializeField]
    TextMeshPro stateNumbers;

    public void Init(StateData data) {
        stateText.text = data.name;
        stateNumbers.text = data.positive;
    }
}

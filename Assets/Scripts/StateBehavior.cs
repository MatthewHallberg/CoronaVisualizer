using TMPro;
using UnityEngine;

public class StateBehavior : MonoBehaviour {

    [SerializeField]
    TextMeshPro stateText;
    [SerializeField]
    TextMeshPro stateNumbers;

    public void Init(StateData data, MapController.SelectedState desiredState) {
        stateText.text = data.name;
        stateNumbers.text = SetNumbers(data, desiredState);
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

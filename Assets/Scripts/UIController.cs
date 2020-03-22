using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController> {

    [SerializeField]
    Text totalText;
    [SerializeField]
    List<Toggle> toggles = new List<Toggle>();

    void Start() {
        SetDefaultState(MapController.SelectedState.POSITIVE);
    }

    public void OnTestedSelected(bool selected) {
        if (!selected) {
            return;
        }
        MapController.Instance.ChangeState(MapController.SelectedState.TESTED);
    }

    public void OnPositiveSelected(bool selected) {
        if (!selected) {
            return;
        }
        MapController.Instance.ChangeState(MapController.SelectedState.POSITIVE);
    }

    public void OnDeathsSelected(bool selected) {
        if (!selected) {
            return;
        }
        MapController.Instance.ChangeState(MapController.SelectedState.DEATHS);
    }

    public void SetTotalText(string total) {
        totalText.text = total;
    }

    void SetDefaultState(MapController.SelectedState selectedState) {
        toggles[(int)selectedState].isOn = true;
        MapController.Instance.ChangeState(selectedState);
    }
}

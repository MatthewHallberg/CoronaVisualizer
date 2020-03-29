using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;

public class MapController : Animate, iController {

    public static readonly float MAX_SCALE = 0.15f;
    public static readonly float MIN_SCALE = .02f;

    [SerializeField]
    TextMeshPro txtDeathTotal;
    [SerializeField]
    TextMeshPro txtPositiveTotal;
    [SerializeField]
    TextMeshPro txtTestedTotal;

    [SerializeField]
    GameObject StatePrefab;

    List<StateBehavior> currStates = new List<StateBehavior>();
    int deathTotal;
    int testedTotal;
    int positiveTotal;

    void Start() {
        GetData();
    }

    public void GetData() {
        Reset();
        API.Instance.GetVirusData(OnDataRecieved);
    }

    void Reset() {
        MakeSmall();
        DestroyAllInfo();
        ResetStateColors();
    }

    void DestroyAllInfo() {
        foreach (StateBehavior state in currStates) {
            state.DestroyElement();
        }
        currStates.Clear();
    }

    void OnDataRecieved(List<StateData> states) {
        MakeBig();
        StartCoroutine(LoadStateRoutine(states));
    }

    IEnumerator LoadStateRoutine(List<StateData> states) {
        //update totals for main UI element
        UpdateTotals(states);
        //load prefab
        foreach (StateData state in states) {
            LoadState(state);
            yield return new WaitForEndOfFrame();
        }
        //update total
        UpdateTotal();
    }

    void UpdateTotal() {

        txtDeathTotal.text = string.Format("{0:#,###0}", deathTotal);
        txtPositiveTotal.text = string.Format("{0:#,###0}", positiveTotal);
        txtTestedTotal.text = string.Format("{0:#,###0}", testedTotal);

        deathTotal = 0;
        testedTotal = 0;
        positiveTotal = 0;
    }

    void LoadState(StateData stateData) {
        //find state object
        Transform parent = transform.Find(stateData.name);

        if (parent == null) {
            return;
        }

        //instiate prefab as child
        GameObject StateGO = Instantiate(StatePrefab, parent);

        //initialize gameobject behavior
        StateBehavior stateBehavior = StateGO.GetComponent<StateBehavior>();
        currStates.Add(stateBehavior);
        stateBehavior.Init(stateData);

        //change color of state
        StateTransform stateTransform = parent.GetComponent<StateTransform>();
        float.TryParse(stateData.positive, out float positiveCases);
        float percentOfTotal = positiveCases / (float)positiveTotal;
        stateTransform.SetColor(percentOfTotal);
    }

    void ResetStateColors() {
        foreach (StateTransform state in FindObjectsOfType<StateTransform>()) {
            state.ResetColor();
        }
    }

    void UpdateTotals(List<StateData> states) {
        foreach (StateData state in states) {
            //keep track of desired total, some values are empty
            if (int.TryParse(state.tested, out int tested)) {
                testedTotal += tested;
            }

            if (int.TryParse(state.deaths, out int deaths)) {
                deathTotal += deaths;
            }

            if (int.TryParse(state.positive, out int positive)) {
                positiveTotal += positive;
            }
        }
    }
}



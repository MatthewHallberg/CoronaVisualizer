using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using TMPro;
using System;

public class MapController : Singleton<MapController> {

    public float MAX_SCALE = 1f;
    public float MIN_SCALE = .3f;
    public float MAX_DIST = .1f;
    public float MIN_DIST = 0f;

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
        UpdateInfo();
    }

    void UpdateInfo() {
        DestroyAllInfo();
        API.Instance.GetVirusData(OnDataRecieved);
    }

    void DestroyAllInfo() {
        foreach (StateBehavior state in currStates) {
            state.DestroyElement();
        }
        currStates.Clear();
    }

    void OnDataRecieved(string data) {
        StartCoroutine(LoadStateRoutine(data));
    }

    IEnumerator LoadStateRoutine(string data) {
        //get list of states
        List<StateData> States = ParseData(data);
        //load prefab
        foreach (StateData state in States) {
            LoadState(state);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
        }
        //update total
        UpdateTotal();
    }

    void UpdateTotal() {
        txtDeathTotal.text = deathTotal.ToString();
        txtPositiveTotal.text = positiveTotal.ToString();
        txtTestedTotal.text = testedTotal.ToString();

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

    List<StateData> ParseData(string data) {
        List<string> dataLines = data.Split('\n').ToList();
        //print("FORMAT: " + dataLines[0]);
        //removed first and last line
        dataLines.RemoveAt(0);
        dataLines.RemoveAt(dataLines.Count - 1);

        //create list to return
        List<StateData> States = new List<StateData>();

        //parse each line
        foreach (string line in dataLines) {
            //print(line);
            string[] lineInfo = line.Split(',');
            //create state Data object from array values
            StateData stateData = new StateData {
                name = lineInfo[0],
                tested = string.IsNullOrEmpty(lineInfo[1]) ? "0" : lineInfo[1],
                positive = string.IsNullOrEmpty(lineInfo[2]) ? "0" : lineInfo[2],
                deaths = string.IsNullOrEmpty(lineInfo[3]) ? "0" : lineInfo[3]
            };

            //keep track of desired total, some values are empty
            if (int.TryParse(stateData.tested, out int tested)) {
                testedTotal += tested;
            }

            if (int.TryParse(stateData.deaths, out int deaths)) {
                deathTotal += deaths;
            }

            if (int.TryParse(stateData.positive, out int positive)) {
                positiveTotal += positive;
            }

            States.Add(stateData);
        }
        return States;
    }
}



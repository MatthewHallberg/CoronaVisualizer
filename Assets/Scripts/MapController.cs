using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class MapController : Singleton<MapController> {

    public readonly float MAX_SCALE = 1f;
    public readonly float MIN_SCALE = .3f;
    public float MAX_DIST = .35f;
    public readonly float MIN_DIST = 0f;

    [SerializeField]
    GameObject StatePrefab;

    public enum SelectedState {
        TESTED, POSITIVE, DEATHS
    };

    SelectedState currState;
    List<StateBehavior> currStates = new List<StateBehavior>();
    int currTotal;

    public void ChangeState(SelectedState desiredState) {
        if (currState != desiredState) {
            currState = desiredState;
            DestroyAllInfo();
            API.Instance.GetVirusData(OnDataRecieved);
        }
    }

    void DestroyAllInfo() {
        currTotal = 0;
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
        string totalText = "Total ";
        switch (currState) {
        case SelectedState.TESTED:
            totalText += "tested ";
            break;
        case SelectedState.POSITIVE:
            totalText += "positives ";
            break;
        default:
            totalText += "deaths  ";
            break;
        }
        totalText += currTotal;
        UIController.Instance.SetTotalText(totalText);
    }

    void LoadState(StateData stateData) {
        //find state object
        Transform parent = transform.Find(stateData.name);

        if (parent == null) {
            return;
        }

        //instiate prefab as child
        GameObject StateGO = Instantiate(StatePrefab,parent);

        //initialize gameobject behavior
        StateBehavior stateBehavior = StateGO.GetComponent<StateBehavior>();
        currStates.Add(stateBehavior);
        stateBehavior.Init(stateData, currState);

        //change color of state
        StateTransform stateTransform = parent.GetComponent<StateTransform>();
        float percentOfTotal = (float)stateData.selectedNum / (float)currTotal;
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
                tested = lineInfo[1],
                positive = lineInfo[2],
                deaths = lineInfo[3]
            };

            //keep track of desired total, some values are empty
            if (int.TryParse(lineInfo[(int)currState + 1], out int num)){
                currTotal += num;
                stateData.selectedNum = num;
            }

            States.Add(stateData);
        }
        return States;
    }
}



using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapController : MonoBehaviour {

    [SerializeField]
    GameObject StatePrefab;

    void Start() {
        API.Instance.GetVirusData(OnDataRecieved);
    }

    void OnDataRecieved(string data) {
        //get list of states
        List<StateData> States = ParseData(data);
        //load prefab
        foreach(StateData state in States) {
            LoadState(state);
        }
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
        stateBehavior.Init(stateData);
    }

    List<StateData> ParseData(string data) {
        List<string> dataLines = data.Split('\n').ToList();
        print("FORMAT: " + dataLines[0]);
        //removed first and last line
        dataLines.RemoveAt(0);
        dataLines.RemoveAt(dataLines.Count - 1);

        //create list to return
        List<StateData> States = new List<StateData>();

        //parse each line
        foreach (string line in dataLines) {
            print(line);
            //each line does not always contain data so create fake one
            string[] stateDataArray = new string[4];
            string[] lineInfo = line.Split(',');
            //load array that is our desired size
            for (int i = 0; i < lineInfo.Length; i++) {
                stateDataArray[i] = lineInfo[i];
            }
            //create state Data object from array values
            StateData stateData = new StateData {
                name = stateDataArray[0],
                tested = stateDataArray[1],
                positive = stateDataArray[2],
                deaths = stateDataArray[3]
            };
            States.Add(stateData);
        }
        return States;
    }
}



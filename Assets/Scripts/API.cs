using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class API : Singleton<API> {

    const string STATE_DATA = "http://coronavirusapi.com/states.csv";
    const string TIME_DATA = "http://coronavirusapi.com/time_series.csv";
    const string WORLD_DATA = "https://www.worldometers.info/coronavirus/";

    public void GetWorldData(UnityAction<int> callback) {
        StartCoroutine(GetWorldDataRoutine(callback));
    }

    IEnumerator GetWorldDataRoutine(UnityAction<int> callback) {
        UnityWebRequest webRequest = UnityWebRequest.Get(WORLD_DATA);
        // Request and wait for the desired page.
        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError) {
            Debug.Log("Error: " + webRequest.error);
        } else {
            callback(ParseWorldData(webRequest.downloadHandler.text));
        }
    }

    int ParseWorldData(string text) {
        List<string> dataLines = text.Split('\n').ToList();
        List<string> desiredLine = dataLines[10].Split(' ').ToList();
        string total = desiredLine[3].Replace(",","");
        int.TryParse(total, out int totalWorldCases);
        return totalWorldCases;
    }

    public void GetVirusData(UnityAction<List<StateData>> callback) {
        StartCoroutine(GetVirusDataRoutine(callback));
    }

    IEnumerator GetVirusDataRoutine(UnityAction<List<StateData>> callback) {
        UnityWebRequest webRequest = UnityWebRequest.Get(STATE_DATA);
        // Request and wait for the desired page.
        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError) {
            Debug.Log("Error: " + webRequest.error);
        } else {
            callback(ParseStateData(webRequest.downloadHandler.text));
        }
    }

    List<StateData> ParseStateData(string data) {
        List<string> dataLines = data.Split('\n').ToList();
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
            States.Add(stateData);
        }
        return States;
    }

    public void GetTimeData(UnityAction<List<TimeData>> callback) {
        StartCoroutine(GetTimeDateRoutine(callback));
    }

    IEnumerator GetTimeDateRoutine(UnityAction<List<TimeData>> callback) {

        UnityWebRequest www = UnityWebRequest.Get(TIME_DATA);
        yield return www.SendWebRequest();

        if (www.isNetworkError) {
            Debug.Log("Error: " + www.error);
        } else {
            callback(ParseTimeData(www.downloadHandler.text));
        }
    }

    List<TimeData> ParseTimeData(string data) {

        List<string> lines = data.Split('\n').ToList();
        lines.RemoveAt(0);
        lines.RemoveAt(lines.Count - 1);

        //create list of TimeData
        List<TimeData> dataList = new List<TimeData>();

        foreach (string line in lines) {
            List<string> lineData = line.Split(',').ToList();
            TimeData timeData = new TimeData {
                date = DateTime.Parse(lineData[0]),
                tested = int.Parse(lineData[3]),
                positive = int.Parse(lineData[4]),
                deaths = int.Parse(lineData[5])
            };
            dataList.Add(timeData);
        }
        return dataList;
    }
}

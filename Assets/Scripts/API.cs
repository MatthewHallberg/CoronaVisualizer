using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class API : MonoBehaviour {

    const string ENDPOINT = "http://coronavirusapi.com/states.csv";

    void Start() {
        GetVirusData();
    }

    void GetVirusData() {
        StartCoroutine(GetVirusDataRoutine());
    }

    IEnumerator GetVirusDataRoutine() {
        UnityWebRequest webRequest = UnityWebRequest.Get(ENDPOINT);
        // Request and wait for the desired page.
        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError) {
            Debug.Log("Error: " + webRequest.error);
        } else {
            Debug.Log("Received: " + webRequest.downloadHandler.text);
        }
    }
}

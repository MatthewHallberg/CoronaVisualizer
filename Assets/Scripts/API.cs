using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class API : Singleton<API> {

    const string ENDPOINT = "http://coronavirusapi.com/states.csv";

    public void GetVirusData(UnityAction<string> callback) {
        StartCoroutine(GetVirusDataRoutine(callback));
    }

    IEnumerator GetVirusDataRoutine(UnityAction<string> callback) {
        UnityWebRequest webRequest = UnityWebRequest.Get(ENDPOINT);
        // Request and wait for the desired page.
        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError) {
            Debug.Log("Error: " + webRequest.error);
        } else {
            callback.Invoke(webRequest.downloadHandler.text);
        }
    }
}

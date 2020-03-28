using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldController : MonoBehaviour {

    [SerializeField]
    GameObject cube;
    [SerializeField]
    TextMeshPro titleText;

    List<GameObject> cubes = new List<GameObject>();

    void Start() {
        Simulate();
    }

    void OnMouseDown() {
        Simulate();
    }

    void Simulate() {
        DeleteAll();
        API.Instance.GetWorldData(OnDataRecieved);
    }

    void OnDataRecieved(int count) {
        StartCoroutine(CreateCubeRoutine(count));
    }

    IEnumerator CreateCubeRoutine(int count) {
        yield return new WaitForFixedUpdate();
        //set text
        titleText.text = "Cases: " + string.Format("{0:#,###0}", count);
        string total = count.ToString();

        //create cubes
        int numCubes = count / 10000;
        for (int i = 0; i < numCubes; i++) {
            GameObject cubeObject = Instantiate(cube,transform);
            cubes.Add(cubeObject);
            //random position
            Vector3 pos;
            //configure
            float range = .11f;
            float size = .08f;
            float waitTime = .01f;
            //generate from rand pos
            pos.x = UnityEngine.Random.Range(-range, range);
            pos.y = .1f;
            pos.z = UnityEngine.Random.Range(-range, range);
            cubeObject.transform.localPosition = pos;
            //set scale
            cubeObject.transform.localScale = Vector3.one * size;
            yield return new WaitForEndOfFrame();
        }
    }

    void DeleteAll() {
        StopAllCoroutines();
        //clear old cubes
        foreach (GameObject obj in cubes) {
            Destroy(obj);
        }
        cubes.Clear();
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }
}

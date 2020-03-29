using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldController : Animate, iController {

    const float WAIT_TIME = 2f;

    [SerializeField]
    GameObject cube;
    [SerializeField]
    TextMeshPro titleText;

    List<GameObject> cubes = new List<GameObject>();

    void Start() {
        GetData();
    }

    public void GetData() {
        Reset();
        API.Instance.GetWorldData(OnDataRecieved);
    }

    void Reset() {
        StopAllCoroutines();
        MakeSmall();
        DeleteAll();
    }

    void OnDataRecieved(int count) {
        MakeBig();
        StartCoroutine(CreateCubeRoutine(count));
    }

    IEnumerator CreateCubeRoutine(int count) {
        //set text
        titleText.text = "Cases: " + string.Format("{0:#,###0}", count);
        string total = count.ToString();
        while (true) {
            yield return new WaitForEndOfFrame();
            //create cubes
            int numCubes = count / 10000;
            for (int i = 0; i < numCubes; i++) {
                GameObject cubeObject = Instantiate(cube, transform);
                cubes.Add(cubeObject);
                //random position
                Vector3 pos;
                //configure
                float range = .11f;
                float size = .08f;
                //generate from rand pos
                pos.x = UnityEngine.Random.Range(-range, range);
                pos.y = .1f;
                pos.z = UnityEngine.Random.Range(-range, range);
                cubeObject.transform.localPosition = pos;
                //set scale
                cubeObject.transform.localScale = Vector3.one * size;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(WAIT_TIME);
            DeleteAll();
        }
    }

    void DeleteAll() {
        //clear old cubes
        foreach (GameObject obj in cubes) {
            Destroy(obj);
        }
        cubes.Clear();
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }
}

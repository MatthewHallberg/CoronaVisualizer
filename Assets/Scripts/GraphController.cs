using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GraphController : Animate, iController {

    public float WaitTime;

    [SerializeField]
    TextMeshPro title;

    [SerializeField]
    BarBehavior deathBar;
    [SerializeField]
    BarBehavior positiveBar;
    [SerializeField]
    BarBehavior testedBar;

    void Start() {
        GetData();
    }

    public void GetData() {
        Reset();
        API.Instance.GetTimeData(OnDataRecieved);
    }

    void Reset() {
        MakeSmall();

    }

    void OnDataRecieved(List<TimeData> dataList) {
        MakeBig();
        StartCoroutine(CycleDataRoutine(dataList));
    }

    IEnumerator CycleDataRoutine(List<TimeData> dataList) {
        while (true) {

            float maxNum = dataList.Max(x => x.tested);

            foreach (TimeData data in dataList) {
                title.text = data.date.ToString("MMMM dd, yyyy");

                float testedScale = ExtensionMethods.Remap(data.tested, 0, maxNum, 0, 10);
                testedBar.SetScale(testedScale);

                float positiveScale = ExtensionMethods.Remap(data.positive, 0, maxNum, 0, 10);
                positiveBar.SetScale(positiveScale);

                float deathScale = ExtensionMethods.Remap(data.deaths, 0, maxNum, 0, 10);
                deathBar.SetScale(deathScale);

                yield return new WaitForSeconds(WaitTime);
            }
        }
    }
}

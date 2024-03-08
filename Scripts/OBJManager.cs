using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OBJManager : MonoBehaviour
{
    public GameObject groundEnemyGo;
    public List<GameObject> groundEnemy;
    private GameObject[] targetPool;
    public int capacity = 9;

    public static OBJManager instance;

    private void Awake()
    {
        OBJManager.instance = this;
    }

    void Start()
    {
        groundEnemy = new List<GameObject>();
        this.Generate();
    }

    public void Generate()
    {
        Debug.LogFormat("<color=yellow>Generate</color>");

        for (int i = 0; i < capacity; i++)
        {
            var go = Instantiate(groundEnemyGo, this.transform);
            go.SetActive(false);
            var enemy = go.GetComponent<Enemy>();
            this.groundEnemy.Add(go);
        }
    }

    public GameObject GetEnemy(string type)
    {
        Debug.LogFormat("<color=yellow>GetEnemy</color>");

        if (this.groundEnemy.Count == 0) return null;

        var enemy = this.groundEnemy.FirstOrDefault();
        this.groundEnemy.Remove(enemy);

        Debug.LogFormat("=> {0}, => {1}", enemy.name, enemy.transform.position);

        enemy.SetActive(true);
        enemy.transform.parent = null;
        return enemy;
        //switch (type)
        //{
        //    case "groundEnemy":
        //        targetPool = groundEnemy;

        //        break;

        //}
        //for (int i = 0; i < targetPool.Length; i++)
        //{
        //    if (!targetPool[i].activeSelf) //활성 상태가 아닐 때
        //    {
        //        targetPool[i].SetActive(true);
        //        targetPool[i].transform.parent = null;
        //        return targetPool[i];
        //    }

        //}
        //return null;
    }

    public void RelaseObject(GameObject go)
    {
        Debug.LogFormat("<color=cyan> <size=20>RelaseObject </size> </color>");

        go.SetActive(false);
        go.transform.SetParent(this.transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        this.groundEnemy.Add(go);

    }

}

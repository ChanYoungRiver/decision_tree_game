using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLauncher1 : MonoBehaviour
{

    // Use this for initialization
    private static GameLauncher1 instance;
    private List<GameObject> enemyList;

    public List<GameObject> enemyListPostion;

    void Start()
    {
        enemyList = new List<GameObject>();
        initEnemy();
        instance = this;
    }

    public static GameLauncher1 GetInstance()
    {
        return instance;
    }

    void initEnemy()
    {

        enemyList.Clear();
        foreach (GameObject enemyPos in enemyListPostion)
        {
            GameObject enemyObject = Instantiate(Resources.Load("Prefab/enemy/Enemy")) as GameObject;
          //  enemyObject.transform.SetParent(GameObject.Find("GameView").transform);
            enemyObject.transform.position = enemyPos.transform.position;
            Debug.Log(enemyPos.transform.position);
            enemyList.Add(enemyObject);
           // enemyPos.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (enemyList.Count > 0)
        {
            foreach (GameObject enemy in enemyList)
            {
                if (enemy.GetComponent<Enemy3DPlayer>().hp <= 0)
                {
                    enemyList.Remove(enemy);
                    Destroy(enemy);
                    break;
                }
            }
        }

    }

    public bool CheckExistEnemy()
    {
        return enemyList.Count > 0;
    }



}

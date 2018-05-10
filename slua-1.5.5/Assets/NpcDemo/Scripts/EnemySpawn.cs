using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
    public Transform enemy;
    public int enemyCount = 0;
    public int maxEnemy = 3;
    public float timer = 30;
    protected Transform m_transform;

	// Use this for initialization
	void Start () {
        m_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (enemyCount >= maxEnemy)
            return;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 60;
            Transform obj = (Transform)Instantiate(enemy, m_transform.position, Quaternion.identity);
            Enemy enemyIden = obj.GetComponent<Enemy>();
            enemyIden.Init(this);
            
        }
	}
    private void OnDrawGizmos()
    {
       // Gizmos.DrawIcon(transform.position, "item.png", true);
    }
}

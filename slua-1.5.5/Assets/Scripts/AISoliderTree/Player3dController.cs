using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.AIBehaviorTree;
using DG.Tweening;
public class Player3dController : PlayerController
{

    // Use this for initialization
    public Solider3dPlayer m_Character1;



    void Start()
    {
        m_Character1 = GetComponent<Solider3dPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.tree != null && this.m_Character1 != null)
        {
            this.tree.Run(this.m_Character1);
        }
    }


    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 40), "load"))
        {
            BTreeMgr.sInstance.Load(text.text);
            this.tree = BTreeMgr.sInstance.GetTree("test_tree");
        }
    }
}

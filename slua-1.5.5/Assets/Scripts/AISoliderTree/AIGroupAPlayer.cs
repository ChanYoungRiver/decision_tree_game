using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGroupAPlayer : AIPlayer {


	public override void initConfig(){
		belongGroup = AIPlayerGroup.A;
		hp = 200;
	}

}

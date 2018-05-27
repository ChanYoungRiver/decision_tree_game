using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGroupBPlayer : AIPlayer {

	public override void initConfig(){
		belongGroup = AIPlayerGroup.B;
		hp = 100;
	}
}

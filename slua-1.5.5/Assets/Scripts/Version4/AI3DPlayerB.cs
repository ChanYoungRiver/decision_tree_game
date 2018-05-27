using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI3DPlayerB : AI3DPlayer {

	public override void initConfig(){
		belongGroup = AIPlayerGroup.B;
		routeGroup = "RouteListGroupB";
		dangersHP = 40;
	}
}

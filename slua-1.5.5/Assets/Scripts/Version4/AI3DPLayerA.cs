using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI3DPLayerA : AI3DPlayer {

	public override void initConfig(){
		belongGroup = AIPlayerGroup.A;
		routeGroup = "RouteListGroupA";
		dangersHP = 80;
	}
}

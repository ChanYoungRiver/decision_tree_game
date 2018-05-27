using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutePoint {

	public Vector3 pointPos;
	public bool isPass;
	public string pointName;

}

public class RoutePointList {

	public List<RoutePoint> pointList;
	public bool isLinePass;
	public RoutePointList(){
		pointList = new List<RoutePoint> ();
	}

}

public class RouteManager{


	public List<RoutePointList> routeList;
	public RoutePoint basePos;

	public RouteManager(){
		basePos = new RoutePoint ();
		basePos.pointPos = Vector3.zero;

		routeList = new List<RoutePointList> ();
	}

	public RoutePoint getCurPoint(){
		foreach(RoutePointList routePointList in routeList){
			if(!routePointList.isLinePass )
			{
				foreach(RoutePoint routePoint in routePointList.pointList)
				{
					if (!routePoint.isPass) {
						return routePoint;
					}
				}
				routePointList.isLinePass = true;

			}
		
		}

		return basePos;

	}

}

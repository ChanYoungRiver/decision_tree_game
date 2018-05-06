using UnityEngine;

//Not Exported in builds
namespace DaiMangou.ProRadarBuilder
{
#if (UNITY_EDITOR)
    public class Visualization2D : MonoBehaviour
    {
        private _2DRadar Radar;

        public void OnDrawGizmos()
        {
            transform.hideFlags = HideFlags.None;

            if (!Radar) Radar = this.GetComponent<_2DRadar>();


            if (Radar.RadarDesign == null) return;
            if (!Radar.RadarDesign.Visualize) return;
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, Radar.RadarDesign.TrackingBounds * transform.lossyScale.x);
            UnityEditor.Handles.color = Color.cyan;
            UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, Radar.RadarDesign.InnerCullingZone * transform.lossyScale.x);
            if (Radar.RadarDesign.UseLocalScale) return;
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, Radar.RadarDesign.RadarDiameter);


            // Gizmos.DrawFrustum(transform.position, 60, 20, 0.3f, 1);
        }

    }
#endif
}

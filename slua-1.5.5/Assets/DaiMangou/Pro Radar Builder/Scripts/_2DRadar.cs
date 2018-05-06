/*        /// <summary>
        /// <para> Defines the object which is to be targeted and the way in which it must be rotated </para>
        /// </summary>
        [System.Serializable]
        public class RotationTarget
        {
            /// <summary>
            /// called only from editor , and is not necessary at runtime 
            /// </summary>
            public bool ShowDesignSetings;
            /// <summary>
            /// When true , the z rotation will be the same as the Y rotation
            /// </summary>
            public bool UseY;
            /// <summary>
            /// Freeze rotation around particular axis
            /// </summary>
            public bool FreezeX, FreezeY, FreezeZ;
            /// <summary>
            /// Damping used to control rotation of particular design layer
            /// </summary>
            public float RotationDamping;
            /// <summary>
            /// the string tag you define 
            /// </summary>
            public string tag;
            /// <summary>
            /// the name of the object you wish to find
            /// </summary>
            public string FindingName;
            /// <summary>
            /// The name of the instanced object  you wish to target
            /// </summary>
            public string InstancedObjectToTrackBlipName;
            /// <summary>
            /// the name of the instanced blip you wish to track
            /// </summary>
            public string InstancedTargetBlipname;
            /// <summary>
            /// Selection between Inverse rotation and Proportional rotation
            /// </summary>
            public Rotations rotations;
            /// <summary>
            ///  This may be a blip or any other object in scene
            /// </summary>
            public GameObject TargetedObject;
            /// <summary>
            /// the object whose rotation you wish to target
            /// </summary>
            public GameObject Target;
            /// <summary>
            /// Selection of the way in which you wish to select and object
            /// </summary>
            public TargetObject ObjectToTrack = TargetObject.ThisObject;
            /// <summary>
            /// The blip you wish to target
            /// </summary>
            public TargetBlip target = TargetBlip.ThisObject;
            /// <summary>
            /// Determining what axis value we wish to pass to another axis value
            /// </summary>
            public RetargetRotation RetargetedRotation = RetargetRotation.none;
            /// <summary>
            /// A selection between positive and negative
            /// </summary>
            public valueState ValueState = valueState.positive;
            /// <summary>
            /// this rotation value is usually used when using sprites 
            /// </summary>
            public float AddedRotation = 90;

        }
        /// <summary>
        /// Minimap Module 
        /// </summary>
        [System.Serializable]
        public class MiniMapModule
        {
            /// <summary>
            /// Choose between Realtime minimap or a stati minimap
            /// </summary>
            public MapType mapType = MapType.Realtime;
            /// <summary>
            /// Texture to be used for static minimaps
            /// </summary>
            public Sprite MapTexture;
            // public bool autogenerateMaps;
            /// <summary>
            /// Check if the map has been generated
            /// </summary>
            [NonSerialized]
            public bool generated;
            //public bool UseVector3 = true;
            /// <summary>
            /// Determine if the static minimap is being calibrated 
            /// </summary>
            public bool calibrate;
            /// <summary>
            /// the objet which will use the Map texture and Masked Material
            /// </summary>
            public GameObject Map;
            /// <summary>
            /// the object which will use the mask material
            /// </summary>
            public GameObject Mask;
            //   public GameObject CenterObject;
            /// <summary>
            /// Cashe of the SceneScale vlaue
            /// </summary>
            public float SavedSceneScale;
            /// <summary>
            /// The value set during calibrating of ststic minimap
            /// </summary>
            public float MapScale = 1;
            /// <summary>
            /// Cashe of the MapScale vlaue
            /// </summary>
            public float SavedMapScale;
            /// <summary>
            /// Determines by what rate the minmap is scales at rintime
            /// </summary>
            public float Scalingfactor;
            //public Vector3 CenterPosition;
            /// <summary>
            /// Masked material
            /// </summary>
            public Material MapMaterial;
            /// <summary>
            /// Mask Material
            /// </summary>
            public Material MaskMaterial;
            /// <summary>
            /// The layer on which the minimap will be rendered
            /// </summary>
            public LayerMask layer;
            /// <summary>
            /// the RenderTexture to be used with the realtime minimap
            /// </summary>
            public RenderTexture renderTexture;
            /// <summary>
            /// The camera reading the RenderTexture for the Minimap
            /// </summary>
            public Camera RealtimeMinimapCamera;
            /// <summary>
            /// the position of the RealtimeMinimapCamera in the Y axis
            /// </summary>
            public float CameraHeight;
            /// <summary>
            /// the order in layer of the blip
            /// </summary>
            public int OrderInLayer = -1;

            Texture2D CalculateTexture(int h, int w, float r, float cx, float cy, Texture2D sourceTex)
            {
                Color[] c = sourceTex.GetPixels(0, 0, sourceTex.width, sourceTex.height);
                Texture2D b = new Texture2D(h, w);
                for (int i = (int)(cx - r); i < cx + r; i++)
                {
                    for (int j = (int)(cy - r); j < cy + r; j++)
                    {
                        float dx = i - cx;
                        float dy = j - cy;
                        float d = Mathf.Sqrt(dx * dx + dy * dy);
                        if (d <= r)
                            b.SetPixel(i - (int)(cx - r), j - (int)(cy - r), sourceTex.GetPixel(i, j));
                        else
                            b.SetPixel(i - (int)(cx - r), j - (int)(cy - r), Color.clear);
                    }
                }
                b.Apply();
                return b;
            }

            /// <summary>
            /// generates are sprite specificially used for the mask layer of the Radar
            /// </summary>
            /// <returns></returns>
            public Sprite MaskSprite()
            {


                Sprite mask = Sprite.Create(CalculateTexture(200, 200, 100, 0.5f, 0.5f, new Texture2D(200, 200)),
        new Rect(0, 0, 200, 200),
        new Vector2(0.5f, 0.5f));
                return mask;



            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public  Mesh ProceduralMapQuad()
            {
                Mesh TempMesh = new Mesh();
                Vector3[] verts;
                int[] triangles;
                Vector3[] Normals;
                Vector2[] UVs;

                verts = new Vector3[4];
                verts[0] = new Vector3(-1, -1, 0);
                verts[1] = new Vector3(1, -1, 0);
                verts[2] = new Vector3(-1, 1, 0);
                verts[3] = new Vector3(1, 1, 0);

                triangles = new int[6];
                triangles[0] = 0;
                triangles[1] = 2;
                triangles[2] = 1;
                triangles[3] = 1;
                triangles[4] = 2;
                triangles[5] = 3;

                Normals = new Vector3[4];
                Normals[0] = -Vector3.forward;
                Normals[1] = -Vector3.forward;
                Normals[2] = -Vector3.forward;
                Normals[3] = -Vector3.forward;

                UVs = new Vector2[4];
                UVs[0] = new Vector2(0, 0);
                UVs[1] = new Vector2(1, 0);
                UVs[2] = new Vector2(0, 1);
                UVs[3] = new Vector2(1, 1);

                TempMesh.vertices = verts;
                TempMesh.triangles = triangles;
                TempMesh.normals = Normals;
                TempMesh.uv = UVs;

                return TempMesh;
            }

        }
        /// <summary>
        /// Options for optimizing  the radars functions
        /// </summary>
        [System.Serializable]
        public class OptimizationModule
        {
            /// <summary>
            ///  pool size for objects you wish to track
            /// </summary>
            public int poolSize;
            /// <summary>
            /// Determines if the blip will be using pooling
            /// </summary>
            public bool SetPoolSizeManually = false;
            /// <summary>
            /// Options for usng two diferent object finding methods
            /// </summary>
            public ObjectFindingMethod objectFindingMethod = ObjectFindingMethod.Recursive;

            /// <summary>
            /// if true , blips will be removed if the object they track has lost its original tag
            /// </summary>
            public bool RemoveBlipsOnTagChange;
            /// <summary>
            /// if true , blips will be removed if the object they track has been disabled
            /// </summary>
            public bool RemoveBlipsOnDisable;
            /// <summary>
            /// if true and you are using Recursive optimization method then you can call _3DRadar.radar3D.doInstanceObjectCheck() trigger object search;
            /// </summary>
            public bool RequireInstanceObjectCheck;

            /// <summary>
            /// By setting this to true, you can ensure that evne if your pool value at atart is greater then the actual amount of objects that can be found , your pool value will be reset to the amount of objects found ao that recusrsive searching is avoided
            /// </summary>
            public bool RecalculatePoolSizeBasedOnFirstFoundObjects;

        }



        #region 2D

        #region _2DRadarBlips Class
        /// <summary>
        /// 
        /// </summary>
        [System.Serializable]
        public class RadarBlips2D
        {
            /// <summary>
            /// Tell the blip to do a removal of blips if the Recursive optimization method is used 
            /// </summary>
            [NonSerialized]
            public bool DoRemoval = false;
            /// <summary>
            /// checks if all blips have ben instanced
            /// </summary>
            [NonSerialized]
            public bool Instanced;
            /// <summary>
            /// check if the blip is set turned on or off
            /// </summary>
            public bool IsActive;
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public bool ShowBLipSettings;
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public bool ShowSpriteBlipSettings;
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public bool ShowPrefabBlipSettings;
            /// <summary>
            ///  Determines if the blip will be tracking the rotation of its target
            /// </summary>
            public bool IsTrackRotation;
            /// <summary>
            /// Determines if th blips can scale by distance
            /// </summary>
            public bool BlipCanScleBasedOnDistance;
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public bool ShowGeneralSettings;
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public bool ShowAdditionalOptions;
            /// <summary>
            /// determines if the blip should always remeing inside the radar 
            /// </summary>
            public bool AlwaysShowBlipsInRadarSpace;
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public bool ShowOptimizationSettings;
            /// <summary>
            /// if you are using Always Show and Scale By Distance , this will ensure that you have a smooth ttansition from the moment your blip passes the Tracking Bounds to the moment is is scales to its minimaum scale
            /// </summary>
            public bool SmoothScaleTransition;
            /// <summary>
            /// The blip icon if the blip is a sprite
            /// </summary>
            public Sprite icon = new Sprite();
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public string State = "";
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public string Tag = "Untagged";
            /// <summary>
            /// The material used for the sprite blip
            /// </summary>
            public Material SpriteMaterial;
            /// <summary>
            ///  The colour of the sprite blip
            /// </summary>
            public Color colour = new Color(1F, 0.6F, 0F, 0.8F);
            /// <summary>
            /// The size of the blip
            /// </summary>
            public float BlipSize = 1;
            /// <summary>
            ///  The default minimum scale of the blip
            /// </summary>
            public const float DynamicBlipSize = 0.01f;
            /// <summary>
            /// The minimum size of the blip
            /// </summary>
            public float BlipMinSize = 0.5f;
            /// <summary>
            /// The maximum size of the blip
            /// </summary>
            public float BlipMaxSize = 1;
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public int Layer = 0;
            /// <summary>
            /// Prefab blip
            /// </summary>
            public Transform prefab;
            /// <summary>
            /// A list of the objects being tracked
            /// </summary>
            [NonSerialized]
            public List<GameObject> gos = new List<GameObject>();
            /// <summary>
            /// A list of the actual blips you see in your radar
            /// </summary>
            [NonSerialized]
            public List<Transform> RadarObjectToTrack = new List<Transform>();
            /// <summary>
            /// Determines what the blip should be created as , prefab or sprite
            /// </summary>
            public CreateBlipAs _CreateBlipAs;
            /// <summary>
            /// the order in layer of the blip
            /// </summary>
            public int OrderInLayer = 1;
            /// <summary>
            ///  Sorting layer of the sprite blip
            /// </summary>
            public SortingLayer sortingLayer;
            /// <summary>
            /// 
            /// </summary>
            [NonSerialized]
            public int ObjectCount = -1;
            /// <summary>
            /// Methods of optimizing radar performance
            /// </summary>
            public OptimizationModule optimization = new OptimizationModule();


        }

        #endregion

        #region CenterObject class
        /// <summary>
        /// 
        /// </summary>
        [System.Serializable]
        public class RadarCenterObject2D
        {
            /// <summary>
            /// checks if all blips have ben instanced
            /// </summary>
            [NonSerialized]
            public bool Instanced;
            /// <summary>
            /// check if the blip is set turned on or off
            /// </summary>
            public bool IsActive;
            /// <summary>
            /// INTERNAL USE ONLY 
            /// </summary>
            public bool ShowCenterBLipSettings;
            /// <summary>
            /// INTERNAL USE ONLY 
            /// </summary>
            public bool ShowSpriteBlipSettings;
            /// <summary>
            /// INTERNAL USE ONLY 
            /// </summary>
            public bool ShowPrefabBlipSettings;
            /// <summary>
            ///  Determines if the blip will be tracking the rotation of its target
            /// </summary>
            public bool IsTrackRotation;
            /// <summary>
            /// INTERNAL USE ONLY 
            /// </summary>
            public bool ShowGeneralSettings;
            /// <summary>
            /// Determines if the enter object or center blip should alwats be shown in th radar 
            /// </summary>
            public bool AlwaysShowCenterObject;
            /// <summary>
            /// Determines if the center object (center blip) can scale by distance
            /// </summary>
            public bool CenterObjectCanScaleByDistance;
            /// <summary>
            /// INTERNAL USE ONLY 
            /// </summary>
            public bool ShowAdditionalOptions;
            /// <summary>
            /// if you are using Always Show and Scale By Distance , this will ensure that you have a smooth ttansition from the moment your blip passes the Tracking Bounds to the moment is is scales to its minimaum scale
            /// </summary>
            public bool SmoothScaleTransition;
            /// <summary>
            /// The blip icon if the blip is a sprite
            /// </summary>
            public Sprite icon = new Sprite();
            /// <summary>
            /// The sprite which will represent the racking line
            /// </summary>
            public Sprite TrackingLine = new Sprite();
            /// <summary>
            /// prefab blip
            /// </summary>
            public Transform prefab;
            /// <summary>
            /// INTERNAL USE ONLY 
            /// </summary>
            public string State = "";
            /// <summary>
            /// INTERNAL USE ONLY 
            /// </summary>
            public string Tag = "Player";
            /// <summary>
            /// The material used for the sprite blip
            /// </summary>
            public Material SpriteMaterial;
            /// <summary>
            /// The colour of the sprite blip
            /// </summary>
            public Color colour = new Color(1F, 0.435F, 0F, 0.5F);
            /// <summary>
            ///  The size of the blip
            /// </summary>
            public float BlipSize = 1;
            /// <summary>
            ///  The default minimum scale of the blip
            /// </summary>
            public const float DynamicBlipSize = 0.01f;
            /// <summary>
            /// The minimum scale of the blip
            /// </summary>
            public float BlipMinSize = 0.5f;
            /// <summary>
            /// The maximum Size of th eblip
            /// </summary>
            public float BlipMaxSize = 1;
            /// <summary>
            /// 
            /// </summary>
            public int Layer = 0;
            /// <summary>
            /// The blip at the center of the radar 
            /// </summary>
            [NonSerialized]
            public Transform CenterBlip;
            /// <summary>
            /// The obje t being tracked
            /// </summary>
            [NonSerialized]
            public Transform CenterObject;
            /// <summary>
            /// Determines what the blip should be created as , prefab or sprite
            /// </summary>
            public CreateBlipAs _CreateBlipAs;
            /// <summary>
            /// the order in layer of the blip
            /// </summary>
            public int OrderInLayer = 1;
            /// <summary>
            ///  Sorting layer of the sprite blip
            /// </summary>
            public SortingLayer sortingLayer;
        }

        #endregion

        #region Radar Design Class
        /// <summary>
        /// 
        /// </summary>
        [System.Serializable]
        public class RadarDesign2D
        {


            /// <summary>
            /// This is the Diameter of the radar, this value will directly change the scale of the Radars child object "Designs" once UseSceneScale is false
            /// </summary>
            public float RadarDiameter = 1;
            /// <summary>
            /// This is the amound of the scene that the radar is able to 'see' in order to collect dats on things to track and display
            /// </summary>
            public float SceneScale = 100.0f;
            /// <summary>
            /// The range in which all blips can be shown in the radar
            /// </summary>
            public float TrackingBounds = 1;
            /// <summary>
            ///  The diameter of the zone at the center of the radar in which all blips will ce culled 
            /// </summary>
            public float InnerCullingZone = 0f;
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public float RadarRotationOffset = 0f;
            /// <summary>
            /// Do not replace this value
            /// </summary>
            public const float ConstantRadarRenderDistance = 4;
            /// <summary>
            /// The padding on the x and Y axis of the radar system
            /// </summary>
            public float xPadding, yPadding;
            /// <summary>
            /// Determins if the radar will ise Manual position or Snap Positioning
            /// </summary>
            public RadarPositioning radarPositioning = RadarPositioning.Snap;
            /// <summary>
            /// Determines where in scren space the radar system will be positioned
            /// </summary>
            public SnapPosition snapPosition = SnapPosition.BottomLeft;
            /// <summary>
            /// Determining what defines the forward facing position of the radar 
            /// </summary>
            public FrontIs frontIs = FrontIs.North;
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public Rect RadarRect, SnappedRect = new Rect(0, 0, 200, 200);
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public int BlipCount = 0;
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public int DesignsCount = 0;
            /// <summary>
            /// Determines if we should use the scale of the Radar "Designs" child object instead of the RadarDiameter 
            /// </summary>
            public bool UseLocalScale;
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public bool Visualize = true;
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public bool ShowDesignSection;
            /// <summary>
            /// When true, the radar ; diameter (Sale of the Radars "Designs" child object) when scales to a vlue greater or less than one 
            /// will not prompt the radar system to reposition itslf automatically to maintain a correct position in screen space
            /// 
            /// </summary>
            public bool IgnoreDiameterScale = false;
            /// <summary>
            /// Determines if the tracking bounds values will always be the same as 
            /// </summary>
            public bool LinkToTrackingBounds;
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public bool ShowRenderCameraSettings;
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public bool ShowScaleSettings;
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public bool ShowPositioningSettings;
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public bool ShowDesignLayers;
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public bool ManualCameraSetup;
            /// <summary>
            /// determines if we will be using the gameobject in the scne with the tag "Main Camera"
            /// </summary>
            public bool UseMainCamera;
            /// <summary>
            /// Determines if the radar can also be a minimap
            /// </summary>
            public bool _2DSystemsWithMinimapFunction;
            /// <summary>
            /// INTERNAL USE ONLY 
            /// </summary>
            public bool ShowMinimapSettings;
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public GameObject DesignsObject;
            /// <summary>
            /// The camera which will be the camera your player views the world through at any time 
            /// </summary>
            public Camera camera;
            /// <summary>
            /// The camera whuch will only render radar systems, (These camera are automatically created for you)
            /// </summary>
            public Camera renderingCamera;
            /// <summary>
            /// INTERNAL USE ONLY
            /// </summary>
            public string CameraTag = "MainCamera";
            /// <summary>
            /// The list of Rotation targets 
            /// </summary>
            public List<RotationTarget> RotationTargets = new List<RotationTarget>();
            /// <summary>
            /// The pan of the blips in the radar
            /// </summary>
            public Vector3 Pan = new Vector3();

            //private bool _2DSystemWithMinimap;



        }


        #endregion

        #endregion*/

using System;
using System.Collections.Generic;
using System.Linq;
using DaiMangou.ProRadarBuilder.Editor;
using UnityEngine;
using UnityEngine.Rendering;

namespace DaiMangou.ProRadarBuilder
{
    [AddComponentMenu("Tools/DaiMangou/2D Radar")]

    public class _2DRadar : MonoBehaviour
    {
        public delegate void DoInstanceObjectCheck();

        /// <summary>
        ///     trigger search for new objes for the radar to track as blips
        /// </summary>
        public static DoInstanceObjectCheck doInstanceObjectCheck;


        /// <summary>
        ///     when set to true, will trigger a search for objects to track, once completed , it will reset it false
        /// </summary>
        internal bool BeginObjectInstanceCheck = true;

        public void OnEnable()
        {
            // subscrive to the event
            doInstanceObjectCheck += InstanceObjectCheck;
        }

        public void OnDisable()
        {
            // unsubscribe from the evnet
            doInstanceObjectCheck -= InstanceObjectCheck;
        }

        public void Start()
        {
            InstanceObjectCheck();
            ReadyCheck();
            SetBlipParentObject();
        }

        /// <summary>
        ///     will trigger a search for objects to track
        /// </summary>
        private void InstanceObjectCheck()
        {
            BeginObjectInstanceCheck = true;
        }


        public void Update()
        {
            // set up our  renderingCamera and main Camera
            CameraSetup();
            // Determine the front facing direction of the radar
            Frontis();
            // manage the rotations of the radar itself
            SetRotations();
            // set the dimentions of the radar 
            SetRadarDimentions();
            // manages the positioning of the radar
            SnapAndPositioning();
            //manage the poition, rotation and scale of the center blip
            CenterObjectBlip();
            // manage the position, rotation and scale for all other blips
            CheckAndSetBlips();
            // manages the Static and Realtime minimap systems
            Minimap();
            // manages specifically target objects rotations
            RotateSpecifics();
        }

        internal void ReadyCheck()
        {
            if (transform.parent)
            {
                Debug.LogWarning("2D Radar des not need to be the child of any object.This may cause unanted effects");
            }
        }

        private void SetBlipParentObject()
        {
            RadarDesign.BlipsParentObject = transform.Find("Blips");

            if (RadarDesign.BlipsParentObject == null)
            {
                Debug.LogWarning("This version of the Radar Builder requires that an object named 'Blips' is made the child of your radar. This is done automatically when you create a new Radar");
                GameObject tempBLipsContiner = new GameObject("Blips");
                tempBLipsContiner.transform.parent = transform;
                tempBLipsContiner.transform.localPosition = Vector3.zero;
                RadarDesign.BlipsParentObject = tempBLipsContiner.transform;
                Debug.Log("Radar Builder has created a Temporary 'Blips' container object, Please create a new gameobject named 'Blips' and make it a child of your Radar gameobject");
            }
        }

        /// <summary>
        ///     here we setup main camera, this script will constantly searn for your main camera or camera you wish to find IF it
        ///     is not yet found
        ///     during th abscence of a camera , no errors will be thrown.
        ///     However you will be warned if you do not set a 'Rendering camera' for your radar.
        /// </summary>
        private void CameraSetup()
        {
            #region Setup the camera

            if (!RadarDesign.camera)
                if (!RadarDesign.ManualCameraSetup)
                    if (RadarDesign.UseMainCamera)
                        RadarDesign.camera = Camera.main;
                    else
                        try
                        {
                            RadarDesign.camera = GameObject.FindWithTag(RadarDesign.CameraTag).GetComponent<Camera>();
                        }
                        catch
                        {
                            return;
                        }
            if (RadarDesign.camera == null)
                return;

            #endregion

            #region Setup RenderCamera

            if (RadarDesign.renderingCamera)
            {
                RadarDesign.renderingCamera.transform.rotation = RadarDesign.camera.transform.rotation;
            }
            else
            {
                Debug.LogWarning(
                    "Please specify a rendering camera, Your rendering camera was created when you created this radar ");
                Debug.Break();
            }

            #endregion
        }

        /// <summary>
        ///     Here we determine which direction we wish for our radars front to be
        ///     For example, If you ser Frontis to be East then all blips moving forward through the Z axis will appear to be
        ///     moving to the east in your radar .
        ///     This makes the function very useful for various games
        /// </summary>
        private void Frontis()
        {
            #region Front is 

            switch (RadarDesign.frontIs)
            {
                case FrontIs.North:
                    RadarDesign.RadarRotationOffset = 0;
                    break;
                case FrontIs.East:
                    RadarDesign.RadarRotationOffset = 270;

                    break;
                case FrontIs.South:
                    RadarDesign.RadarRotationOffset = 180;

                    break;
                case FrontIs.West:
                    RadarDesign.RadarRotationOffset = 90;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            RadarDesign.BlipsParentObject.transform.localEulerAngles = new Vector3(RadarDesign.BlipsParentObject.transform.localEulerAngles.x, RadarDesign.BlipsParentObject.transform.localEulerAngles.y, RadarDesign.RadarRotationOffset);
            #endregion
        }

        /// <summary>
        ///     Here we set the rotation of our radar to be proportional to the rotation of the target camera
        /// </summary>
        private void SetRotations()
        {
            #region Set Rotation of radar

            transform.eulerAngles = new Vector3(
                RadarDesign.camera.transform.eulerAngles.x,
                RadarDesign.camera.transform.eulerAngles.y,
             RadarDesign.camera.transform.eulerAngles.y);

            #endregion
        }

        /// <summary>
        ///     Scale the radar lie any other object or by usign the  RadarDiameter Value when UseLocalScale is true
        /// </summary>
        private void SetRadarDimentions()
        {
            #region Setting Radar Dimentions

            transform.localScale = RadarDesign.UseLocalScale
                ? transform.localScale
                : new Vector3(RadarDesign.RadarDiameter, RadarDesign.RadarDiameter, RadarDesign.RadarDiameter);

            #endregion
        }

        /// <summary>
        ///     Snap position allows for the radar to be positioned in 9 points of the screen
        /// </summary>
        private void SnapAndPositioning()
        {
            RadarDesign.SnappedRect.size = new Vector2(DMMath.REQS(Screen.height, DMMath.Mv),
                DMMath.REQS(Screen.height, DMMath.Mv));

            #region Snap&Manual positioning

            // for every increase or decrease of 0.01 in radar diameter above or below 1 will result in  the amount of times * 0.0024096385

            var scale = transform.localScale.x;

            var autoScalingForChangeInDiameter = Mathf.Abs((1 - scale) / 0.01f) * DMMath.ScalingConstant;
            // Do not change this value

            var posOffSetByScale = RadarDesign.IgnoreDiameterScale
                ? 0
                : scale < 1
                    ? Screen.height * autoScalingForChangeInDiameter
                    : scale > 1 ? -(Screen.height * autoScalingForChangeInDiameter) : 0;


            switch (RadarDesign.radarPositioning)
            {
                case RadarPositioning.Manual:
                    RadarDesign.SnappedRect.position = new Vector2(RadarDesign.RadarRect.xMax,
                        -RadarDesign.RadarRect.yMax + Screen.height);

                    transform.position =
                        RadarDesign.renderingCamera.ScreenToWorldPoint(
                            new Vector3(RadarDesign.SnappedRect.center.x + RadarDesign.xPadding,
                                RadarDesign.SnappedRect.center.y - RadarDesign.SnappedRect.height -
                                RadarDesign.yPadding, RadarDesign2D.ConstantRadarRenderDistance));

                    break;
                case RadarPositioning.Snap:
                    switch (RadarDesign.snapPosition)
                    {
                        case SnapPosition.TopLeft:
                            RadarDesign.SnappedRect =
                                ThisScreen.ScreenRect.ToUpperLeft(RadarDesign.SnappedRect.width,
                                        RadarDesign.SnappedRect.width, -posOffSetByScale, posOffSetByScale)
                                    .AddRect(0, Screen.height - RadarDesign.SnappedRect.size.y);
                            break;
                        case SnapPosition.TopRight:
                            RadarDesign.SnappedRect =
                                ThisScreen.ScreenRect.ToUpperRight(RadarDesign.SnappedRect.width,
                                        RadarDesign.SnappedRect.width, posOffSetByScale, posOffSetByScale)
                                    .AddRect(0, Screen.height - RadarDesign.SnappedRect.size.y);
                            break;
                        case SnapPosition.BottomLeft:
                            RadarDesign.SnappedRect =
                                ThisScreen.ScreenRect.ToLowerLeft(RadarDesign.SnappedRect.width,
                                        RadarDesign.SnappedRect.width, -posOffSetByScale, -posOffSetByScale)
                                    .AddRect(0, -Screen.height + RadarDesign.SnappedRect.size.y);
                            break;
                        case SnapPosition.BottomRight:
                            RadarDesign.SnappedRect =
                                ThisScreen.ScreenRect.ToLowerRight(RadarDesign.SnappedRect.width,
                                        RadarDesign.SnappedRect.width, posOffSetByScale, -posOffSetByScale)
                                    .AddRect(0, -Screen.height + RadarDesign.SnappedRect.size.y);
                            break;
                        case SnapPosition.Center:
                            RadarDesign.SnappedRect = ThisScreen.ScreenRect.ToCenter(RadarDesign.SnappedRect.width,
                                RadarDesign.SnappedRect.width);
                            break;
                        case SnapPosition.LeftMiddle:
                            RadarDesign.SnappedRect = ThisScreen.ScreenRect.ToCenterLeft(RadarDesign.SnappedRect.width,
                                RadarDesign.SnappedRect.width, -posOffSetByScale);
                            break;
                        case SnapPosition.RightMiddle:
                            RadarDesign.SnappedRect = ThisScreen.ScreenRect.ToCenterRight(
                                RadarDesign.SnappedRect.width, RadarDesign.SnappedRect.width, posOffSetByScale);
                            break;
                        case SnapPosition.BottomMiddle:
                            RadarDesign.SnappedRect =
                                ThisScreen.ScreenRect.ToCenterBottom(RadarDesign.SnappedRect.width,
                                        RadarDesign.SnappedRect.width, 0, -posOffSetByScale)
                                    .AddRect(0, -Screen.height + RadarDesign.SnappedRect.size.y);
                            break;
                        case SnapPosition.TopMiddle:
                            RadarDesign.SnappedRect =
                                ThisScreen.ScreenRect.ToCenterTop(RadarDesign.SnappedRect.width,
                                        RadarDesign.SnappedRect.width, 0, posOffSetByScale)
                                    .AddRect(0, Screen.height - RadarDesign.SnappedRect.size.y);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    transform.localPosition = RadarDesign.renderingCamera.ScreenToWorldPoint(new Vector3(
                        RadarDesign.SnappedRect.center.x + RadarDesign.xPadding,
                        RadarDesign.SnappedRect.center.y - RadarDesign.yPadding,
                        RadarDesign2D.ConstantRadarRenderDistance));


                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            #endregion
        }

        /// <summary>
        ///     Here we create the center blip  by simply checking the blip values which you have set and then instancing a sprite
        ///     to match your settings. it simply remains in the center of your radar
        ///     if you have not set the center object to be 'Acive'  then the radar itself will be the centr object and no blip
        ///     will be shown.
        /// </summary>
        private void CenterObjectBlip()
        {
            #region CenterObject BLip

            if (_RadarCenterObject2D.IsActive)
            {


                if (!_RadarCenterObject2D.Instanced)
                {
                    // if we are creating the center object as a sprite

                    #region Sprite

                    if (_RadarCenterObject2D._CreateBlipAs == CreateBlipAs.AsSprite ||
                        _RadarCenterObject2D._CreateBlipAs == CreateBlipAs.AsMesh)
                    {
                        _RadarCenterObject2D.CenterBlip = new GameObject().transform;
                        _RadarCenterObject2D.CenterBlip.transform.parent = RadarDesign.BlipsParentObject;
                        _RadarCenterObject2D.CenterBlip.name = _RadarCenterObject2D.Tag;
                        _RadarCenterObject2D.CenterBlip.transform.position = transform.position;
                        _RadarCenterObject2D.CenterBlip.localScale = new Vector3(_RadarCenterObject2D.BlipSize,
                            _RadarCenterObject2D.BlipSize, _RadarCenterObject2D.BlipSize);
                        _RadarCenterObject2D.CenterBlip.gameObject.AddComponent<SpriteRenderer>();
                        var centerObjectSpriteRenderer = _RadarCenterObject2D.CenterBlip.GetComponent<SpriteRenderer>();
                        centerObjectSpriteRenderer.material =
                            _RadarCenterObject2D.SpriteMaterial;
                        centerObjectSpriteRenderer.color =
                            _RadarCenterObject2D.colour;
                        centerObjectSpriteRenderer.sprite =
                            _RadarCenterObject2D.icon;
                        centerObjectSpriteRenderer.sortingOrder =
                            _RadarCenterObject2D.OrderInLayer;
                        centerObjectSpriteRenderer.gameObject.layer =
                            _RadarCenterObject2D.Layer;
                        _RadarCenterObject2D.Instanced = true;
                    }

                    #endregion

                    // if we are creating the center object as a prefab

                    #region Prefab

                    if (_RadarCenterObject2D._CreateBlipAs == CreateBlipAs.AsPrefab)
                    {
                        _RadarCenterObject2D.CenterBlip = Instantiate(_RadarCenterObject2D.prefab, transform.position,
                            Quaternion.identity);
                        _RadarCenterObject2D.CenterBlip.transform.parent = RadarDesign.BlipsParentObject;
                        _RadarCenterObject2D.CenterBlip.transform.position = transform.position;
                        _RadarCenterObject2D.CenterBlip.name = _RadarCenterObject2D.Tag;
                        _RadarCenterObject2D.CenterBlip.localScale = new Vector3(_RadarCenterObject2D.BlipSize,
                            _RadarCenterObject2D.BlipSize, _RadarCenterObject2D.BlipSize);
                        _RadarCenterObject2D.CenterBlip.GetComponent<Transform>().gameObject.layer =
                            _RadarCenterObject2D.Layer;
                        _RadarCenterObject2D.Instanced = true;
                    }

                    #endregion
                }

                var centerBlipTransform = _RadarCenterObject2D.CenterBlip.transform;


                #region Set Center Object 

                if (!_RadarCenterObject2D.CenterObject ||
                    _RadarCenterObject2D.CenterObject == RadarDesign.camera.transform)
                    try
                    {
                        _RadarCenterObject2D.CenterObject =
                            GameObject.FindGameObjectWithTag(_RadarCenterObject2D.Tag).transform;
                    }
                    catch
                    {
                        // if for some reson the center object is destroyed of disable d then we fal back to using the camera as the center object
                        _RadarCenterObject2D.CenterObject = RadarDesign.camera.transform;
                    }

                #endregion

                #region calculate distance and determine action to take based on the center blip distance 

                // we calculate the distance between the blip and this transform 


                var distance = Vector3.Distance(transform.position + (RadarDesign.Pan / RadarDesign.SceneScale), transform.position);

                if (distance > RadarDesign.TrackingBounds)
                {
                    // here we check if AlwaysShowCenterObject is true. If it is, we will let the center object remain in the radar 
                    if (_RadarCenterObject2D.AlwaysShowCenterObject)
                    {
                        if (!_RadarCenterObject2D.CenterBlip.gameObject.activeInHierarchy)
                            _RadarCenterObject2D.CenterBlip.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (_RadarCenterObject2D.CenterBlip.gameObject.activeInHierarchy)
                            _RadarCenterObject2D.CenterBlip.gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (!_RadarCenterObject2D.CenterBlip.gameObject.activeInHierarchy)
                        _RadarCenterObject2D.CenterBlip.gameObject.SetActive(true);
                }

                #endregion

                #region Scale Tracking


                var workingBlipScale = _RadarCenterObject2D.CenterObjectCanScaleByDistance ? _RadarCenterObject2D.BlipMinSize : _RadarCenterObject2D.BlipSize;


                var dynamicScale =
                    (distance < RadarDesign.TrackingBounds) ?


                    _RadarCenterObject2D.CenterObjectCanScaleByDistance ?
                               Mathf.Clamp(_RadarCenterObject2D.BlipMaxSize - (_RadarCenterObject2D.BlipMaxSize * (distance / RadarDesign.TrackingBounds))
                             , _RadarCenterObject2D.BlipMinSize
                             , _RadarCenterObject2D.BlipMaxSize)

                             :
                             _RadarCenterObject2D.BlipSize
                             :
                             _RadarCenterObject2D.AlwaysShowCenterObject ?
                              Mathf.Clamp(workingBlipScale - (workingBlipScale * (distance - RadarDesign.TrackingBounds))
                                        , RadarBlips2D.DynamicBlipSize
                                        , workingBlipScale)
                                        :
                                        _RadarCenterObject2D.BlipSize
                             ;



                centerBlipTransform.localScale = new Vector3(dynamicScale, dynamicScale, dynamicScale);

                #endregion

                #region Position tracking

                //to place the center object at the center of the radar at all times and also make it use the pan offset we simply do the following 

                var centerObjectOffset = !_RadarCenterObject2D.AlwaysShowCenterObject
                    ? RadarDesign.Pan / RadarDesign.SceneScale
                    : Vector3.ClampMagnitude(RadarDesign.Pan / RadarDesign.SceneScale, RadarDesign.TrackingBounds);

                centerBlipTransform.localPosition = new Vector3(centerObjectOffset.x,
                    centerObjectOffset.z, 0);
                // = new Vector3(transform.position.x + CenterObjectOffset.x, transform.position.y + CenterObjectOffset.z, transform.position.z - 0.01f);

                #endregion

                #region Rotation tracking

                if (_RadarCenterObject2D.IsTrackRotation && _RadarCenterObject2D.IsActive)
                    centerBlipTransform.localEulerAngles = new Vector3(0, 0,
                        -_RadarCenterObject2D.CenterObject.transform.eulerAngles.y - transform.eulerAngles.z);
                else
                    centerBlipTransform.localEulerAngles = new Vector3(0, 0,
                        transform.eulerAngles.z);

                #endregion
            }
            else
            {
                if (_RadarCenterObject2D.CenterBlip != null)
                    if (_RadarCenterObject2D.CenterBlip.gameObject.activeInHierarchy)
                        _RadarCenterObject2D.CenterBlip.gameObject.SetActive(false);

                _RadarCenterObject2D.CenterObject = RadarDesign.camera.transform;
            }

            #endregion
        }

        /// <summary>
        ///     Here we go about instancing all other blips
        /// </summary>
        private void CheckAndSetBlips()
        {
            #region Check and Set Blips

            // we go through all the blip type you create and then...
            foreach (var item in Blips)
            {
                var trackedObjectCount = item.gos.Count;

                #region Removal of BLips when using Recursive Optimization 

                if (item.optimization.objectFindingMethod == ObjectFindingMethod.Recursive)
                {
                    for (var i = 0; i < trackedObjectCount; i++)
                    {
                        var currentWorkingObject = item.gos[i];


                        if (currentWorkingObject == null)
                        {
                            DestroyImmediate(item.RadarObjectToTrack[i].gameObject);
                            item.DoRemoval = true;
                        }

                        if (item.optimization.RemoveBlipsOnTagChange)
                            if (currentWorkingObject != null)
                                if (currentWorkingObject.tag != item.Tag)
                                {

                                    DestroyImmediate(item.RadarObjectToTrack[i].gameObject);
                                    item.DoRemoval = true;
                                }

                        if (item.optimization.RemoveBlipsOnDisable)
                            if (currentWorkingObject != null)
                                if (!currentWorkingObject.activeInHierarchy)
                                {

                                    DestroyImmediate(item.RadarObjectToTrack[i].gameObject);
                                    item.DoRemoval = true;
                                }
                    }

                    if (item.DoRemoval)
                    {
                        item.RadarObjectToTrack.RemoveAll(x => x == null);
                    }
                }



                #endregion


                #region Find all objects that we want to track 

                switch (item.optimization.objectFindingMethod)
                {
                    case ObjectFindingMethod.Pooling:
                        if (item.optimization.SetPoolSizeManually)
                        {
                            if (BeginObjectInstanceCheck || trackedObjectCount < item.optimization.poolSize)
                            {
                                item.gos = GameObject.FindGameObjectsWithTag(item.Tag).ToList(); //DO AT END OF FRAME !!
                                if (item.optimization.RecalculatePoolSizeBasedOnFirstFoundObjects)
                                    if (item.optimization.poolSize > trackedObjectCount)
                                        item.optimization.poolSize = trackedObjectCount;


                            }
                        }
                        else
                        {
                            if (BeginObjectInstanceCheck)
                            {
                                item.gos = GameObject.FindGameObjectsWithTag(item.Tag).ToList(); //DO AT END OF FRAME !!

                            }
                        }

                        break;
                    case ObjectFindingMethod.Recursive:
                        if (item.optimization.RequireInstanceObjectCheck)
                        {

                            if (BeginObjectInstanceCheck || item.DoRemoval)
                            {
                                item.gos = GameObject.FindGameObjectsWithTag(item.Tag).ToList(); //DO AT END OF FRAME !!

                            }
                        }
                        else
                        {

                            item.gos = GameObject.FindGameObjectsWithTag(item.Tag).ToList(); //DO AT END OF FRAME !!
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                #endregion

                #region Ensure that our tracking line count and TrackedObjects= count are always correct ONLY FOR RECURSIVE OPTIMIZATION METHOD

                if (item.DoRemoval)
                {
                    trackedObjectCount = item.gos.Count;
                    item.DoRemoval = false;
                }

                #endregion

                // if the blip type is set to active the following will execute
                if (item.IsActive)
                {
                    #region Instancing blips

                    // we will begin populating the list which will hold the blips by checking if the amount of things in the blip list is equal to the amount of theing we want to track
                    for (var a = item.RadarObjectToTrack.Count; a < trackedObjectCount; a++)
                    {
                        //  ListExt.Resize(item.RadarObjectToTrack, trackedObjectCount);

                        #region Sprites

                        if (item._CreateBlipAs == CreateBlipAs.AsSprite || item._CreateBlipAs == CreateBlipAs.AsMesh)
                        {
                            var blip = new GameObject();

                            //here we check if we need to add a pivot gameobject to the blip 

                            blip.transform.parent = RadarDesign.BlipsParentObject;
                            blip.transform.position = transform.position;
                            blip.name = item.Tag;
                            blip.AddComponent<SpriteRenderer>();
                            var blipSpriteRenderer = blip.GetComponent<SpriteRenderer>();
                            blipSpriteRenderer.material = item.SpriteMaterial;
                            blipSpriteRenderer.color = item.colour;
                            blipSpriteRenderer.sprite = item.icon;
                            blipSpriteRenderer.sortingOrder = item.OrderInLayer;
                            blip.GetComponent<Transform>().gameObject.layer = item.Layer;
                            blip.transform.localScale = new Vector3(item.BlipSize, item.BlipSize, item.BlipSize);
                            item.RadarObjectToTrack.Add(blip.transform);

                        }

                        #endregion

                        #region Prefab

                        if (item._CreateBlipAs != CreateBlipAs.AsPrefab) continue;
                        {
                            var blip = Instantiate(item.prefab, transform.position, Quaternion.identity);
                            blip.transform.parent = RadarDesign.BlipsParentObject;
                            blip.transform.localScale = new Vector3(item.BlipSize, item.BlipSize, item.BlipSize);
                            blip.transform.position = transform.position;
                            blip.name = item.Tag;
                            blip.GetComponent<Transform>().gameObject.layer = item.Layer;
                            item.RadarObjectToTrack.Add(blip.transform);
                        }

                        #endregion
                    }

                    #endregion

                    // we then sort through all the object in the gos list
                    for (var i = 0; i < trackedObjectCount; i++)
                    {
                        var currentWorkingRadarObjectToTrack = item.RadarObjectToTrack[i];
                        var currentWorkingRadarObjectToTrackGameObject = currentWorkingRadarObjectToTrack.gameObject;
                        var isRadarObjectToTrackActiveInHeirarchy =
                            currentWorkingRadarObjectToTrackGameObject.activeInHierarchy;

                        var currentWorkingObject = item.gos[i];

                        #region Removal of blips when using Pooling optimization mode

                        if (item.optimization.objectFindingMethod == ObjectFindingMethod.Pooling)
                        {
                            // if you ae using pooling then you should not be destroying your objets anyway. if you do. this annoying code will run
                            if (currentWorkingObject == null)
                            {
                                // remove and add back to the list

                                //  blips to the front of their respective lists This is a fast process

                                item.RadarObjectToTrack.Remove(currentWorkingRadarObjectToTrack);
                                item.RadarObjectToTrack.Add(currentWorkingRadarObjectToTrack);
                                foreach (var b in item.RadarObjectToTrack)
                                    b.gameObject.SetActive(false);

                                item.gos = GameObject.FindGameObjectsWithTag(item.Tag).ToList();
                                //DO AT END OF FRAME !! // dont let it come to this :( EVEN THOUGH THIS IS REALLY FASE :P
                                // in case you destroy objects that should be pooled we reset the pool size.
                                item.optimization.poolSize = item.gos.Count;




                                return;
                            }


                            if (item.optimization.RemoveBlipsOnTagChange)
                                if (currentWorkingObject.tag != item.Tag)
                                {
                                    // remove and add back to the list

                                    // blips to the front of their respective lists This is a fast process
                                    item.RadarObjectToTrack.Remove(currentWorkingRadarObjectToTrack);
                                    item.RadarObjectToTrack.Add(currentWorkingRadarObjectToTrack);
                                    foreach (var b in item.RadarObjectToTrack)
                                        b.gameObject.SetActive(false);

                                    item.gos = GameObject.FindGameObjectsWithTag(item.Tag).ToList();
                                    //DO AT END OF FRAME !! // dont let it come to this :( EVEN THOUGH THIS IS REALLY FASE :P
                                    // in case you destroy objects that should be pooled we reset the pool size.
                                    item.optimization.poolSize = item.gos.Count;



                                    return;
                                }

                            if (item.optimization.RemoveBlipsOnDisable)
                                if (!currentWorkingObject.activeInHierarchy)
                                    currentWorkingRadarObjectToTrackGameObject.SetActive(false);
                        }

                        #endregion

                        var currentWorkingObjectTransform = currentWorkingObject.transform;
                        var trackedObjectsPoition = currentWorkingObjectTransform.position;
                        var currentWorkingRadarObjectToTrackTransform = currentWorkingRadarObjectToTrack.transform;
                        var centerObjectsPosition = _RadarCenterObject2D.CenterObject.transform.position;

                        #region CheckAnd Set Distance

                        // the distance each blip is from the center of the radar is calculated now that we have access to everything we want to track
                        var distance =
                            Vector3.Distance(trackedObjectsPoition,
                                centerObjectsPosition - RadarDesign.Pan) /
                            RadarDesign.SceneScale;

                        #endregion

                        if (distance < RadarDesign.TrackingBounds && distance >= RadarDesign.InnerCullingZone)
                        {
                            #region Position Tracking

                            var trackedObjectPos = (trackedObjectsPoition -
                                                    (centerObjectsPosition -
                                                     RadarDesign.Pan)) / RadarDesign.SceneScale;
                            currentWorkingRadarObjectToTrackTransform.localPosition = new Vector3(trackedObjectPos.x,
                                (RadarDesign.TrackYPosition) ? trackedObjectPos.y : trackedObjectPos.z, -0.01f);

                            #endregion


                            #region Rotation Tracking

                            if (item.IsTrackRotation)
                                currentWorkingRadarObjectToTrackTransform.localEulerAngles = new Vector3(0, 0,
                                    -currentWorkingObjectTransform.eulerAngles.y - transform.rotation.eulerAngles.z);
                            else
                                currentWorkingRadarObjectToTrackTransform.localEulerAngles = new Vector3(0, 0,
                                    transform.rotation.eulerAngles.z);

                            #endregion

                            #region Scale Trcking

                            var currentBlipScale = item.BlipCanScleBasedOnDistance ? Mathf.Clamp(item.BlipMaxSize - (item.BlipMaxSize * (distance / RadarDesign.TrackingBounds)),
                                    item.BlipMinSize,
                                    item.BlipMaxSize) : item.BlipSize;
                            currentWorkingRadarObjectToTrackTransform.localScale = new Vector3(currentBlipScale, currentBlipScale, currentBlipScale);

                            #endregion


                            #region here we ensure that any blip that is not active must be made active

                            if (!isRadarObjectToTrackActiveInHeirarchy && currentWorkingObject.activeInHierarchy)
                                currentWorkingRadarObjectToTrackGameObject.SetActive(true);

                            #endregion
                        }

                        #region here we must disable all blips that enter the inner cullin zone or exced th tracking bounds

                        if (distance < RadarDesign.InnerCullingZone ||
                            distance > RadarDesign.TrackingBounds && !item.AlwaysShowBlipsInRadarSpace)
                            if (isRadarObjectToTrackActiveInHeirarchy)
                                currentWorkingRadarObjectToTrackGameObject.SetActive(false);

                        #endregion

                        #region Always show blips in radar space

                        if (!item.AlwaysShowBlipsInRadarSpace || !(distance >= RadarDesign.TrackingBounds)) continue;
                        {
                            #region Position Tracking

                            var trackedObjectPos = (trackedObjectsPoition -
                                                    (centerObjectsPosition -
                                                     RadarDesign.Pan)) / RadarDesign.SceneScale;
                            var offset = new Vector3(trackedObjectPos.x, trackedObjectPos.z, -0.01f);
                            currentWorkingRadarObjectToTrackTransform.localPosition = Vector3.ClampMagnitude(offset,
                                RadarDesign.TrackingBounds);

                            #endregion

                            #region Rotation Tracking

                            if (item.IsTrackRotation)
                                currentWorkingRadarObjectToTrackTransform.localEulerAngles = new Vector3(0, 0,
                                    -currentWorkingObjectTransform.eulerAngles.y - transform.rotation.eulerAngles.z);
                            else
                                currentWorkingRadarObjectToTrackTransform.localEulerAngles = new Vector3(0, 0,
                                    transform.rotation.eulerAngles.z);

                            #endregion

                            #region Re-Scale Tracking

                            if (distance > RadarDesign.TrackingBounds)
                            {

                                var workingBlipScale = item.BlipCanScleBasedOnDistance ? item.BlipMinSize : item.BlipSize;

                                var dynamicScale =
                                               Mathf.Clamp(workingBlipScale - (workingBlipScale * (distance - RadarDesign.TrackingBounds))
                                             , RadarBlips2D.DynamicBlipSize
                                             , workingBlipScale);

                                currentWorkingRadarObjectToTrackTransform.localScale = new Vector3(dynamicScale,
                                    dynamicScale,
                                    dynamicScale);
                            }

                            #endregion

                            // here we ensure that any blip outside of the tracking bounds must be enabled
                            if (!isRadarObjectToTrackActiveInHeirarchy && currentWorkingObject.activeInHierarchy)
                                currentWorkingRadarObjectToTrackGameObject.SetActive(true);
                        }

                        #endregion
                    }
                }
                else
                {
                    #region turn off all blips if blip type is set to be inactive

                    if (item.RadarObjectToTrack.Count != 0)
                    {
                        for (var i = 0; i < trackedObjectCount; i++)
                        {
                            item.RadarObjectToTrack[i].gameObject.SetActive(false);
                        }
                    }

                    #endregion
                }
            }

            #endregion

            if (BeginObjectInstanceCheck) BeginObjectInstanceCheck = false;
        }

        /// <summary>
        ///     here we set up and reposition the minimap
        /// </summary>
        private void Minimap()
        {
            if (!RadarDesign._2DSystemsWithMinimapFunction) return;

            #region creationg the minimap

            if (!minimapModule.generated)
            {
                var miniMap = new GameObject("MiniMap");
                miniMap.transform.localScale = transform.localScale;
                miniMap.transform.parent = transform;
                miniMap.transform.localPosition = Vector3.zero;
                var mapPivot = new GameObject("map Pivot");
                mapPivot.transform.parent = miniMap.transform;
                minimapModule.MapPivot = mapPivot;
                mapPivot.transform.localPosition = Vector3.zero;
                var map = new GameObject("map");
                map.transform.parent = mapPivot.transform;
                map.transform.localPosition = Vector3.zero;
                map.transform.localScale = Vector3.one;
                var mask = new GameObject("Minimap Mask");
                mask.transform.parent = miniMap.transform;
                minimapModule.Mask = mask;
                mask.AddComponent<SpriteRenderer>();
                mask.GetComponent<SpriteRenderer>().sprite = (minimapModule.UseCustomMapMaskShape && minimapModule.CustomMapMaskShape != null) ? minimapModule.CustomMapMaskShape : minimapModule.MaskSprite();
                mask.GetComponent<SpriteRenderer>().sortingOrder = minimapModule.OrderInLayer;
                mask.GetComponent<SpriteRenderer>().sharedMaterial = minimapModule.MaskMaterial;
                mask.transform.localScale = Vector3.one;
                mask.transform.localPosition = new Vector3(0, 0, 0.01f);
                mask.layer = minimapModule.layer;
                minimapModule.Map = map;

                if (minimapModule.mapType == MapType.Realtime)
                {
                    map.AddComponent<MeshFilter>().sharedMesh = minimapModule.ProceduralMapQuad();
                    // create a new masked material on the fly so that we dont overwrite any preexisting matrials in the project
                    // and use it as the map material
                    var mapRenderTextureMaterial = new Material(minimapModule.MapMaterial.shader)
                    {
                        mainTexture = minimapModule.renderTexture
                    };
                    // use the render texture as the main texture
                    // Add a mesh renderer and set its shared materia lto be the MapRenderTextureMaterial
                    var mapRenderer = map.AddComponent<MeshRenderer>();
                    mapRenderer.sharedMaterial = mapRenderTextureMaterial;
                    mapRenderer.receiveShadows = false;
                    mapRenderer.lightProbeUsage = LightProbeUsage.Off;
                    mapRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
                    mapRenderer.sortingOrder = minimapModule.OrderInLayer;
                    // Map.AddComponent<RT2S>();
                    //  Map.GetComponent<RT2S>().renderTexture = minimapModule.renderTexture;
                }
                else
                {
                    map.AddComponent<SpriteRenderer>();
                    map.GetComponent<SpriteRenderer>().sprite = minimapModule.MapTexture;
                    map.GetComponent<SpriteRenderer>().sharedMaterial = minimapModule.MapMaterial;
                    map.GetComponent<SpriteRenderer>().sortingOrder = minimapModule.OrderInLayer;
                }
                map.layer = minimapModule.layer;
                minimapModule.SavedSceneScale = RadarDesign.SceneScale;
                minimapModule.SavedMapScale = minimapModule.MapScale;
                if (!minimapModule.calibrate)
                    minimapModule.Scalingfactor = minimapModule.SavedMapScale / minimapModule.SavedSceneScale;
                minimapModule.generated = true;
                miniMap.transform.localEulerAngles = new Vector3(0, 0, 0);
            }

            #endregion

            #region Map Positioning

            if (minimapModule.mapType == MapType.Static)
            {
                var trackedObjectPos = -(_RadarCenterObject2D.CenterObject.transform.position - RadarDesign.Pan) /
                                       RadarDesign.SceneScale;

                minimapModule.Map.transform.localPosition = new Vector3(trackedObjectPos.x, trackedObjectPos.z,
                    -0.01f);
            }
            else
            {
                var trackedObjectPos = _RadarCenterObject2D.CenterObject.transform.position - RadarDesign.Pan;
                minimapModule.RealtimeMinimapCamera.transform.position = new Vector3(
                    trackedObjectPos.x,
                    trackedObjectPos.y + minimapModule.CameraHeight,
                    trackedObjectPos.z);
            }

            #endregion

            #region Map Scale

            if (minimapModule.calibrate) return;
            if (minimapModule.mapType == MapType.Static)
            {
                var scaleOffset = RadarDesign.SceneScale - minimapModule.SavedSceneScale;

                var scale = minimapModule.SavedMapScale -
                            scaleOffset * minimapModule.Scalingfactor /
                            (RadarDesign.SceneScale / minimapModule.SavedSceneScale);
                minimapModule.Map.transform.localScale = new Vector3(scale, scale, scale);
            }
            else
            {
                minimapModule.RealtimeMinimapCamera.orthographicSize = RadarDesign.SceneScale;
            }

            #endregion


            #region Map rotation
            minimapModule.MapPivot.transform.localEulerAngles = new Vector3(0, 0, RadarDesign.RadarRotationOffset);
            #endregion
        }

        /// <summary>
        ///     Here we determine what 'Design layers' are to be rotates and what object rotation they will match proportionally |
        ///     inversely and with a damping
        /// </summary>
        private void RotateSpecifics()
        {
            #region Rotating specific design layers

            foreach (var rotationTarget in RadarDesign.RotationTargets)
            {
                if (!rotationTarget.Target)
                    switch (rotationTarget.ObjectToTrack)
                    {
                        case TargetObject.FindObject:
                            try
                            {
                                if (rotationTarget.FindingName.Length > 0)
                                    rotationTarget.Target = GameObject.Find(rotationTarget.FindingName);
                            }
                            catch
                            {
                                // ignored
                            }
                            break;
                        case TargetObject.ObectWithTag:
                            try
                            {
                                if (rotationTarget.tag.Length > 0)
                                    rotationTarget.Target = GameObject.FindWithTag(rotationTarget.tag);
                            }
                            catch
                            {
                                // ignored
                            }
                            break;
                        case TargetObject.InstancedBlip:
                            try
                            {
                                if (rotationTarget.InstancedObjectToTrackBlipName.Length > 0)
                                    rotationTarget.Target =
                                        RadarDesign.BlipsParentObject.transform.Find(rotationTarget.InstancedObjectToTrackBlipName).gameObject;
                            }
                            catch
                            {
                                // ignored
                            }
                            break;
                        case TargetObject.ThisObject:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                if (!rotationTarget.TargetedObject)
                    switch (rotationTarget.target)
                    {
                        case TargetBlip.InstancedBlip:
                            try
                            {
                                if (rotationTarget.InstancedTargetBlipname.Length > 0)
                                    rotationTarget.TargetedObject =
                                       RadarDesign.BlipsParentObject.transform.Find(rotationTarget.InstancedTargetBlipname).gameObject;
                            }
                            catch
                            {
                                // ignored
                            }
                            break;
                        case TargetBlip.ThisObject:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                if (rotationTarget.RotationDamping == 0) return;

                if (rotationTarget.TargetedObject && rotationTarget.Target)
                    rotationTarget.TargetedObject.transform.localEulerAngles = new Vector3(
                        0,
                        0,
                        (rotationTarget.UseY
                             ? (rotationTarget.rotations == Rotations.Proportional
                                 ? rotationTarget.Target.transform.eulerAngles.y
                                 : -rotationTarget.Target.transform.eulerAngles.y)
                             : (rotationTarget.rotations == Rotations.Proportional
                                 ? rotationTarget.Target.transform.eulerAngles.z
                                 : -rotationTarget.Target.transform.eulerAngles.z)
                         ) / rotationTarget.RotationDamping);
            }

            #endregion
        }

        #region variables (View source code listed aove for reference to these 4 classes, if you wish to replave these variables you will have to rename the classes to prevent clashing )

        /// <summary>
        /// The object being displayed at the center of your radar
        /// </summary>
        [HideInInspector]
        public RadarCenterObject2D _RadarCenterObject2D;

        /// <summary>
        /// all other radar blips 
        /// </summary>
        [HideInInspector]
        public List<RadarBlips2D> Blips = new List<RadarBlips2D>();

        /// <summary>
        /// Radar Design area 
        /// </summary>
        [HideInInspector]
        public RadarDesign2D RadarDesign;

        /// <summary>
        /// Minimap modeule
        /// </summary>
        [HideInInspector]
        public MiniMapModule minimapModule = new MiniMapModule();

        #endregion
    }
}
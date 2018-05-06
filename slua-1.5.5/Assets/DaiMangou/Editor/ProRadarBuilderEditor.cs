/// <summary>
/// Developed by DaiMangou Games 
/// programmer and developer : Ruchmair Dixon
/// Copytight 2017 DaiMangou Limited
/// </summary>

using UnityEngine;
using UnityEditor;
using DaiMangou.ProRadarBuilder.Editor;
using DaiMangou.ProRadarBuilderEditor;
using System;
using System.Linq;
using DaiMangou.ProRadarBuilderEditor.ResourceManagement;

namespace DaiMangou.ProRadarBuilder
{

    [System.Serializable]

    public class ProRadarBuilderEditor : EditorWindow
    {

        [MenuItem("Tools/DaiMangou/Pro Radar Builder")]
        private static void Init()
        {
            EditorWindow win = GetWindow(typeof(ProRadarBuilderEditor));
            win.minSize = new Vector2(600, 260);
        }

        #region Variables
        [SerializeField]
        private _2DRadar Current_Selected2D_Radar;
        [SerializeField]
        private _3DRadar Current_Selected3D_Radar;
        [SerializeField]
        private int Tab_Selection = 0;
        private GameObject _Selection()
        {
            try { return Selection.activeGameObject; }
            catch { return null; }

        }
        private Vector2 RadarBlipScrollPosition, DesignsScrollPosition;
        private GameObject RadarObject;
        private bool ShowHelpMessages;
        private Texture2D BackgroundImage;
        [SerializeField, HideInInspector]
        private Texture2D
            On,
            Off,
            Active2D,
            Inactive2D,
            Active3D,
            Inactive3D,
            Optimizeicon;


        private bool _2DSystem;

        private bool _3DSystem;

        private bool ShowContactArea;
        private string emailAddress, Subject, Message;



        #endregion


        public void OnEnable()
        {

            if (Application.HasProLicense())
                this.titleContent.image = GetResource.LoadDllImageResource(typeof(GetResource), "GeneralImageResources", "RadarIconlight", "png");
            else
                this.titleContent.image = GetResource.LoadDllImageResource(typeof(GetResource), "GeneralImageResources", "RadarIconDark", "png");


            this.titleContent.text = "Pro Radar";

            BackgroundImage = GetResource.LoadDllImageResource(typeof(GetResource),"GeneralImageResources", "RadarBuilderLogo", "png");

            On = Application.HasProLicense() ? GetResource.LoadDllImageResource(typeof(GetResource), "GeneralImageResources", "PowerOnpro", "png") : GetResource.LoadDllImageResource(typeof(GetResource), "GeneralImageResources", "PowerOn", "png");
            Off = Application.HasProLicense() ? GetResource.LoadDllImageResource(typeof(GetResource), "GeneralImageResources", "PowerOffpro", "png") : GetResource.LoadDllImageResource(typeof(GetResource), "GeneralImageResources", "PowerOff", "png");
            Active2D = GetResource.LoadDllImageResource(typeof(GetResource), "GeneralImageResources", "2D_icon_Active", "png");
            Inactive2D = GetResource.LoadDllImageResource(typeof(GetResource), "GeneralImageResources", "2D_icon_Inactive", "png");
            Active3D = GetResource.LoadDllImageResource(typeof(GetResource), "GeneralImageResources", "3D_icon_Active", "png");
            Inactive3D = GetResource.LoadDllImageResource(typeof(GetResource), "GeneralImageResources", "3D_icon_Inactive", "png");
            Optimizeicon = GetResource.LoadDllImageResource(typeof(GetResource), "GeneralImageResources", "optimizeIcon", "png");


        }


        #region Creation Area
        void CreationArea()
        {


            if (!ShowContactArea)
            {


                if (!RadarObject) EditorGUILayout.HelpBox("There is no Radar selected , you will need to add one or select an existing radar. Thanks a bunch", MessageType.Info);

                GUILayout.BeginHorizontal();
                GUILayout.Box(BackgroundImage, EditorStyles.label, GUILayout.Width(BackgroundImage.width), GUILayout.Height(BackgroundImage.height));

                GUILayout.EndHorizontal();
                Separator();
                EditorGUILayout.Separator();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button((_2DSystem) ? Active2D : Inactive2D, EditorStyles.label, GUILayout.Width(50)))
                {

                    _2DSystem = !_2DSystem;
                }
                cursorchange();

                GUILayout.Label("Screen Space Systems");

                GUILayout.EndHorizontal();
                Separator();
                EditorGUILayout.Separator();


                GUILayout.BeginHorizontal();
                if (GUILayout.Button((_3DSystem) ? Active3D : Inactive3D, EditorStyles.label, GUILayout.Width(50)))
                {
                    _3DSystem = !_3DSystem;
                }
                cursorchange();

                GUILayout.Label("World & Screen Space Systems");
                GUILayout.EndHorizontal();
                Separator();
                EditorGUILayout.Separator();

                if (GUI.Button(GUILayoutUtility.GetLastRect().ToLowerRight(80, 15, 0, 20), "Create"))
                {
                    if (_2DSystem)
                    {
                        Create2DRadar();
                        _2DSystem = false;
                    }

                    if (_3DSystem)
                    {
                        Create3DRadar();
                        _3DSystem = false;
                    }
                    EditorWindow.focusedWindow.Repaint();
                }





                #region Contact and info section

                GUILayout.BeginHorizontal();
                var emailaddres = "daimangou@gmail.com";
                GUILayout.Label("Contact:", GUILayout.Width(100));
                EditorGUILayout.TextArea(emailaddres, GUILayout.Width(220));


                if (GUILayout.Button("TUTORIALS", EditorStyles.label, GUILayout.Width(80)))
                {
                    Application.OpenURL("https://www.youtube.com/watch?v=_qpWRevZe7E&list=PLn9dlpX8Ym8J9OiddiNKSIWYM0vzZnmDH&index=2");
                }
                cursorchange();

                GUILayout.EndHorizontal();

                #endregion


            }
            else
            {


                GUILayout.BeginHorizontal();
                GUILayout.Label("Email :");
                emailAddress = GUILayout.TextArea(emailAddress, GUILayout.Width(200));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Subject :");
                Subject = GUILayout.TextArea(Subject, GUILayout.Width(200));
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical();
                GUILayout.Label("Message :");
                Message = GUILayout.TextArea(Message, GUILayout.Height(150));
                GUILayout.EndVertical();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Send", GUILayout.Width(80)))
                {
                    SendMail.Send(emailAddress, Subject, Message);
                }

                if (GUILayout.Button("Cancel", GUILayout.Width(80)))
                {
                    ShowContactArea = false;
                }
                GUILayout.EndHorizontal();
            }
        }
        #endregion


        #region IMGUI Interfce
        public void OnGUI()
        {

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Designs", EditorStyles.toolbarButton, new GUILayoutOption[] { GUILayout.Width(50) }))
            {
                Tab_Selection = 0;
            }
            if (GUILayout.Button("Blips", EditorStyles.toolbarButton, new GUILayoutOption[] { GUILayout.Width(50) }))
            {
                Tab_Selection = 1;
            }
            if (GUILayout.Button("Create", EditorStyles.toolbarButton, new GUILayoutOption[] { GUILayout.Width(50) }))
            {
                Tab_Selection = 2;

            }
            if (Tab_Selection == 1)
            {
                if (RadarObject)
                {
                    if (RadarObject.GetComponent<_2DRadar>() != null)
                    {
                        Current_Selected2D_Radar = RadarObject.GetComponent<_2DRadar>();

                        Current_Selected2D_Radar.RadarDesign.TrackYPosition = GUILayout.Toggle(Current_Selected2D_Radar.RadarDesign.TrackYPosition, "Track Y Position", EditorStyles.toolbarButton, new GUILayoutOption[] { GUILayout.Width(150) });
                    }
                }

            }

            GUILayout.Box(" ", EditorStyles.toolbarButton, GUILayout.MaxWidth(Screen.width));
            if (RadarObject)
            {
                if (RadarObject.GetComponent<_2DRadar>() != null)
                {
                    Current_Selected2D_Radar = RadarObject.GetComponent<_2DRadar>();
Current_Selected2D_Radar.RadarDesign._2DSystemsWithMinimapFunction = GUILayout.Toggle(Current_Selected2D_Radar.RadarDesign._2DSystemsWithMinimapFunction, "Minimap", EditorStyles.toolbarButton, new GUILayoutOption[] { GUILayout.Width(50) });

                }
                if (RadarObject && RadarObject.GetComponent<_3DRadar>() != null)
                {

                    Current_Selected3D_Radar = RadarObject.GetComponent<_3DRadar>();

                    Current_Selected3D_Radar.RadarDesign._3DSystemsWithScreenSpaceFunction = GUILayout.Toggle(Current_Selected3D_Radar.RadarDesign._3DSystemsWithScreenSpaceFunction, "Screen Space", EditorStyles.toolbarButton, new GUILayoutOption[] { GUILayout.Width(100) });

                    Current_Selected3D_Radar.RadarDesign._3DSystemsWithMinimapFunction = GUILayout.Toggle(Current_Selected3D_Radar.RadarDesign._3DSystemsWithMinimapFunction, "Minimap", EditorStyles.toolbarButton, new GUILayoutOption[] { GUILayout.Width(50) });


                }
            }

            GUILayout.EndHorizontal();
            #region Undo
            if (Current_Selected2D_Radar != null)
            {
                Undo.RecordObject(Current_Selected2D_Radar, "vala");
                EditorUtility.SetDirty(Current_Selected2D_Radar);
            }
            if (Current_Selected3D_Radar != null)
            {
                Undo.RecordObject(Current_Selected3D_Radar, "valb");
                EditorUtility.SetDirty(Current_Selected3D_Radar);
            }

            if (Event.current.type == EventType.ValidateCommand)
            {
                switch (Event.current.commandName)
                {
                    case "UndoRedoPerformed":

                        break;
                }
            }
            #endregion

            #region Call CreationArea
            if (RadarObject == null)
            {

                CreationArea();
            }
            #endregion

            #region 2D UI

            if (RadarObject && RadarObject.GetComponent<_2DRadar>() != null)
            {
                Current_Selected2D_Radar = RadarObject.GetComponent<_2DRadar>();


                switch (Tab_Selection)
                {
                    #region Radar Design and Settings
                    case 0:



                        DesignsScrollPosition = GUILayout.BeginScrollView(DesignsScrollPosition, false, false);
                        EditorGUILayout.Space();


                        #region Camera Section

                        HelpMessage("Here you set up your rendering camera and main camera");

                        Current_Selected2D_Radar.RadarDesign.ShowRenderCameraSettings = EditorGUILayout.Foldout(Current_Selected2D_Radar.RadarDesign.ShowRenderCameraSettings, "Camera Settings");

                        if (Current_Selected2D_Radar.RadarDesign.ShowRenderCameraSettings)
                        {
                            HelpMessage("This is the camers in front of which the radar will be displayed");

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Manual Camera Setup");
                            Current_Selected2D_Radar.RadarDesign.ManualCameraSetup = GUILayout.Toggle(Current_Selected2D_Radar.RadarDesign.ManualCameraSetup, "", GUILayout.Width(220));
                            GUILayout.EndHorizontal();

                            if (Current_Selected2D_Radar.RadarDesign.ManualCameraSetup)
                            {
                                HelpMessage("When selected , the scale of the radar once greater than or less than 1 ; will be ignored");

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Camera");
                                Current_Selected2D_Radar.RadarDesign.camera = (Camera)EditorGUILayout.ObjectField("", Current_Selected2D_Radar.RadarDesign.camera, typeof(Camera), true, GUILayout.Width(220));
                                GUILayout.EndHorizontal();
                            }
                            else
                            {
                                HelpMessage("Will always find and set the scenes Main Camera as the efault Camera  for the radar");

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Always use main camera");
                                Current_Selected2D_Radar.RadarDesign.UseMainCamera = GUILayout.Toggle(Current_Selected2D_Radar.RadarDesign.UseMainCamera, "", GUILayout.Width(220));
                                GUILayout.EndHorizontal();

                                HelpMessage("Finds a camera with the selected tag and uses it as the radars camera");

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Use camera with tag");
                                Current_Selected2D_Radar.RadarDesign.CameraTag = EditorGUILayout.TagField("", Current_Selected2D_Radar.RadarDesign.CameraTag, GUILayout.Width(220));
                                GUILayout.EndHorizontal();
                            }

                            HelpMessage("YOU MUST HAVE A RENDERING CAMERA");
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Rendering Camera");
                            Current_Selected2D_Radar.RadarDesign.renderingCamera = (Camera)EditorGUILayout.ObjectField("", Current_Selected2D_Radar.RadarDesign.renderingCamera, typeof(Camera), true, GUILayout.Width(220));
                            GUILayout.EndHorizontal();


                        }

                        Separator();

                        EditorGUILayout.Space();

                        #endregion

                        #region Minimap Settings
                        if (Current_Selected2D_Radar.RadarDesign._2DSystemsWithMinimapFunction)
                        {
                            GUILayout.BeginHorizontal();
                            Current_Selected2D_Radar.RadarDesign.ShowMinimapSettings = EditorGUILayout.Foldout(Current_Selected2D_Radar.RadarDesign.ShowMinimapSettings, "Minimap Settings");
                            GUILayout.Space(300);
                            GUILayout.EndHorizontal();
                            if (Current_Selected2D_Radar.RadarDesign.ShowMinimapSettings)
                            {
                                if (Current_Selected2D_Radar.RadarDesign._2DSystemsWithMinimapFunction)
                                {
                                    HelpMessage("When set to Realtime, the minimap texture will be drawn from the curent scene . If static then a your predesigned map will be used");
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Minimap Type");
                                    Current_Selected2D_Radar.minimapModule.mapType = (MapType)EditorGUILayout.EnumPopup(Current_Selected2D_Radar.minimapModule.mapType, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();

                                    if (Current_Selected2D_Radar.minimapModule.mapType != MapType.Realtime)
                                    {
                                        HelpMessage("The static texture2D mage that will be your map");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Minimap Texture");
                                        Current_Selected2D_Radar.minimapModule.MapTexture = (Sprite)EditorGUILayout.ObjectField(Current_Selected2D_Radar.minimapModule.MapTexture, typeof(Sprite), false, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        HelpMessage("Sets the ratio which the radars internal system will use to ensue consistncy in your minimap");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Calibrate Minimap");
                                        Current_Selected2D_Radar.minimapModule.calibrate = EditorGUILayout.Toggle("", Current_Selected2D_Radar.minimapModule.calibrate, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        HelpMessage("The order of the map sprite in the layer");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Order In layer");
                                        Current_Selected2D_Radar.minimapModule.OrderInLayer = EditorGUILayout.IntField(Current_Selected2D_Radar.minimapModule.OrderInLayer, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                    }
                                    else
                                    {
                                        HelpMessage("The render texture which will be used to pass the data from Realtime Minimap Camera to the Map");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Render Texture");
                                        Current_Selected2D_Radar.minimapModule.renderTexture = (RenderTexture)EditorGUILayout.ObjectField(Current_Selected2D_Radar.minimapModule.renderTexture, typeof(RenderTexture), true, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();


                                        if (!Current_Selected2D_Radar.minimapModule.RealtimeMinimapCamera)
                                        {
                                            if (GUILayout.Button("Create Minimap Camera", GUILayout.Width(220)))
                                            {
                                                Current_Selected2D_Radar.minimapModule.RealtimeMinimapCamera = CreateRealtimeMinimapCamera(typeof(_2DRadar));

                                            }

                                        }
                                        else
                                        {
                                            HelpMessage("Your minimap camera ");
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Minimap Camera");
                                            Current_Selected2D_Radar.minimapModule.RealtimeMinimapCamera = (Camera)EditorGUILayout.ObjectField(Current_Selected2D_Radar.minimapModule.RealtimeMinimapCamera, typeof(Camera), true, GUILayout.Width(220));
                                            GUILayout.EndHorizontal();
                                        }

                                        HelpMessage("The y position of your Realtime Minimap Camera");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Camera Height");
                                        Current_Selected2D_Radar.minimapModule.CameraHeight = EditorGUILayout.FloatField(Current_Selected2D_Radar.minimapModule.CameraHeight, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();


                                    }

                                    HelpMessage("Create radars of any shape using sprites, use 100 x 100 sprites and turn on Ignore diametere scale and ensure that your blips use a material that can be masked like our example 'Masked' material");
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Use Custom Mask Sprite Shape");
                                    Current_Selected2D_Radar.minimapModule.UseCustomMapMaskShape = EditorGUILayout.Toggle("", Current_Selected2D_Radar.minimapModule.UseCustomMapMaskShape, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();

                                    if (Current_Selected2D_Radar.minimapModule.UseCustomMapMaskShape)
                                    {
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("custom Mask sprite shape");
                                        Current_Selected2D_Radar.minimapModule.CustomMapMaskShape = (Sprite)EditorGUILayout.ObjectField(Current_Selected2D_Radar.minimapModule.CustomMapMaskShape, typeof(Sprite), false, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                    }

                                    HelpMessage("the Material to be place on the Minimap, this must be a material must be able to be masked a,d its shader must allow for Texture images");
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Map Material");
                                    Current_Selected2D_Radar.minimapModule.MapMaterial = (Material)EditorGUILayout.ObjectField(Current_Selected2D_Radar.minimapModule.MapMaterial, typeof(Material), false, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();

                                    HelpMessage("The material that will Mask the Map  in a circle");
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Mask Material");
                                    Current_Selected2D_Radar.minimapModule.MaskMaterial = (Material)EditorGUILayout.ObjectField(Current_Selected2D_Radar.minimapModule.MaskMaterial, typeof(Material), false, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();


                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("On Layer"); Current_Selected2D_Radar.minimapModule.layer = EditorGUILayout.LayerField(Current_Selected2D_Radar.minimapModule.layer, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();


                                    if (Current_Selected2D_Radar.minimapModule.calibrate && Current_Selected2D_Radar.minimapModule.mapType == MapType.Static)
                                    {


                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Map Scale");
                                        Current_Selected2D_Radar.minimapModule.MapScale = EditorGUILayout.FloatField("", Current_Selected2D_Radar.minimapModule.MapScale, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();
                                    }

                                }

                            }
                            Separator();
                            EditorGUILayout.Space();
                        }
                        #endregion

                        #region Scale Settings
                        HelpMessage("The scale setting of your radar");

                        Current_Selected2D_Radar.RadarDesign.ShowScaleSettings = EditorGUILayout.Foldout(Current_Selected2D_Radar.RadarDesign.ShowScaleSettings, "Scale");

                        if (Current_Selected2D_Radar.RadarDesign.ShowScaleSettings)
                        {
                            HelpMessage("This will override the Radar Diameter value to make the radar be set to the default scale of the DESIGNS child objects of the radar");

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Use Local Scale");
                            Current_Selected2D_Radar.RadarDesign.UseLocalScale = GUILayout.Toggle(Current_Selected2D_Radar.RadarDesign.UseLocalScale, "", GUILayout.Width(220));
                            GUILayout.EndHorizontal();

                            if (!Current_Selected2D_Radar.RadarDesign.UseLocalScale)
                            {
                                HelpMessage("Radar Diameter is the diameter of the radar");

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Radar Diameter");
                                GUILayout.Space(110);
                                Current_Selected2D_Radar.RadarDesign.RadarDiameter = EditorGUILayout.FloatField(" ", Mathf.Clamp(Current_Selected2D_Radar.RadarDesign.RadarDiameter, 0, Mathf.Infinity), GUILayout.MaxWidth(Screen.width));
                                GUILayout.EndHorizontal();
                            }

                            HelpMessage("Anything outside of the tracking bounds will not be seen");

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Tracking Bounds");
                            GUILayout.Space(106);
                            Current_Selected2D_Radar.RadarDesign.TrackingBounds = EditorGUILayout.FloatField(" ", Mathf.Clamp(Current_Selected2D_Radar.RadarDesign.TrackingBounds, 0, Mathf.Infinity), GUILayout.MaxWidth(Screen.width));
                            GUILayout.EndHorizontal();







                            HelpMessage("When selected , the scale of the radar once greater than or less than 1 ; will be ignored");

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Ignore Diameter Scale");
                            Current_Selected2D_Radar.RadarDesign.IgnoreDiameterScale = GUILayout.Toggle(Current_Selected2D_Radar.RadarDesign.IgnoreDiameterScale, "", GUILayout.Width(220));
                            GUILayout.EndHorizontal();


                            HelpMessage("Scene Scale is the zoom of the radar or how much units of space the radar can 'read'");

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Scene Scale");
                            GUILayout.Space(132);
                            Current_Selected2D_Radar.RadarDesign.SceneScale = EditorGUILayout.FloatField(" ", Mathf.Clamp(Current_Selected2D_Radar.RadarDesign.SceneScale, 1, Mathf.Infinity), GUILayout.MaxWidth(Screen.width));
                            GUILayout.EndHorizontal();

                            HelpMessage("Anything inside this area will be culled(not seen)");

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Inner Culling Zone");
                            GUILayout.Space(95);
                            Current_Selected2D_Radar.RadarDesign.InnerCullingZone = EditorGUILayout.FloatField(" ", Mathf.Clamp(Current_Selected2D_Radar.RadarDesign.InnerCullingZone, 0, Current_Selected2D_Radar.RadarDesign.TrackingBounds), GUILayout.MaxWidth(Screen.width));
                            GUILayout.EndHorizontal();
                        }
                        Separator();

                        EditorGUILayout.Space();
                        #endregion

                        #region Positioning  Section
                        HelpMessage("Position setting of your radar in screen space");

                        Current_Selected2D_Radar.RadarDesign.ShowPositioningSettings = EditorGUILayout.Foldout(Current_Selected2D_Radar.RadarDesign.ShowPositioningSettings, "Position");
                        if (Current_Selected2D_Radar.RadarDesign.ShowPositioningSettings)
                        {

                            #region Positioning settings
                            HelpMessage("Choose between 9 point snapping or Manual positioning");

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Radar Positioning");
                            Current_Selected2D_Radar.RadarDesign.radarPositioning = (RadarPositioning)EditorGUILayout.EnumPopup("", Current_Selected2D_Radar.RadarDesign.radarPositioning, GUILayout.Width(220));
                            GUILayout.EndHorizontal();

                            switch (Current_Selected2D_Radar.RadarDesign.radarPositioning)
                            {
                                case RadarPositioning.Manual:
                                    HelpMessage("Position the radar manually on the x and y axis");

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("X and Y position");
                                    Current_Selected2D_Radar.RadarDesign.RadarRect.position = EditorGUILayout.Vector2Field("", Current_Selected2D_Radar.RadarDesign.RadarRect.position, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();

                                    break;
                                case RadarPositioning.Snap:
                                    HelpMessage("Use our 9 point snapping  to snap the position of your radar to 9 dirent points on your screen");

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Snap to the");
                                    Current_Selected2D_Radar.RadarDesign.snapPosition = (SnapPosition)EditorGUILayout.EnumPopup("", Current_Selected2D_Radar.RadarDesign.snapPosition, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();
                                    break;

                            }
                            #endregion

                            #region FrontIs Settings
                            HelpMessage("Determine what the front facing direction of the radar is ");

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Front is");
                            Current_Selected2D_Radar.RadarDesign.frontIs = (FrontIs)EditorGUILayout.EnumPopup("", Current_Selected2D_Radar.RadarDesign.frontIs, GUILayout.Width(220));
                            GUILayout.EndHorizontal();
                            #endregion

                            HelpMessage("Pading on the X axis");

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("X Pading");
                            Current_Selected2D_Radar.RadarDesign.xPadding = EditorGUILayout.FloatField("", Current_Selected2D_Radar.RadarDesign.xPadding, GUILayout.Width(220));
                            GUILayout.EndHorizontal();

                            HelpMessage("Pading on the Y axis");
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Y Pading");
                            Current_Selected2D_Radar.RadarDesign.yPadding = EditorGUILayout.FloatField("", Current_Selected2D_Radar.RadarDesign.yPadding, GUILayout.Width(220));
                            GUILayout.EndHorizontal();



                        }
                        Separator();

                        EditorGUILayout.Space();
                        #endregion

                        #region Design Sprites


                        #region Setting and creating Rotation Targets
                        HelpMessage("You are able to target layers blips or parts of your radar and have them rotate in various ways");

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Rotation Targets");
                        Current_Selected2D_Radar.RadarDesign.DesignsCount = Mathf.Clamp(EditorGUILayout.IntField("", Current_Selected2D_Radar.RadarDesign.DesignsCount, GUILayout.Width(60)), 0, 8000);
                        GUILayout.EndHorizontal();


                        if (Event.current.keyCode == KeyCode.Return)
                        {
                            if (Current_Selected2D_Radar.RadarDesign.RotationTargets.Count < Current_Selected2D_Radar.RadarDesign.DesignsCount)
                                Current_Selected2D_Radar.RadarDesign.RotationTargets.AddRange(Enumerable.Repeat(default(RotationTarget), Current_Selected2D_Radar.RadarDesign.DesignsCount));
                            else
                                Current_Selected2D_Radar.RadarDesign.RotationTargets.RemoveRange(Current_Selected2D_Radar.RadarDesign.DesignsCount, Current_Selected2D_Radar.RadarDesign.RotationTargets.Count - Current_Selected2D_Radar.RadarDesign.DesignsCount);

                        }
                        Separator();

                        EditorGUILayout.Space();
                        #endregion

                        EditorGUILayout.Separator();
                        foreach (var rotationTarget in Current_Selected2D_Radar.RadarDesign.RotationTargets.Select((x, i) => new { value = x, index = i }))
                        {




                            try
                            {

                                string Target = (!rotationTarget.value.TargetedObject) ? (rotationTarget.value.target == TargetBlip.InstancedBlip) ? rotationTarget.value.InstancedTargetBlipname : "nothing " : rotationTarget.value.TargetedObject.name;
                                string Target2 = (!rotationTarget.value.Target) ? "nothing " : rotationTarget.value.Target.name;
                                rotationTarget.value.ShowDesignSetings = EditorGUILayout.Foldout(rotationTarget.value.ShowDesignSetings, "Rotate " + Target);

                                GUILayout.BeginHorizontal();
                                GUILayout.Space(20);
                                GUILayout.BeginVertical();

                                if (rotationTarget.value.ShowDesignSetings)
                                {

                                    #region Draw Radar Design Interface
                                    HelpMessage("Here you will set the objet(blip or otherwise) which you wish to have rotate ");

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Rotate");
                                    rotationTarget.value.target = (TargetBlip)EditorGUILayout.EnumPopup(rotationTarget.value.target, GUILayout.Width(95));
                                    switch (rotationTarget.value.target)
                                    {

                                        case TargetBlip.ThisObject:

                                            rotationTarget.value.TargetedObject = (GameObject)EditorGUILayout.ObjectField(rotationTarget.value.TargetedObject, typeof(GameObject), true);

                                            break;
                                        case TargetBlip.InstancedBlip:

                                            rotationTarget.value.InstancedTargetBlipname = EditorGUILayout.TextField(rotationTarget.value.InstancedTargetBlipname);
                                            break;

                                    }


                                    //  string RotationTargetName = (rotationTarget.value.RtoationTarget) ? rotationTarget.value.RtoationTarget.name : "nothing";

                                    rotationTarget.value.rotations = (Rotations)EditorGUILayout.EnumPopup(rotationTarget.value.rotations, GUILayout.Width(95));
                                    GUILayout.Label("to");

                                    rotationTarget.value.ObjectToTrack = (TargetObject)EditorGUILayout.EnumPopup(rotationTarget.value.ObjectToTrack, GUILayout.Width(95));

                                    switch (rotationTarget.value.ObjectToTrack)
                                    {
                                        case TargetObject.FindObject:
                                            rotationTarget.value.FindingName = EditorGUILayout.TextField(rotationTarget.value.FindingName);
                                            break;
                                        case TargetObject.ObectWithTag:
                                            rotationTarget.value.tag = EditorGUILayout.TextField(rotationTarget.value.tag);
                                            break;
                                        case TargetObject.ThisObject:
                                            rotationTarget.value.Target = (GameObject)EditorGUILayout.ObjectField(rotationTarget.value.Target, typeof(GameObject), true);

                                            break;
                                        case TargetObject.InstancedBlip:
                                            rotationTarget.value.InstancedObjectToTrackBlipName = EditorGUILayout.TextField(rotationTarget.value.InstancedObjectToTrackBlipName);
                                            break;

                                    }
                                    GUILayout.EndHorizontal();
                                    HelpMessage("By what multiple will rotation be reduced");

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Damping");
                                    rotationTarget.value.RotationDamping = EditorGUILayout.FloatField(Mathf.Clamp(rotationTarget.value.RotationDamping, 1, Mathf.Infinity), GUILayout.Width(220));
                                    GUILayout.EndHorizontal();

                                    /*    HelpMessage("Freze rotation on any axis");

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Freeze");
                                        rotationTarget.value.FreezeX = GUILayout.Toggle(rotationTarget.value.FreezeX, "X");
                                        rotationTarget.value.FreezeY = GUILayout.Toggle(rotationTarget.value.FreezeY, "Y");
                                        rotationTarget.value.FreezeZ = GUILayout.Toggle(rotationTarget.value.FreezeZ, "Z");
                                        GUILayout.EndHorizontal(); */

                                    HelpMessage("Use the Y rotation of the targeted object  as the rotation of the target");

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Use " + Target2 + "'s Y rotation");
                                    rotationTarget.value.UseY = EditorGUILayout.Toggle(rotationTarget.value.UseY, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();

                                    #endregion
                                }
                                GUILayout.EndVertical();
                                GUILayout.EndHorizontal();
                                Separator();

                                EditorGUILayout.Separator();
                            }
                            catch
                            {

                            }


                        }
                        #endregion

                        #region Visualize and HelpMessage
                        HelpMessage("Turn on 'Visualize'. This show you your radar settings.");
                        Current_Selected2D_Radar.RadarDesign.Visualize = GUILayout.Toggle(Current_Selected2D_Radar.RadarDesign.Visualize, "Visualize");
                        HelpMessage("These help messages will help you to set up your radar");
                        ShowHelpMessages = GUILayout.Toggle(ShowHelpMessages, "Show Help Messages");

                        #endregion



                        GUILayout.EndScrollView();




                        break;
                    #endregion

                    #region Blip Design UI
                    case 1:

                        RadarBlipScrollPosition = EditorGUILayout.BeginScrollView(RadarBlipScrollPosition, false, false);

                        #region Setting Blips

                        HelpMessage("This blip ; if set to active will always appear at the center of your radar.");
                        Current_Selected2D_Radar._RadarCenterObject2D.State = Current_Selected2D_Radar._RadarCenterObject2D.IsActive ? Current_Selected2D_Radar._RadarCenterObject2D.Tag + " is " + " Active" : Current_Selected2D_Radar._RadarCenterObject2D.Tag + " is " + " Inactive";

                        GUILayout.BeginHorizontal();
                        Current_Selected2D_Radar._RadarCenterObject2D.ShowCenterBLipSettings = EditorGUILayout.Foldout(Current_Selected2D_Radar._RadarCenterObject2D.ShowCenterBLipSettings, Current_Selected2D_Radar._RadarCenterObject2D.State);
                        if (GUILayout.Button((Current_Selected2D_Radar._RadarCenterObject2D.IsActive) ? On : Off, "Label", GUILayout.Width(30))) Current_Selected2D_Radar._RadarCenterObject2D.IsActive = !Current_Selected2D_Radar._RadarCenterObject2D.IsActive;
                        GUILayout.EndHorizontal();
                        cursorchange();

                        if (Current_Selected2D_Radar._RadarCenterObject2D.ShowCenterBLipSettings)
                        {
                            Separator();
                            EditorGUILayout.Space();

                            #region sprite
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginVertical();

                            HelpMessage("If using sprites as blips; set the Sprite and Material here");
                            Current_Selected2D_Radar._RadarCenterObject2D.ShowSpriteBlipSettings = EditorGUILayout.Foldout(Current_Selected2D_Radar._RadarCenterObject2D.ShowSpriteBlipSettings, Current_Selected2D_Radar._RadarCenterObject2D.Tag + " Sprite");

                            if (Current_Selected2D_Radar._RadarCenterObject2D.ShowSpriteBlipSettings)
                            {
                                #region Sprite Blip Design
                                GUILayout.BeginHorizontal();

                                GUILayout.BeginVertical();

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Sprite");
                                Current_Selected2D_Radar._RadarCenterObject2D.icon = (Sprite)EditorGUILayout.ObjectField(Current_Selected2D_Radar._RadarCenterObject2D.icon, typeof(Sprite), false, GUILayout.Width((Current_Selected2D_Radar._RadarCenterObject2D.icon) ? 170 : 220));
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Material");
                                Current_Selected2D_Radar._RadarCenterObject2D.SpriteMaterial = (Material)EditorGUILayout.ObjectField("", Current_Selected2D_Radar._RadarCenterObject2D.SpriteMaterial, typeof(Material), true, GUILayout.Width((Current_Selected2D_Radar._RadarCenterObject2D.icon) ? 170 : 220));
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Colour");
                                Current_Selected2D_Radar._RadarCenterObject2D.colour = EditorGUILayout.ColorField("", Current_Selected2D_Radar._RadarCenterObject2D.colour, GUILayout.Width((Current_Selected2D_Radar._RadarCenterObject2D.icon) ? 170 : 220));
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Order In Layer");
                                Current_Selected2D_Radar._RadarCenterObject2D.OrderInLayer = EditorGUILayout.IntField(Current_Selected2D_Radar._RadarCenterObject2D.OrderInLayer, GUILayout.Width(220));
                                GUILayout.EndHorizontal();

                                GUILayout.EndVertical();

                                try
                                {
                                    GUILayout.Box(Current_Selected2D_Radar._RadarCenterObject2D.icon.texture, GUILayout.Height(50), GUILayout.Width(50));
                                }
                                catch { }

                                GUILayout.EndHorizontal();
                                #endregion
                            }
                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();

                            Separator();

                            EditorGUILayout.Space();
                            #endregion

                            #region Prefab

                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginVertical();
                            HelpMessage("If using prefab as blips; se the prefab here");
                            Current_Selected2D_Radar._RadarCenterObject2D.ShowPrefabBlipSettings = EditorGUILayout.Foldout(Current_Selected2D_Radar._RadarCenterObject2D.ShowPrefabBlipSettings, Current_Selected2D_Radar._RadarCenterObject2D.Tag + " Prefab");
                            if (Current_Selected2D_Radar._RadarCenterObject2D.ShowPrefabBlipSettings)
                            {
                                #region Prefab BLip Design

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Prefab");
                                Current_Selected2D_Radar._RadarCenterObject2D.prefab = (Transform)EditorGUILayout.ObjectField("", Current_Selected2D_Radar._RadarCenterObject2D.prefab, typeof(Transform), true, GUILayout.Width(220));
                                GUILayout.EndHorizontal();

                                #endregion
                            }
                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();

                            Separator();

                            EditorGUILayout.Space();
                            #endregion

                            #region Additional Options

                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginVertical();

                            HelpMessage("Displaying additional options for your blip");
                            Current_Selected2D_Radar._RadarCenterObject2D.ShowAdditionalOptions = EditorGUILayout.Foldout(Current_Selected2D_Radar._RadarCenterObject2D.ShowAdditionalOptions, "AdditionalOptions");
                            if (Current_Selected2D_Radar._RadarCenterObject2D.ShowAdditionalOptions)
                            {
                                HelpMessage("When eabled all " + Current_Selected2D_Radar._RadarCenterObject2D.Tag + "blips will not disppear when at they pass the bounderies of the radar, but will remain at the edge and will be scaled based on its distance from the center object");

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Always Show " + Current_Selected2D_Radar._RadarCenterObject2D.Tag + " in radar");
                                Current_Selected2D_Radar._RadarCenterObject2D.AlwaysShowCenterObject = GUILayout.Toggle(Current_Selected2D_Radar._RadarCenterObject2D.AlwaysShowCenterObject, "", GUILayout.Width(220));
                                GUILayout.EndHorizontal();

                            }



                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();

                            Separator();

                            EditorGUILayout.Space();

                            #endregion

                            #region Scale And Rotation Settings


                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginVertical();


                            Current_Selected2D_Radar._RadarCenterObject2D.ShowGeneralSettings = EditorGUILayout.Foldout(Current_Selected2D_Radar._RadarCenterObject2D.ShowGeneralSettings, "Rotation and Scale");
                            if (Current_Selected2D_Radar._RadarCenterObject2D.ShowGeneralSettings)
                            {
                                #region Blip Scale Settings
                                if (!Current_Selected2D_Radar._RadarCenterObject2D.CenterObjectCanScaleByDistance)
                                {
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Scale");
                                    GUILayout.Space(153);
                                    Current_Selected2D_Radar._RadarCenterObject2D.BlipSize = EditorGUILayout.FloatField(" ", Current_Selected2D_Radar._RadarCenterObject2D.BlipSize, GUILayout.MaxWidth(Screen.width));
                                    GUILayout.EndHorizontal();
                                }

                                /*  HelpMessage("Your Center Blip will always shrink when moving to the edge of the radar if this is not checkd");
                                  GUILayout.BeginHorizontal();
                                  GUILayout.Label("Autoscale Only At Border");
                                  Current_Selected2D_Radar._RadarCenterObject2D.AutoScaleOnlyAtBorder = GUILayout.Toggle(Current_Selected2D_Radar._RadarCenterObject2D.AutoScaleOnlyAtBorder, "", GUILayout.Width(220));
                                  GUILayout.EndHorizontal();

                                  HelpMessage("this value determines , be how much the blip will scale over distance");
                                  GUILayout.BeginHorizontal();
                                  GUILayout.Label("AutoScale Factor");
                                  Current_Selected2D_Radar._RadarCenterObject2D.AutoScaleFactor = EditorGUILayout.FloatField("", Current_Selected2D_Radar._RadarCenterObject2D.AutoScaleFactor, GUILayout.Width(220));
                                  GUILayout.EndHorizontal();

                                  */

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Scale By Distance");
                                Current_Selected2D_Radar._RadarCenterObject2D.CenterObjectCanScaleByDistance = GUILayout.Toggle(Current_Selected2D_Radar._RadarCenterObject2D.CenterObjectCanScaleByDistance, "", GUILayout.Width(220));
                                GUILayout.EndHorizontal();

                                if (Current_Selected2D_Radar._RadarCenterObject2D.CenterObjectCanScaleByDistance)
                                {
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Min Scale");
                                    Current_Selected2D_Radar._RadarCenterObject2D.BlipMinSize = EditorGUILayout.FloatField("", Current_Selected2D_Radar._RadarCenterObject2D.BlipMinSize, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Max Scale");
                                    Current_Selected2D_Radar._RadarCenterObject2D.BlipMaxSize = EditorGUILayout.FloatField("", Current_Selected2D_Radar._RadarCenterObject2D.BlipMaxSize, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();


                                }
                                #endregion


                                #region Blip Rotation Settings
                                HelpMessage("Track Rotation allows for the blip to rotate and match the rotation of the tracked object");
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Track Rotation");
                                Current_Selected2D_Radar._RadarCenterObject2D.IsTrackRotation = GUILayout.Toggle(Current_Selected2D_Radar._RadarCenterObject2D.IsTrackRotation, "", GUILayout.Width(220));
                                GUILayout.EndHorizontal();



                                #endregion

                            }
                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();

                            Separator();

                            EditorGUILayout.Space();

                            #region Universal Settings
                            HelpMessage("With the current settings,If this blip is active, it will represent your gameobject with the tag " + Current_Selected2D_Radar._RadarCenterObject2D.Tag + " on the layer " + Current_Selected2D_Radar._RadarCenterObject2D.Layer + " and will be at scale " + Current_Selected2D_Radar._RadarCenterObject2D.BlipSize);

                            GUILayout.BeginHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Create blip");
                            Current_Selected2D_Radar._RadarCenterObject2D._CreateBlipAs = (CreateBlipAs)EditorGUILayout.EnumPopup(Current_Selected2D_Radar._RadarCenterObject2D._CreateBlipAs, GUILayout.Width(100));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("With Tag"); Current_Selected2D_Radar._RadarCenterObject2D.Tag = EditorGUILayout.TagField(Current_Selected2D_Radar._RadarCenterObject2D.Tag);
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("On Layer"); Current_Selected2D_Radar._RadarCenterObject2D.Layer = EditorGUILayout.LayerField(Current_Selected2D_Radar._RadarCenterObject2D.Layer);
                            GUILayout.EndHorizontal();

                            GUILayout.EndHorizontal();

                            if (Current_Selected2D_Radar._RadarCenterObject2D._CreateBlipAs == CreateBlipAs.AsMesh)
                            {
                                EditorGUILayout.HelpBox("Meshes are not supporeted for 2D blips, will fallback to Sprite", MessageType.Warning);
                            }


                            #endregion


                        }
                        #endregion

                        Separator();
                        GUILayout.Space(10);


                        #region Setting and creating Blips


                        GUILayout.BeginHorizontal();

                        GUILayout.Label("Number Of Other Blip Types");
                        Current_Selected2D_Radar.RadarDesign.BlipCount = Mathf.Clamp(EditorGUILayout.IntField("", Current_Selected2D_Radar.RadarDesign.BlipCount, GUILayout.Width(60)), 0, 8000);

                        if (Event.current.keyCode == KeyCode.Return)
                        {
                            if (Current_Selected2D_Radar.Blips.Count < Current_Selected2D_Radar.RadarDesign.BlipCount)
                                Current_Selected2D_Radar.Blips.AddRange(Enumerable.Repeat(default(RadarBlips2D), Current_Selected2D_Radar.RadarDesign.BlipCount));
                            else
                                Current_Selected2D_Radar.Blips.RemoveRange(Current_Selected2D_Radar.RadarDesign.BlipCount, Current_Selected2D_Radar.Blips.Count - Current_Selected2D_Radar.RadarDesign.BlipCount);

                        }
                        GUILayout.EndHorizontal();
                        #endregion
                        Separator();


                        #region Setting and creating All other Blips
                        EditorGUILayout.Separator();

                        foreach (var _Blip in Current_Selected2D_Radar.Blips.Select((x, i) => new { Value = x as RadarBlips2D, Index = i }))
                        {
                            try
                            {

                                _Blip.Value.State = _Blip.Value.IsActive ? _Blip.Value.Tag + " is " + "Active" : _Blip.Value.Tag + " is " + "Inactive";

                                GUILayout.BeginHorizontal();
                                _Blip.Value.ShowBLipSettings = EditorGUILayout.Foldout(_Blip.Value.ShowBLipSettings, _Blip.Value.State);
                                if (GUILayout.Button((_Blip.Value.IsActive) ? On : Off, "Label", GUILayout.Width(30))) _Blip.Value.IsActive = !_Blip.Value.IsActive;
                                GUILayout.EndHorizontal();
                                cursorchange();


                                if (_Blip.Value.ShowBLipSettings)
                                {
                                    Separator();
                                    EditorGUILayout.Space();

                                    #region Sprite
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(20);
                                    GUILayout.BeginVertical();



                                    HelpMessage("If using sprites as blips; set the Sprite and Material here");

                                    _Blip.Value.ShowSpriteBlipSettings = EditorGUILayout.Foldout(_Blip.Value.ShowSpriteBlipSettings, _Blip.Value.Tag + " Sprite");
                                    if (_Blip.Value.ShowSpriteBlipSettings)
                                    {

                                        #region Sprite Blip Design
                                        GUILayout.BeginHorizontal();

                                        GUILayout.BeginVertical();

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Sprite");
                                        _Blip.Value.icon = (Sprite)EditorGUILayout.ObjectField(_Blip.Value.icon, typeof(Sprite), false, (GUILayout.Width(220)));
                                        GUILayout.EndHorizontal();

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Material");
                                        Current_Selected2D_Radar.Blips[_Blip.Index].SpriteMaterial = (Material)EditorGUILayout.ObjectField(Current_Selected2D_Radar.Blips[_Blip.Index].SpriteMaterial, typeof(Material), true, (GUILayout.Width(220)));
                                        GUILayout.EndHorizontal();

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Colour");
                                        Current_Selected2D_Radar.Blips[_Blip.Index].colour = EditorGUILayout.ColorField("", Current_Selected2D_Radar.Blips[_Blip.Index].colour, (GUILayout.Width(220)));
                                        GUILayout.EndHorizontal();

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Order In Layer");
                                        Current_Selected2D_Radar.Blips[_Blip.Index].OrderInLayer = EditorGUILayout.IntField(Current_Selected2D_Radar.Blips[_Blip.Index].OrderInLayer, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        GUILayout.EndVertical();

                                        try
                                        {
                                            GUILayout.Box(_Blip.Value.icon.texture, GUILayout.Height(50), GUILayout.Width(50));
                                        }
                                        catch { }
                                        GUILayout.EndHorizontal();
                                        #endregion


                                    }
                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();

                                    Separator();

                                    EditorGUILayout.Space();
                                    #endregion

                                    #region Prefab

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(20);
                                    GUILayout.BeginVertical();
                                    HelpMessage("If using prefabs as blips; set the Prefab here");

                                    _Blip.Value.ShowPrefabBlipSettings = EditorGUILayout.Foldout(_Blip.Value.ShowPrefabBlipSettings, _Blip.Value.Tag + " Prefab");
                                    if (_Blip.Value.ShowPrefabBlipSettings)
                                    {
                                        #region Prefab BLip Design

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Prefab");
                                        _Blip.Value.prefab = (Transform)EditorGUILayout.ObjectField("", _Blip.Value.prefab, typeof(Transform), true, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        #endregion
                                    }
                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();

                                    Separator();

                                    EditorGUILayout.Space();
                                    #endregion

                                    #region Additional Options

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(20);
                                    GUILayout.BeginVertical();

                                    HelpMessage("Displaying additional options for your blip");
                                    Current_Selected2D_Radar.Blips[_Blip.Index].ShowAdditionalOptions = EditorGUILayout.Foldout(Current_Selected2D_Radar.Blips[_Blip.Index].ShowAdditionalOptions, "AdditionalOptions");
                                    if (Current_Selected2D_Radar.Blips[_Blip.Index].ShowAdditionalOptions)
                                    {
                                        HelpMessage("When eabled all " + _Blip.Value.Tag + "blips will not disppear when at they pass the bounderies of the radar, but will remain at the edge and will be scaled based on its distance from the center object");

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Always Show " + _Blip.Value.Tag + " in radar");
                                        Current_Selected2D_Radar.Blips[_Blip.Index].AlwaysShowBlipsInRadarSpace = GUILayout.Toggle(Current_Selected2D_Radar.Blips[_Blip.Index].AlwaysShowBlipsInRadarSpace, "", GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                    }

                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();

                                    Separator();

                                    EditorGUILayout.Space();

                                    #endregion

                                    #region Optimization 

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(20);
                                    GUILayout.BeginVertical();

                                    HelpMessage("Options for optimization the radar proceses");
                                    GUILayout.BeginHorizontal();
                                    Current_Selected2D_Radar.Blips[_Blip.Index].ShowOptimizationSettings = EditorGUILayout.Foldout(Current_Selected2D_Radar.Blips[_Blip.Index].ShowOptimizationSettings, "Optimization Options");
                                    GUILayout.Box(Optimizeicon, "Label", GUILayout.Width(120), GUILayout.Height(20));
                                    GUILayout.EndHorizontal();
                                    if (Current_Selected2D_Radar.Blips[_Blip.Index].ShowOptimizationSettings)
                                    {
                                        if (Current_Selected2D_Radar.Blips[_Blip.Index].optimization.objectFindingMethod == ObjectFindingMethod.Pooling)
                                        {
                                            HelpMessage("If you are spawning any new objects into the scene then call radar2D.DoInstanceObjectCheck() at instance or at the end of instancing");
                                        }

                                        if (Current_Selected2D_Radar.Blips[_Blip.Index].optimization.objectFindingMethod != ObjectFindingMethod.Recursive)
                                        {
                                            HelpMessage("This requires that you call ' _2DRadar.doInstanceObjectCheck() whenever you want to make the radar search for objects to create blips from. This can also be used to icrease your internal pool size if you need to track more objects'");

                                            HelpMessage(" If you know exactly ow many scene objects this blip should represet then you can set the pool size manually");
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Set Pool Size");
                                            Current_Selected2D_Radar.Blips[_Blip.Index].optimization.SetPoolSizeManually = EditorGUILayout.Toggle(Current_Selected2D_Radar.Blips[_Blip.Index].optimization.SetPoolSizeManually, GUILayout.Width(220));
                                            GUILayout.EndHorizontal();
                                            if (Current_Selected2D_Radar.Blips[_Blip.Index].optimization.SetPoolSizeManually)
                                            {
                                                HelpMessage("The mumber of scene objects that this blip will represent");
                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Pool Size");
                                                Current_Selected2D_Radar.Blips[_Blip.Index].optimization.poolSize = EditorGUILayout.IntField(Current_Selected2D_Radar.Blips[_Blip.Index].optimization.poolSize, GUILayout.Width(220));
                                                GUILayout.EndHorizontal();

                                                HelpMessage("If your pool size is too large then the count will be calculated DOWN");
                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Recalculate Pool Size On Start");
                                                Current_Selected2D_Radar.Blips[_Blip.Index].optimization.RecalculatePoolSizeBasedOnFirstFoundObjects = EditorGUILayout.Toggle(Current_Selected2D_Radar.Blips[_Blip.Index].optimization.RecalculatePoolSizeBasedOnFirstFoundObjects, GUILayout.Width(220));
                                                GUILayout.EndHorizontal();
                                            }

                                        }




                                        HelpMessage("This method allows you to use object pooling to store your blips");
                                        GUILayout.BeginHorizontal();
                                        var info = Current_Selected2D_Radar.Blips[_Blip.Index].optimization.objectFindingMethod == ObjectFindingMethod.Pooling ? " (Fast)" : (Current_Selected2D_Radar.Blips[_Blip.Index].optimization.RequireInstanceObjectCheck) ? " (Fast)" : " (Slower)";
                                        GUILayout.Label("Use Recursive Object Finding" + info);
                                        Current_Selected2D_Radar.Blips[_Blip.Index].optimization.objectFindingMethod = (ObjectFindingMethod)EditorGUILayout.EnumPopup(Current_Selected2D_Radar.Blips[_Blip.Index].optimization.objectFindingMethod, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        if (Current_Selected2D_Radar.Blips[_Blip.Index].optimization.objectFindingMethod == ObjectFindingMethod.Recursive)
                                        {
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Require Instance Object Check");
                                            if (Current_Selected2D_Radar.Blips[_Blip.Index].optimization.RequireInstanceObjectCheck)
                                                HelpMessage("This requires that you call ' _2DRadar.doInstanceObjectCheck() whenever you want to make the radar search for objects to create blips from. This can also be used to icrease your internal pool size if you need to track more objects'");
                                            Current_Selected2D_Radar.Blips[_Blip.Index].optimization.RequireInstanceObjectCheck = EditorGUILayout.Toggle(Current_Selected2D_Radar.Blips[_Blip.Index].optimization.RequireInstanceObjectCheck, GUILayout.Width(220));
                                            GUILayout.EndHorizontal();
                                        }

                                        HelpMessage("Allows blips to be removed whenever the object it represent  , changes its tag");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Remove Blip On Tag Change");
                                        Current_Selected2D_Radar.Blips[_Blip.Index].optimization.RemoveBlipsOnTagChange = EditorGUILayout.Toggle(Current_Selected2D_Radar.Blips[_Blip.Index].optimization.RemoveBlipsOnTagChange, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        HelpMessage("Allows for the blip to be turned off when the object its represents is disabled");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Remove Blip On Disable");
                                        Current_Selected2D_Radar.Blips[_Blip.Index].optimization.RemoveBlipsOnDisable = EditorGUILayout.Toggle(Current_Selected2D_Radar.Blips[_Blip.Index].optimization.RemoveBlipsOnDisable, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();


                                    }

                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();

                                    Separator();

                                    EditorGUILayout.Space();

                                    #endregion

                                    #region Scale and Rotation Settings

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(20);
                                    GUILayout.BeginVertical();

                                    _Blip.Value.ShowGeneralSettings = EditorGUILayout.Foldout(_Blip.Value.ShowGeneralSettings, "Rotation and Scale");
                                    if (_Blip.Value.ShowGeneralSettings)
                                    {
                                        HelpMessage("Set the scale of the blip. If 'Scale by distance' is being used then the blps will scale in the radar based on their distance from the center object . the visibility of the scaling varies based on the size of the radar ");

                                        #region Blip Scale Settings
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Scale by distance");
                                        Current_Selected2D_Radar.Blips[_Blip.Index].BlipCanScleBasedOnDistance = GUILayout.Toggle(Current_Selected2D_Radar.Blips[_Blip.Index].BlipCanScleBasedOnDistance, "", GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        if (!Current_Selected2D_Radar.Blips[_Blip.Index].BlipCanScleBasedOnDistance)
                                        {
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Scale");
                                            GUILayout.Space(153);
                                            Current_Selected2D_Radar.Blips[_Blip.Index].BlipSize = EditorGUILayout.FloatField(" ", Current_Selected2D_Radar.Blips[_Blip.Index].BlipSize, GUILayout.MaxWidth(Screen.width));
                                            GUILayout.EndHorizontal();

                                        }
                                        else
                                        {
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Min Scale");
                                            Current_Selected2D_Radar.Blips[_Blip.Index].BlipMinSize = EditorGUILayout.FloatField("", Current_Selected2D_Radar.Blips[_Blip.Index].BlipMinSize, GUILayout.Width(220));
                                            GUILayout.EndHorizontal();

                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Max Scale");
                                            Current_Selected2D_Radar.Blips[_Blip.Index].BlipMaxSize = EditorGUILayout.FloatField("", Current_Selected2D_Radar.Blips[_Blip.Index].BlipMaxSize, GUILayout.Width(220));
                                            GUILayout.EndHorizontal();

                                        }

                                        #endregion

                                        #region Rotation Settings
                                        HelpMessage("Track Rotation allows for the blip to rotate and match the rotation of the tracked object");

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Track Rotation");
                                        Current_Selected2D_Radar.Blips[_Blip.Index].IsTrackRotation = GUILayout.Toggle(Current_Selected2D_Radar.Blips[_Blip.Index].IsTrackRotation, "", GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        #endregion

                                    }

                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();

                                    Separator();

                                    EditorGUILayout.Space();
                                    #endregion


                                    #region Universal Settings
                                    HelpMessage("With the current settings,If this blip is active, it will represent your gameobject with the tag " + _Blip.Value.Tag + " on the layer " + _Blip.Value.Layer + " and will be at scale " + Current_Selected2D_Radar.Blips[_Blip.Index].BlipSize);

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Create blip");

                                    _Blip.Value._CreateBlipAs = (CreateBlipAs)EditorGUILayout.EnumPopup(_Blip.Value._CreateBlipAs, GUILayout.Width(100));

                                    GUILayout.BeginHorizontal(); GUILayout.Label("With Tag"); _Blip.Value.Tag = EditorGUILayout.TagField(_Blip.Value.Tag); GUILayout.EndHorizontal();

                                    GUILayout.BeginHorizontal(); GUILayout.Label("On Layer"); _Blip.Value.Layer = EditorGUILayout.LayerField(_Blip.Value.Layer); GUILayout.EndHorizontal();

                                    GUILayout.EndHorizontal();
                                    if (_Blip.Value._CreateBlipAs == CreateBlipAs.AsMesh)
                                    {
                                        EditorGUILayout.HelpBox("Meshes are not supporeted for 2D blips, will fallback to Sprite", MessageType.Warning);
                                    }

                                    #endregion
                                }
                                #endregion

                                Separator();
                                EditorGUILayout.Separator();

                            }
                            catch { }
                        }
                        #endregion

                        EditorGUILayout.EndScrollView();


                        break;
                    #endregion

                    #region Make New Radar
                    case 2:

                        CreationArea();

                        break;
                        #endregion
                }
            }

            #endregion

            #region 3D UI

            if (RadarObject && RadarObject.GetComponent<_3DRadar>() != null)
            {

                Current_Selected3D_Radar = RadarObject.GetComponent<_3DRadar>();



                switch (Tab_Selection)
                {
                    #region Radar Design and Settings
                    case 0:



                        DesignsScrollPosition = GUILayout.BeginScrollView(DesignsScrollPosition, false, false);
                        EditorGUILayout.Space();


                        #region Camera Section
                        if (Current_Selected3D_Radar.RadarDesign._3DSystemsWithScreenSpaceFunction)
                        {
                            HelpMessage("Here you set up your rendering camera and main camera");

                            Current_Selected3D_Radar.RadarDesign.ShowRenderCameraSettings = EditorGUILayout.Foldout(Current_Selected3D_Radar.RadarDesign.ShowRenderCameraSettings, "Camera Settings");

                            if (Current_Selected3D_Radar.RadarDesign.ShowRenderCameraSettings)
                            {
                                HelpMessage("This is the camers in front of which the radar will be displayed");

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Manual Camera Setup");
                                Current_Selected3D_Radar.RadarDesign.ManualCameraSetup = GUILayout.Toggle(Current_Selected3D_Radar.RadarDesign.ManualCameraSetup, "", GUILayout.Width(220));
                                GUILayout.EndHorizontal();

                                if (Current_Selected3D_Radar.RadarDesign.ManualCameraSetup)
                                {
                                    HelpMessage("you can drag and drop of assign a camera here via script");
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Camera");
                                    Current_Selected3D_Radar.RadarDesign.camera = (Camera)EditorGUILayout.ObjectField("", Current_Selected3D_Radar.RadarDesign.camera, typeof(Camera), true, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();
                                }
                                else
                                {
                                    HelpMessage("Will always find and set the scenes Main Camera as the efault Camera  for the radar");

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Always use main camera");
                                    Current_Selected3D_Radar.RadarDesign.UseMainCamera = GUILayout.Toggle(Current_Selected3D_Radar.RadarDesign.UseMainCamera, "", GUILayout.Width(220));
                                    GUILayout.EndHorizontal();

                                    HelpMessage("Finds a camera with the selected tag and uses it as the radars camera");

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Use camera with tag");
                                    Current_Selected3D_Radar.RadarDesign.CameraTag = EditorGUILayout.TagField("", Current_Selected3D_Radar.RadarDesign.CameraTag, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();
                                }

                                HelpMessage("YOU MUST HAVE A RENDERING CAMERA");
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Rendering Camera");
                                Current_Selected3D_Radar.RadarDesign.renderingCamera = (Camera)EditorGUILayout.ObjectField("", Current_Selected3D_Radar.RadarDesign.renderingCamera, typeof(Camera), true, GUILayout.Width(220));
                                GUILayout.EndHorizontal();
                            }

                            Separator();

                            EditorGUILayout.Space();
                        }

                        #endregion

                        #region Minimap Settings
                        if (Current_Selected3D_Radar.RadarDesign._3DSystemsWithMinimapFunction)
                        {
                            GUILayout.BeginHorizontal();
                            Current_Selected3D_Radar.RadarDesign.ShowMinimapSettings = EditorGUILayout.Foldout(Current_Selected3D_Radar.RadarDesign.ShowMinimapSettings, "Minimap Settings");
                            GUILayout.Space(300);
                            GUILayout.EndHorizontal();
                            if (Current_Selected3D_Radar.RadarDesign.ShowMinimapSettings)
                            {
                                if (Current_Selected3D_Radar.RadarDesign._3DSystemsWithMinimapFunction)
                                {

                                    HelpMessage("When set to Realtime, the minimap texture will be drawn from the curent scene . If static then a your predesigned map will be used");
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Minimap Type");
                                    Current_Selected3D_Radar.minimapModule.mapType = (MapType)EditorGUILayout.EnumPopup(Current_Selected3D_Radar.minimapModule.mapType, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();

                                    if (Current_Selected3D_Radar.minimapModule.mapType != MapType.Realtime)
                                    {
                                        HelpMessage("The static texture2D mage that will be your map");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Minimap Texture");
                                        Current_Selected3D_Radar.minimapModule.MapTexture = (Sprite)EditorGUILayout.ObjectField(Current_Selected3D_Radar.minimapModule.MapTexture, typeof(Sprite), false, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        HelpMessage("Sets the ratio which the radars internal system will use to ensue consistncy in your minimap");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Calibrate Minimap");
                                        Current_Selected3D_Radar.minimapModule.calibrate = EditorGUILayout.Toggle("", Current_Selected3D_Radar.minimapModule.calibrate, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        HelpMessage("The order of the map sprite in the layer");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Order In layer");
                                        Current_Selected3D_Radar.minimapModule.OrderInLayer = EditorGUILayout.IntField(Current_Selected3D_Radar.minimapModule.OrderInLayer, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();
                                    }
                                    else
                                    {
                                        HelpMessage("The render texture which will be used to pass the data from Realtime Minimap Camera to the Map");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Render Texture");
                                        Current_Selected3D_Radar.minimapModule.renderTexture = (RenderTexture)EditorGUILayout.ObjectField(Current_Selected3D_Radar.minimapModule.renderTexture, typeof(RenderTexture), true, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        if (!Current_Selected3D_Radar.minimapModule.RealtimeMinimapCamera)
                                        {
                                            if (GUILayout.Button("Create Minimap Camera", GUILayout.Width(220)))
                                            {
                                                Current_Selected3D_Radar.minimapModule.RealtimeMinimapCamera = CreateRealtimeMinimapCamera(typeof(_3DRadar));

                                            }

                                        }
                                        else
                                        {
                                            HelpMessage("Your minimap camera ");
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Minimap Camera");
                                            Current_Selected3D_Radar.minimapModule.RealtimeMinimapCamera = (Camera)EditorGUILayout.ObjectField(Current_Selected3D_Radar.minimapModule.RealtimeMinimapCamera, typeof(Camera), true, GUILayout.Width(220));
                                            GUILayout.EndHorizontal();
                                        }
                                        HelpMessage("The y position of your Realtime Minimap Camera");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Camera Height");
                                        Current_Selected3D_Radar.minimapModule.CameraHeight = EditorGUILayout.FloatField(Current_Selected3D_Radar.minimapModule.CameraHeight, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();
                                    }



                                    HelpMessage("the Material to be place on the Minimap, this must be a material must be able to be masked a,d its shader must allow for Texture images");
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Map Material");
                                    Current_Selected3D_Radar.minimapModule.MapMaterial = (Material)EditorGUILayout.ObjectField(Current_Selected3D_Radar.minimapModule.MapMaterial, typeof(Material), false, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();

                                    HelpMessage("The material that will Mask the Map  in a circle");
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Mask Material");
                                    Current_Selected3D_Radar.minimapModule.MaskMaterial = (Material)EditorGUILayout.ObjectField(Current_Selected3D_Radar.minimapModule.MaskMaterial, typeof(Material), false, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();

                                    HelpMessage("Places the map components on a specific layer at runtime");
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("On Layer"); Current_Selected3D_Radar.minimapModule.layer = EditorGUILayout.LayerField(Current_Selected3D_Radar.minimapModule.layer, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();


                                    if (Current_Selected3D_Radar.minimapModule.calibrate && Current_Selected3D_Radar.minimapModule.mapType == MapType.Static)
                                    {
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Map Scale");
                                        Current_Selected3D_Radar.minimapModule.MapScale = EditorGUILayout.FloatField("", Current_Selected3D_Radar.minimapModule.MapScale, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();
                                    }

                                }

                            }
                            Separator();
                            EditorGUILayout.Space();
                        }
                        #endregion

                        #region Scale Settings
                        HelpMessage("The scale setting of your radar");
                        Current_Selected3D_Radar.RadarDesign.ShowScaleSettings = EditorGUILayout.Foldout(Current_Selected3D_Radar.RadarDesign.ShowScaleSettings, "Scale");

                        if (Current_Selected3D_Radar.RadarDesign.ShowScaleSettings)
                        {
                            HelpMessage("This will override the Radar Diameter value to make the radar be set to the default scale x,y,z in the inspector");

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Use Local Scale");
                            Current_Selected3D_Radar.RadarDesign.UseLocalScale = GUILayout.Toggle(Current_Selected3D_Radar.RadarDesign.UseLocalScale, "", GUILayout.Width(220));
                            GUILayout.EndHorizontal();

                            if (!Current_Selected3D_Radar.RadarDesign.UseLocalScale)
                            {
                                HelpMessage("Radar Diameter is the diameter of the radar");

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Radar Diameter");
                                GUILayout.Space(110);
                                Current_Selected3D_Radar.RadarDesign.RadarDiameter = EditorGUILayout.FloatField(" ", Mathf.Clamp(Current_Selected3D_Radar.RadarDesign.RadarDiameter, 0, Mathf.Infinity), GUILayout.MaxWidth(Screen.width));
                                GUILayout.EndHorizontal();
                            }

                            HelpMessage("Anything outside of the tracking bounds will not be seen");

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Tracking Bounds");
                            GUILayout.Space(106);
                            Current_Selected3D_Radar.RadarDesign.TrackingBounds = EditorGUILayout.FloatField(" ", Mathf.Clamp(Current_Selected3D_Radar.RadarDesign.TrackingBounds, 0, Mathf.Infinity), GUILayout.MaxWidth(Screen.width));
                            GUILayout.EndHorizontal();






                            HelpMessage("When selected , the scale of the radar once greater than or less than 1 ; will be ignored");

                            if (Current_Selected3D_Radar.RadarDesign._3DSystemsWithScreenSpaceFunction)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Ignore Diameter Scale");
                                Current_Selected3D_Radar.RadarDesign.IgnoreDiameterScale = GUILayout.Toggle(Current_Selected3D_Radar.RadarDesign.IgnoreDiameterScale, "", GUILayout.Width(220));
                                GUILayout.EndHorizontal();
                            }

                            HelpMessage("Scene Scale is the zoom of the radar or how much units of space the radar can 'read'");

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Scene Scale");
                            GUILayout.Space(132);
                            Current_Selected3D_Radar.RadarDesign.SceneScale = EditorGUILayout.FloatField(" ", Mathf.Clamp(Current_Selected3D_Radar.RadarDesign.SceneScale, 1, Mathf.Infinity), GUILayout.MaxWidth(Screen.width));
                            GUILayout.EndHorizontal();

                            HelpMessage("Anything inside this area will be culled(not seen)");

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Inner Culling Zone");
                            GUILayout.Space(95);
                            Current_Selected3D_Radar.RadarDesign.InnerCullingZone = EditorGUILayout.FloatField(" ", Mathf.Clamp(Current_Selected3D_Radar.RadarDesign.InnerCullingZone, 0, Current_Selected3D_Radar.RadarDesign.TrackingBounds), GUILayout.MaxWidth(Screen.width));
                            GUILayout.EndHorizontal();
                        }
                        Separator();

                        EditorGUILayout.Space();
                        #endregion


                        #region Positioning  Section
                        if (Current_Selected3D_Radar.RadarDesign._3DSystemsWithScreenSpaceFunction)
                        {
                            HelpMessage("Position setting of your radar in screen space");

                            Current_Selected3D_Radar.RadarDesign.ShowPositioningSettings = EditorGUILayout.Foldout(Current_Selected3D_Radar.RadarDesign.ShowPositioningSettings, "Position");
                            if (Current_Selected3D_Radar.RadarDesign.ShowPositioningSettings)
                            {

                                #region Positioning settings
                                HelpMessage("Choose between 9 point snapping or Manual positioning");

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Radar Positioning");
                                Current_Selected3D_Radar.RadarDesign.radarPositioning = (RadarPositioning)EditorGUILayout.EnumPopup("", Current_Selected3D_Radar.RadarDesign.radarPositioning, GUILayout.Width(220));
                                GUILayout.EndHorizontal();

                                switch (Current_Selected3D_Radar.RadarDesign.radarPositioning)
                                {
                                    case RadarPositioning.Manual:
                                        HelpMessage("Position the radar manually on the x and y axis");

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("X and Y position");
                                        Current_Selected3D_Radar.RadarDesign.RadarRect.position = EditorGUILayout.Vector2Field("", Current_Selected3D_Radar.RadarDesign.RadarRect.position, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        break;
                                    case RadarPositioning.Snap:
                                        HelpMessage("Use our 9 point snapping  to snap the position of your radar to 9 dirent points on your screen");

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Snap to the");
                                        Current_Selected3D_Radar.RadarDesign.snapPosition = (SnapPosition)EditorGUILayout.EnumPopup("", Current_Selected3D_Radar.RadarDesign.snapPosition, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        HelpMessage("In order to correct for camera distortion of scene object closer to the corder of the screen , you can turn on this function to make snaps to the Top Left,Right, Bottom Left, Right, Middle Left, Right not distort");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Use Orthographic ForSide Snaps");
                                        Current_Selected3D_Radar.RadarDesign.UseOrthographicForSideSnaps = EditorGUILayout.Toggle("", Current_Selected3D_Radar.RadarDesign.UseOrthographicForSideSnaps, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();
                                        break;

                                }
                                #endregion

                                #region FrontIs Settings
                                HelpMessage("Determine what the front facing direction of the radar is ");
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Front is");
                                Current_Selected3D_Radar.RadarDesign.frontIs = (FrontIs)EditorGUILayout.EnumPopup("", Current_Selected3D_Radar.RadarDesign.frontIs, GUILayout.Width(220));

                                GUILayout.EndHorizontal();
                                #endregion

                                HelpMessage("Pading on the X axis");
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("X Pading");
                                Current_Selected3D_Radar.RadarDesign.xPadding = EditorGUILayout.FloatField("", Current_Selected3D_Radar.RadarDesign.xPadding, GUILayout.Width(220));
                                GUILayout.EndHorizontal();

                                HelpMessage("Pading on the Y axis");
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Y Pading");
                                Current_Selected3D_Radar.RadarDesign.yPadding = EditorGUILayout.FloatField("", Current_Selected3D_Radar.RadarDesign.yPadding, GUILayout.Width(220));
                                GUILayout.EndHorizontal();



                            }
                            Separator();

                            EditorGUILayout.Space();
                        }
                        #endregion


                        #region Design Sprites

                        #region Setting and creating Rotation Targets
                        HelpMessage("You are able to target layers blips or parts of your radar and have them rotate in various ways");

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Rotation Targets");
                        Current_Selected3D_Radar.RadarDesign.DesignsCount = Mathf.Clamp(EditorGUILayout.IntField("", Current_Selected3D_Radar.RadarDesign.DesignsCount, GUILayout.Width(60)), 0, 8000);
                        GUILayout.EndHorizontal();

                        Separator();

                        if (Event.current.keyCode == KeyCode.Return)
                        {
                            if (Current_Selected3D_Radar.RadarDesign.RotationTargets.Count < Current_Selected3D_Radar.RadarDesign.DesignsCount)
                                Current_Selected3D_Radar.RadarDesign.RotationTargets.AddRange(Enumerable.Repeat(default(RotationTarget), Current_Selected3D_Radar.RadarDesign.DesignsCount));
                            else
                                Current_Selected3D_Radar.RadarDesign.RotationTargets.RemoveRange(Current_Selected3D_Radar.RadarDesign.DesignsCount, Current_Selected3D_Radar.RadarDesign.RotationTargets.Count - Current_Selected3D_Radar.RadarDesign.DesignsCount);

                        }
                        #endregion

                        EditorGUILayout.Separator();
                        foreach (var rotationTarget in Current_Selected3D_Radar.RadarDesign.RotationTargets.Select((x, i) => new { value = x, index = i }))
                        {




                            try
                            {

                                string Target = (!rotationTarget.value.TargetedObject) ? (rotationTarget.value.target == TargetBlip.InstancedBlip) ? rotationTarget.value.InstancedTargetBlipname : "nothing " : rotationTarget.value.TargetedObject.name;
                                rotationTarget.value.ShowDesignSetings = EditorGUILayout.Foldout(rotationTarget.value.ShowDesignSetings, "Rotate " + Target);

                                GUILayout.BeginHorizontal();
                                GUILayout.Space(20);
                                GUILayout.BeginVertical();

                                if (rotationTarget.value.ShowDesignSetings)
                                {

                                    #region Draw Radar Design Interface
                                    HelpMessage("Here you will set the objet(blip or otherwise) which you wish to have rotate ");

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Rotate");
                                    rotationTarget.value.target = (TargetBlip)EditorGUILayout.EnumPopup(rotationTarget.value.target, GUILayout.Width(95));
                                    switch (rotationTarget.value.target)
                                    {

                                        case TargetBlip.ThisObject:

                                            rotationTarget.value.TargetedObject = (GameObject)EditorGUILayout.ObjectField(rotationTarget.value.TargetedObject, typeof(GameObject), true);

                                            break;
                                        case TargetBlip.InstancedBlip:

                                            rotationTarget.value.InstancedTargetBlipname = EditorGUILayout.TextField(rotationTarget.value.InstancedTargetBlipname);
                                            break;

                                    }



                                    rotationTarget.value.rotations = (Rotations)EditorGUILayout.EnumPopup(rotationTarget.value.rotations, GUILayout.Width(95));
                                    GUILayout.Label("to");

                                    rotationTarget.value.ObjectToTrack = (TargetObject)EditorGUILayout.EnumPopup(rotationTarget.value.ObjectToTrack, GUILayout.Width(95));

                                    switch (rotationTarget.value.ObjectToTrack)
                                    {
                                        case TargetObject.FindObject:
                                            rotationTarget.value.FindingName = EditorGUILayout.TextField(rotationTarget.value.FindingName);
                                            break;
                                        case TargetObject.ObectWithTag:
                                            rotationTarget.value.tag = EditorGUILayout.TextField(rotationTarget.value.tag);
                                            break;
                                        case TargetObject.ThisObject:
                                            rotationTarget.value.Target = (GameObject)EditorGUILayout.ObjectField(rotationTarget.value.Target, typeof(GameObject), true);

                                            break;
                                        case TargetObject.InstancedBlip:
                                            rotationTarget.value.InstancedObjectToTrackBlipName = EditorGUILayout.TextField(rotationTarget.value.InstancedObjectToTrackBlipName);
                                            break;

                                    }
                                    GUILayout.EndHorizontal();

                                    HelpMessage("This value should usually  set to 90 for sprite objects");

                                    GUILayout.Label("Added Rotation (usually, x is set to 90 for sprites)");
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("X");
                                    rotationTarget.value.AddedXRotation = EditorGUILayout.FloatField(Mathf.Clamp(rotationTarget.value.AddedXRotation, 0, Mathf.Infinity));
                                    GUILayout.Label("Y");
                                    rotationTarget.value.AddedYRotation = EditorGUILayout.FloatField(Mathf.Clamp(rotationTarget.value.AddedYRotation, 0, Mathf.Infinity));
                                    GUILayout.Label("Z");
                                    rotationTarget.value.AddedZRotation = EditorGUILayout.FloatField(Mathf.Clamp(rotationTarget.value.AddedZRotation, 0, Mathf.Infinity));
                                    GUILayout.EndHorizontal();

                                    HelpMessage("By what multiple will rotation be reduced");

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Damping");
                                    rotationTarget.value.RotationDamping = EditorGUILayout.FloatField(Mathf.Clamp(rotationTarget.value.RotationDamping, 1, Mathf.Infinity), GUILayout.Width(220));
                                    GUILayout.EndHorizontal();

                                    HelpMessage("Freze rotation on any axis");

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Freeze");
                                    rotationTarget.value.FreezeX = GUILayout.Toggle(rotationTarget.value.FreezeX, "X");
                                    rotationTarget.value.FreezeY = GUILayout.Toggle(rotationTarget.value.FreezeY, "Y");
                                    rotationTarget.value.FreezeZ = GUILayout.Toggle(rotationTarget.value.FreezeZ, "Z");
                                    GUILayout.EndHorizontal();


                                    HelpMessage("Replace the rotation on any axis of your target object with that of the targeted object");

                                    GUILayout.Label("Retargeted Rotation");
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("X");
                                    rotationTarget.value.retargetedXRotation = (RetargetX)EditorGUILayout.EnumPopup(rotationTarget.value.retargetedXRotation);
                                    GUILayout.Label("Y");
                                    rotationTarget.value.retargetedYRotation = (RetargetY)EditorGUILayout.EnumPopup(rotationTarget.value.retargetedYRotation);
                                    GUILayout.Label("Z");
                                    rotationTarget.value.retargetedZRotation = (RetargetZ)EditorGUILayout.EnumPopup(rotationTarget.value.retargetedZRotation);
                                    GUILayout.EndHorizontal();


                                    HelpMessage("set the value state of the retargeted rotation to be either positive or negative");

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Value State");
                                    rotationTarget.value.ValueState = (valueState)EditorGUILayout.EnumPopup(rotationTarget.value.ValueState, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();



                                    #endregion
                                }
                                GUILayout.EndVertical();
                                GUILayout.EndHorizontal();
                                Separator();
                                EditorGUILayout.Separator();
                            }
                            catch
                            {

                            }


                        }
                        #endregion


                        HelpMessage("Turn on 'Visualize'. This show you your radar settings.");
                        Current_Selected3D_Radar.RadarDesign.Visualize = GUILayout.Toggle(Current_Selected3D_Radar.RadarDesign.Visualize, "Visualize");


                        ShowHelpMessages = GUILayout.Toggle(ShowHelpMessages, "Show Help Messages");

                        HelpMessage("These help messages will help you to set up your radar");

                        GUILayout.EndScrollView();





                        break;
                    #endregion

                    #region Blip Design UI
                    case 1:

                        RadarBlipScrollPosition = EditorGUILayout.BeginScrollView(RadarBlipScrollPosition, false, false);

                        #region Setting Blips

                        HelpMessage("This blip ; if set to active will always appear at the center of your radar.");
                        Current_Selected3D_Radar._RadarCenterObject3D.State = Current_Selected3D_Radar._RadarCenterObject3D.IsActive ? Current_Selected3D_Radar._RadarCenterObject3D.Tag + " is " + " Active" : Current_Selected3D_Radar._RadarCenterObject3D.Tag + " is " + " Inactive";

                        GUILayout.BeginHorizontal();
                        Current_Selected3D_Radar._RadarCenterObject3D.ShowBLipSettings = EditorGUILayout.Foldout(Current_Selected3D_Radar._RadarCenterObject3D.ShowBLipSettings, Current_Selected3D_Radar._RadarCenterObject3D.State);
                        if (GUILayout.Button((Current_Selected3D_Radar._RadarCenterObject3D.IsActive) ? On : Off, "Label", GUILayout.Width(30))) Current_Selected3D_Radar._RadarCenterObject3D.IsActive = !Current_Selected3D_Radar._RadarCenterObject3D.IsActive;
                        GUILayout.EndHorizontal();
                        cursorchange();


                        if (Current_Selected3D_Radar._RadarCenterObject3D.ShowBLipSettings)
                        {
                            Separator();
                            EditorGUILayout.Space();

                            #region sprite
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginVertical();

                            HelpMessage("If using sprites as blips; set the Sprite and Material here");
                            Current_Selected3D_Radar._RadarCenterObject3D.ShowSpriteBlipSettings = EditorGUILayout.Foldout(Current_Selected3D_Radar._RadarCenterObject3D.ShowSpriteBlipSettings, Current_Selected3D_Radar._RadarCenterObject3D.Tag + " Sprite");

                            if (Current_Selected3D_Radar._RadarCenterObject3D.ShowSpriteBlipSettings)
                            {
                                #region Sprite Blip Design
                                GUILayout.BeginHorizontal();

                                GUILayout.BeginVertical();

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Sprite");
                                Current_Selected3D_Radar._RadarCenterObject3D.icon = (Sprite)EditorGUILayout.ObjectField(Current_Selected3D_Radar._RadarCenterObject3D.icon, typeof(Sprite), false, GUILayout.Width((Current_Selected3D_Radar._RadarCenterObject3D.icon) ? 170 : 220));
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Material");
                                Current_Selected3D_Radar._RadarCenterObject3D.SpriteMaterial = (Material)EditorGUILayout.ObjectField("", Current_Selected3D_Radar._RadarCenterObject3D.SpriteMaterial, typeof(Material), true, GUILayout.Width((Current_Selected3D_Radar._RadarCenterObject3D.icon) ? 170 : 220));
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Colour");
                                Current_Selected3D_Radar._RadarCenterObject3D.colour = EditorGUILayout.ColorField("", Current_Selected3D_Radar._RadarCenterObject3D.colour, GUILayout.Width((Current_Selected3D_Radar._RadarCenterObject3D.icon) ? 170 : 220));
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Order In Layer");
                                Current_Selected3D_Radar._RadarCenterObject3D.OrderInLayer = EditorGUILayout.IntField(Current_Selected3D_Radar._RadarCenterObject3D.OrderInLayer, GUILayout.Width(220));
                                GUILayout.EndHorizontal();

                                GUILayout.EndVertical();

                                try
                                {
                                    GUILayout.Box(Current_Selected3D_Radar._RadarCenterObject3D.icon.texture, GUILayout.Height(50), GUILayout.Width(50));
                                }
                                catch { }

                                GUILayout.EndHorizontal();
                                #endregion
                            }
                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();

                            Separator();

                            EditorGUILayout.Space();
                            #endregion

                            #region Mesh
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginVertical();
                            HelpMessage("If using mesh as blips; set the mesh and materials here");
                            Current_Selected3D_Radar._RadarCenterObject3D.ShowMeshBlipSettings = EditorGUILayout.Foldout(Current_Selected3D_Radar._RadarCenterObject3D.ShowMeshBlipSettings, Current_Selected3D_Radar._RadarCenterObject3D.Tag + " Mesh");
                            if (Current_Selected3D_Radar._RadarCenterObject3D.ShowMeshBlipSettings)
                            {
                                #region MeshBlipDesign

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Mesh");
                                Current_Selected3D_Radar._RadarCenterObject3D.mesh = (Mesh)EditorGUILayout.ObjectField(Current_Selected3D_Radar._RadarCenterObject3D.mesh, typeof(Mesh), false, GUILayout.Width(220));
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Material Count");
                                Current_Selected3D_Radar._RadarCenterObject3D.MatCount = Mathf.Clamp(EditorGUILayout.IntField("", Current_Selected3D_Radar._RadarCenterObject3D.MatCount, GUILayout.MaxWidth(200), GUILayout.Width(220)), 0, 8000);
                                GUILayout.EndHorizontal();

                                if (Event.current.keyCode == KeyCode.Return)
                                {
                                    Array.Resize(ref Current_Selected3D_Radar._RadarCenterObject3D.MeshMaterials, Current_Selected3D_Radar._RadarCenterObject3D.MatCount);

                                }


                                GUILayout.BeginVertical();
                                for (int i = 0; i < Current_Selected3D_Radar._RadarCenterObject3D.MeshMaterials.Count(); i++)
                                {
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Material " + i);
                                    try
                                    {
                                        Current_Selected3D_Radar._RadarCenterObject3D.MeshMaterials[i] = (Material)EditorGUILayout.ObjectField(Current_Selected3D_Radar._RadarCenterObject3D.MeshMaterials[i], typeof(Material), false, GUILayout.Width(220));
                                    }
                                    catch { }
                                    GUILayout.EndHorizontal();
                                }
                                GUILayout.EndVertical();


                                #endregion
                            }
                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();

                            Separator();

                            EditorGUILayout.Space();
                            #endregion

                            #region Prefab

                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginVertical();
                            HelpMessage("If using prefab as blips; se the prefab here");
                            Current_Selected3D_Radar._RadarCenterObject3D.ShowPrefabBlipSettings = EditorGUILayout.Foldout(Current_Selected3D_Radar._RadarCenterObject3D.ShowPrefabBlipSettings, Current_Selected3D_Radar._RadarCenterObject3D.Tag + " Prefab");
                            if (Current_Selected3D_Radar._RadarCenterObject3D.ShowPrefabBlipSettings)
                            {
                                #region PrefabBLipDesign
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Prefab");
                                Current_Selected3D_Radar._RadarCenterObject3D.prefab = (Transform)EditorGUILayout.ObjectField("", Current_Selected3D_Radar._RadarCenterObject3D.prefab, typeof(Transform), true, GUILayout.Width(220));
                                GUILayout.EndHorizontal();

                                #endregion
                            }
                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();

                            Separator();

                            EditorGUILayout.Space();
                            #endregion

                            #region Tracking Line
                            HelpMessage("Tracking lines allows you to visualize the y distance of your blips from the radar");
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginVertical();


                            string trackingLineState = (Current_Selected3D_Radar._RadarCenterObject3D.UseTrackingLine) ? "Active" : "Inactive";

                            GUILayout.BeginHorizontal();
                            Current_Selected3D_Radar._RadarCenterObject3D.ShowTrackingLineSettings = EditorGUILayout.Foldout(Current_Selected3D_Radar._RadarCenterObject3D.ShowTrackingLineSettings, "Tracking Line is " + trackingLineState);
                            if (GUILayout.Button((Current_Selected3D_Radar._RadarCenterObject3D.UseTrackingLine) ? On : Off, "Label", GUILayout.Width(20))) Current_Selected3D_Radar._RadarCenterObject3D.UseTrackingLine = !Current_Selected3D_Radar._RadarCenterObject3D.UseTrackingLine;
                            GUILayout.EndHorizontal();
                            cursorchange();

                            if (Current_Selected3D_Radar._RadarCenterObject3D.ShowTrackingLineSettings)
                            {
                                HelpMessage("Set the Colour and Material here. You can set any material");
                                #region Tracking Line Design
                                GUILayout.BeginVertical();

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Material");
                                Current_Selected3D_Radar._RadarCenterObject3D.TrackingLineMaterial = (Material)EditorGUILayout.ObjectField("", Current_Selected3D_Radar._RadarCenterObject3D.TrackingLineMaterial, typeof(Material), true, GUILayout.Width(220));
                                GUILayout.EndHorizontal();


                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Start Colour");
                                Current_Selected3D_Radar._RadarCenterObject3D.TrackingLineStartColour = EditorGUILayout.ColorField("", Current_Selected3D_Radar._RadarCenterObject3D.TrackingLineStartColour, GUILayout.Width(220));
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("End Colour");
                                Current_Selected3D_Radar._RadarCenterObject3D.TrackingLineEndColour = EditorGUILayout.ColorField("", Current_Selected3D_Radar._RadarCenterObject3D.TrackingLineEndColour, GUILayout.Width(220));
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Scale");
                                GUILayout.Space(153);
                                Current_Selected3D_Radar._RadarCenterObject3D.TrackingLineDimention = EditorGUILayout.FloatField(" ", Current_Selected3D_Radar._RadarCenterObject3D.TrackingLineDimention, GUILayout.MaxWidth(Screen.width));
                                GUILayout.EndHorizontal();


                                #region BaseTracker Design
                                HelpMessage("Base Trackers are sprites which appear that the base of the reacking line and are optional");
                                GUILayout.BeginHorizontal();
                                Current_Selected3D_Radar._RadarCenterObject3D.ShowBaseTrackerSettings = EditorGUILayout.Foldout(Current_Selected3D_Radar._RadarCenterObject3D.ShowBaseTrackerSettings, "Base Tracker settings");
                                if (GUILayout.Button((Current_Selected3D_Radar._RadarCenterObject3D.UseBaseTracker) ? On : Off, "Label", GUILayout.Width(20))) Current_Selected3D_Radar._RadarCenterObject3D.UseBaseTracker = !Current_Selected3D_Radar._RadarCenterObject3D.UseBaseTracker;
                                GUILayout.EndHorizontal();
                                cursorchange();
                                if (Current_Selected3D_Radar._RadarCenterObject3D.ShowBaseTrackerSettings)
                                {
                                    GUILayout.BeginHorizontal();
                                    GUILayout.BeginVertical();
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Base Tracker");
                                    Current_Selected3D_Radar._RadarCenterObject3D.BaseTracker = (Sprite)EditorGUILayout.ObjectField(Current_Selected3D_Radar._RadarCenterObject3D.BaseTracker, typeof(Sprite), false, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Material");
                                    Current_Selected3D_Radar._RadarCenterObject3D.BaseTrackerMaterial = (Material)EditorGUILayout.ObjectField("", Current_Selected3D_Radar._RadarCenterObject3D.BaseTrackerMaterial, typeof(Material), true, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Colour");
                                    Current_Selected3D_Radar._RadarCenterObject3D.BaseTrackerColour = EditorGUILayout.ColorField("", Current_Selected3D_Radar._RadarCenterObject3D.BaseTrackerColour, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Size");
                                    Current_Selected3D_Radar._RadarCenterObject3D.BaseTrackerSize = EditorGUILayout.FloatField("", Current_Selected3D_Radar._RadarCenterObject3D.BaseTrackerSize, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();

                                    GUILayout.EndVertical();

                                    try
                                    {
                                        GUILayout.Box(Current_Selected3D_Radar._RadarCenterObject3D.BaseTracker.texture, GUILayout.Height(50), GUILayout.Width(50));
                                    }
                                    catch
                                    {
                                        // nothing
                                    }
                                    GUILayout.EndHorizontal();


                                }
                                #endregion

                                GUILayout.EndVertical();

                                #endregion
                            }
                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();
                            Separator();
                            EditorGUILayout.Separator();
                            #endregion

                            #region Additional Options

                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginVertical();

                            HelpMessage("Displaying additional options for your blip");
                            Current_Selected3D_Radar._RadarCenterObject3D.ShowAdditionalOptions = EditorGUILayout.Foldout(Current_Selected3D_Radar._RadarCenterObject3D.ShowAdditionalOptions, "AdditionalOptions");
                            if (Current_Selected3D_Radar._RadarCenterObject3D.ShowAdditionalOptions)
                            {
                                HelpMessage("When eabled all " + Current_Selected3D_Radar._RadarCenterObject3D.Tag + "blips will not disppear when at they pass the bounderies of the radar, but will remain at the edge and will be scaled based on its distance from the center object");

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Always Show " + Current_Selected3D_Radar._RadarCenterObject3D.Tag + " in radar");
                                Current_Selected3D_Radar._RadarCenterObject3D.AlwaysShowCenterObject = GUILayout.Toggle(Current_Selected3D_Radar._RadarCenterObject3D.AlwaysShowCenterObject, "", GUILayout.Width(220));
                                GUILayout.EndHorizontal();

                            }

                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();

                            Separator();

                            EditorGUILayout.Space();

                            #endregion

                            /* #region Optimization 

                             GUILayout.BeginHorizontal();
                             GUILayout.Space(20);
                             GUILayout.BeginVertical();

                             HelpMessage("Options for optimization the radar proceses");
                             GUILayout.BeginHorizontal();
                             Current_Selected3D_Radar._RadarCenterObject3D.ShowOptimizationSettings = EditorGUILayout.Foldout(Current_Selected3D_Radar._RadarCenterObject3D.ShowOptimizationSettings, "Optimization Options");
                             GUILayout.Box(Optimizeicon, "Label", GUILayout.Width(120), GUILayout.Height(20));
                             GUILayout.EndHorizontal();
                             if (Current_Selected3D_Radar._RadarCenterObject3D.ShowOptimizationSettings)
                             {


                                     HelpMessage("Requires that doCenterObjectInstanceObjectCheck be called whenever you want to manually search for a enter object");
                                     GUILayout.BeginHorizontal();
                                     GUILayout.Label("Require Instance Object Check");
                                     Current_Selected3D_Radar._RadarCenterObject3D.RequireInstanceObjectCheck = EditorGUILayout.Toggle(Current_Selected3D_Radar._RadarCenterObject3D.RequireInstanceObjectCheck, GUILayout.Width(220));
                                     GUILayout.EndHorizontal();




                             }

                             GUILayout.EndVertical();
                             GUILayout.EndHorizontal();

                             Separator();

                             EditorGUILayout.Space();

                             #endregion
                             */


                            #region Scale And Rotation Settings


                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.BeginVertical();

                            Current_Selected3D_Radar._RadarCenterObject3D.ShowGeneralSettings = EditorGUILayout.Foldout(Current_Selected3D_Radar._RadarCenterObject3D.ShowGeneralSettings, "Rotation and Scale");
                            if (Current_Selected3D_Radar._RadarCenterObject3D.ShowGeneralSettings)
                            {
                                #region Blip Scale Settings
                                if (!Current_Selected3D_Radar._RadarCenterObject3D.CenterObjectCanScaleByDistance)
                                {
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Scale");
                                    GUILayout.Space(153);
                                    Current_Selected3D_Radar._RadarCenterObject3D.BlipSize = EditorGUILayout.FloatField(" ", Current_Selected3D_Radar._RadarCenterObject3D.BlipSize, GUILayout.MaxWidth(Screen.width));
                                    GUILayout.EndHorizontal();

                                }

                                /*  HelpMessage("Your Center Blip will always shrink when moving to the edge of the radar if this is not checkd");
                                  GUILayout.BeginHorizontal();
                                  GUILayout.Label("Autoscale Only At Border");
                                  Current_Selected3D_Radar._RadarCenterObject3D.AutoScaleOnlyAtBorder = GUILayout.Toggle(Current_Selected3D_Radar._RadarCenterObject3D.AutoScaleOnlyAtBorder, "", GUILayout.Width(220));
                                  GUILayout.EndHorizontal();

                                  HelpMessage("this value determines , be how much the blip will scale over distance");
                                  GUILayout.BeginHorizontal();
                                  GUILayout.Label("AutoScale Factor");
                                  Current_Selected3D_Radar._RadarCenterObject3D.AutoScaleFactor = EditorGUILayout.FloatField("", Current_Selected3D_Radar._RadarCenterObject3D.AutoScaleFactor, GUILayout.Width(220));
                                  GUILayout.EndHorizontal();*/


                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Scale By Distance");
                                Current_Selected3D_Radar._RadarCenterObject3D.CenterObjectCanScaleByDistance = GUILayout.Toggle(Current_Selected3D_Radar._RadarCenterObject3D.CenterObjectCanScaleByDistance, "", GUILayout.Width(220));
                                GUILayout.EndHorizontal();

                                if (Current_Selected3D_Radar._RadarCenterObject3D.CenterObjectCanScaleByDistance)
                                {
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Min Scale");
                                    Current_Selected3D_Radar._RadarCenterObject3D.BlipMinSize = EditorGUILayout.FloatField("", Current_Selected3D_Radar._RadarCenterObject3D.BlipMinSize, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Max Scale");
                                    Current_Selected3D_Radar._RadarCenterObject3D.BlipMaxSize = EditorGUILayout.FloatField("", Current_Selected3D_Radar._RadarCenterObject3D.BlipMaxSize, GUILayout.Width(220));
                                    GUILayout.EndHorizontal();



                                }
                                #endregion


                                #region Blip Rotation Settings

                                HelpMessage("Use custom rotation allows yo uto set a static rotation for all of these blip types");
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Use Custom Rotation");
                                Current_Selected3D_Radar._RadarCenterObject3D.UseCustomRotation = GUILayout.Toggle(Current_Selected3D_Radar._RadarCenterObject3D.UseCustomRotation, "", GUILayout.Width(220));
                                GUILayout.EndHorizontal();

                                if (Current_Selected3D_Radar._RadarCenterObject3D.UseCustomRotation && !Current_Selected3D_Radar._RadarCenterObject3D.IsTrackRotation)
                                {
                                    HelpMessage("Set a X, Y and Z rotation for this blip type");

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Rotation");
                                    GUILayout.Label("X");
                                    Current_Selected3D_Radar._RadarCenterObject3D.CustomXRotation = EditorGUILayout.FloatField(Current_Selected3D_Radar._RadarCenterObject3D.CustomXRotation);
                                    GUILayout.Label("Y");
                                    Current_Selected3D_Radar._RadarCenterObject3D.CustomYRotation = EditorGUILayout.FloatField(Current_Selected3D_Radar._RadarCenterObject3D.CustomYRotation);
                                    GUILayout.Label("Z");
                                    Current_Selected3D_Radar._RadarCenterObject3D.CustomZRotation = EditorGUILayout.FloatField(Current_Selected3D_Radar._RadarCenterObject3D.CustomZRotation);
                                    GUILayout.EndHorizontal();
                                }

                                HelpMessage("Track Rotation allows for the blip to rotate and match the rotation of the tracked object");
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Track Rotation");
                                Current_Selected3D_Radar._RadarCenterObject3D.IsTrackRotation = GUILayout.Toggle(Current_Selected3D_Radar._RadarCenterObject3D.IsTrackRotation, "", GUILayout.Width(220));
                                GUILayout.EndHorizontal();


                                if (Current_Selected3D_Radar._RadarCenterObject3D.IsTrackRotation && !Current_Selected3D_Radar._RadarCenterObject3D.UseCustomRotation)
                                {
                                    HelpMessage("Freeze rotation through the x,y, or z axis");

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Freeze");
                                    Current_Selected3D_Radar._RadarCenterObject3D.lockX = GUILayout.Toggle(Current_Selected3D_Radar._RadarCenterObject3D.lockX, "X");
                                    Current_Selected3D_Radar._RadarCenterObject3D.lockY = GUILayout.Toggle(Current_Selected3D_Radar._RadarCenterObject3D.lockY, "Y");
                                    Current_Selected3D_Radar._RadarCenterObject3D.lockZ = GUILayout.Toggle(Current_Selected3D_Radar._RadarCenterObject3D.lockZ, "Z");
                                    GUILayout.EndHorizontal();
                                }
                                #endregion
                                if (Current_Selected3D_Radar._RadarCenterObject3D.IsTrackRotation && Current_Selected3D_Radar._RadarCenterObject3D.UseCustomRotation)
                                {
                                    EditorGUILayout.HelpBox("Do not use 'Track Rotation' and 'Custom Rotation at the same time'", MessageType.Warning);
                                }
                            }
                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();

                            Separator();

                            EditorGUILayout.Space();

                            #region Universal Settings
                            HelpMessage("With the current settings,If this blip is active, it will represent your gameobject with the tag " + Current_Selected3D_Radar._RadarCenterObject3D.Tag + " on the layer " + Current_Selected3D_Radar._RadarCenterObject3D.Layer + " and will be at scale " + Current_Selected3D_Radar._RadarCenterObject3D.BlipSize);

                            GUILayout.BeginHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Create blip");
                            Current_Selected3D_Radar._RadarCenterObject3D._CreateBlipAs = (CreateBlipAs)EditorGUILayout.EnumPopup(Current_Selected3D_Radar._RadarCenterObject3D._CreateBlipAs, GUILayout.Width(100));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("With Tag"); Current_Selected3D_Radar._RadarCenterObject3D.Tag = EditorGUILayout.TagField(Current_Selected3D_Radar._RadarCenterObject3D.Tag);
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Label("On Layer"); Current_Selected3D_Radar._RadarCenterObject3D.Layer = EditorGUILayout.LayerField(Current_Selected3D_Radar._RadarCenterObject3D.Layer);
                            GUILayout.EndHorizontal();

                            GUILayout.EndHorizontal();
                            #endregion


                        }
                        #endregion
                        Separator();
                        GUILayout.Space(10);


                        #region Setting and creating Blips
                        GUILayout.BeginHorizontal();

                        GUILayout.Label("Number Of Other Blip Types");
                        Current_Selected3D_Radar.RadarDesign.Count = Mathf.Clamp(EditorGUILayout.IntField("", Current_Selected3D_Radar.RadarDesign.Count, GUILayout.Width(60)), 0, 8000);

                        if (Event.current.keyCode == KeyCode.Return)
                        {
                            if (Current_Selected3D_Radar.Blips.Count < Current_Selected3D_Radar.RadarDesign.Count)
                                Current_Selected3D_Radar.Blips.AddRange(Enumerable.Repeat(default(RadarBlips3D), Current_Selected3D_Radar.RadarDesign.Count));
                            else
                                Current_Selected3D_Radar.Blips.RemoveRange(Current_Selected3D_Radar.RadarDesign.Count, Current_Selected3D_Radar.Blips.Count - Current_Selected3D_Radar.RadarDesign.Count);


                        }
                        GUILayout.EndHorizontal();
                        #endregion
                        Separator();


                        #region Setting and creating All other Blips
                        EditorGUILayout.Separator();

                        foreach (var _Blip in Current_Selected3D_Radar.Blips.Select((x, i) => new { Value = x as RadarBlips3D, Index = i }))
                        {
                            try
                            {
                                _Blip.Value.State = _Blip.Value.IsActive ? _Blip.Value.Tag + " is " + "Active" : _Blip.Value.Tag + " is " + "Inactive";

                                GUILayout.BeginHorizontal();
                                _Blip.Value.ShowBLipSettings = EditorGUILayout.Foldout(_Blip.Value.ShowBLipSettings, _Blip.Value.State);
                                if (GUILayout.Button((_Blip.Value.IsActive) ? On : Off, "Label", GUILayout.Width(30))) _Blip.Value.IsActive = !_Blip.Value.IsActive;
                                GUILayout.EndHorizontal();
                                cursorchange();


                                if (_Blip.Value.ShowBLipSettings)
                                {
                                    Separator();
                                    EditorGUILayout.Space();

                                    #region Sprite
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(20);
                                    GUILayout.BeginVertical();



                                    HelpMessage("If using sprites as blips; set the Sprite and Material here");

                                    _Blip.Value.ShowSpriteBlipSettings = EditorGUILayout.Foldout(_Blip.Value.ShowSpriteBlipSettings, _Blip.Value.Tag + " Sprite");
                                    if (_Blip.Value.ShowSpriteBlipSettings)
                                    {

                                        #region Sprite Blip Design
                                        GUILayout.BeginHorizontal();

                                        GUILayout.BeginVertical();

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Sprite");
                                        _Blip.Value.icon = (Sprite)EditorGUILayout.ObjectField(_Blip.Value.icon, typeof(Sprite), false, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Material");
                                        _Blip.Value.SpriteMaterial = (Material)EditorGUILayout.ObjectField(_Blip.Value.SpriteMaterial, typeof(Material), true, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Colour");
                                        _Blip.Value.colour = EditorGUILayout.ColorField("", _Blip.Value.colour, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Order In Layer");
                                        _Blip.Value.OrderInLayer = EditorGUILayout.IntField(_Blip.Value.OrderInLayer, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        GUILayout.EndVertical();

                                        try
                                        {
                                            GUILayout.Box(_Blip.Value.icon.texture, GUILayout.Height(50), GUILayout.Width(50));
                                        }
                                        catch { }
                                        GUILayout.EndHorizontal();
                                        #endregion


                                    }
                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();

                                    Separator();

                                    EditorGUILayout.Space();
                                    #endregion

                                    #region Mesh
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(20);
                                    GUILayout.BeginVertical();
                                    HelpMessage("If using meshes as blips; set the Mesh and Materials here, Optional LODs and the distances at which each mesh is rendered in the radar");

                                    _Blip.Value.ShowMeshBlipSettings = EditorGUILayout.Foldout(_Blip.Value.ShowMeshBlipSettings, _Blip.Value.Tag + " Mesh");
                                    if (_Blip.Value.ShowMeshBlipSettings)
                                    {
                                        #region MeshBlipDesign

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Mesh");
                                        _Blip.Value.mesh = (Mesh)EditorGUILayout.ObjectField(_Blip.Value.mesh, typeof(Mesh), false, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        #region LOD area

                                        string statement = (_Blip.Value.UseLOD) ? " is on" : " is off";

                                        GUILayout.BeginHorizontal();
                                        _Blip.Value.ShowLODSettings = EditorGUILayout.Foldout(_Blip.Value.ShowLODSettings, _Blip.Value.Tag + " LOD" + statement);
                                        if (GUILayout.Button((_Blip.Value.UseLOD) ? On : Off, "Label", GUILayout.Width(20))) _Blip.Value.UseLOD = !_Blip.Value.UseLOD;
                                        GUILayout.EndHorizontal();
                                        cursorchange();

                                        if (_Blip.Value.ShowLODSettings)
                                        {
                                            Separator();
                                            EditorGUILayout.Separator();
                                            #region LowSettings
                                            _Blip.Value.ShowLowMeshSetings = EditorGUILayout.Foldout(_Blip.Value.ShowLowMeshSetings, "Low");
                                            if (_Blip.Value.ShowLowMeshSetings)
                                            {
                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Mesh");
                                                _Blip.Value.Low = (Mesh)EditorGUILayout.ObjectField(_Blip.Value.Low, typeof(Mesh), false, GUILayout.Width(220));
                                                GUILayout.EndHorizontal();

                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Distance");
                                                _Blip.Value.LowDistance = EditorGUILayout.FloatField(_Blip.Value.LowDistance, GUILayout.Width(220));
                                                GUILayout.EndHorizontal();
                                            }

                                            #endregion
                                            Separator();
                                            EditorGUILayout.Separator();
                                            #region MediumDistance
                                            _Blip.Value.ShowMediumMeshSettings = EditorGUILayout.Foldout(_Blip.Value.ShowMediumMeshSettings, "Medium");
                                            if (_Blip.Value.ShowMediumMeshSettings)
                                            {
                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Mesh");
                                                _Blip.Value.Medium = (Mesh)EditorGUILayout.ObjectField(_Blip.Value.Medium, typeof(Mesh), false, GUILayout.Width(220));
                                                GUILayout.EndHorizontal();

                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Distance");
                                                _Blip.Value.MediumDistance = EditorGUILayout.FloatField(_Blip.Value.MediumDistance, GUILayout.Width(220));
                                                GUILayout.EndHorizontal();
                                            }

                                            #endregion
                                            Separator();
                                            EditorGUILayout.Separator();
                                            #region HighDistance
                                            _Blip.Value.ShowHighMeshSettings = EditorGUILayout.Foldout(_Blip.Value.ShowHighMeshSettings, "High");
                                            if (_Blip.Value.ShowHighMeshSettings)
                                            {
                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Mesh");
                                                _Blip.Value.High = (Mesh)EditorGUILayout.ObjectField(_Blip.Value.High, typeof(Mesh), false, GUILayout.Width(220));
                                                GUILayout.EndHorizontal();

                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Distance");
                                                _Blip.Value.HighDistance = EditorGUILayout.FloatField(_Blip.Value.HighDistance, GUILayout.Width(220));
                                                GUILayout.EndHorizontal();
                                            }

                                            #endregion
                                            Separator();
                                            EditorGUILayout.Separator();


                                        }

                                        #endregion


                                        #region Define how many materials the mesh uses
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Material Count");
                                        _Blip.Value.MatCount = Mathf.Clamp(EditorGUILayout.IntField("", _Blip.Value.MatCount, GUILayout.MaxWidth(200), GUILayout.Width(220)), 0, 8000);
                                        GUILayout.EndHorizontal();
                                        #endregion

                                        if (Event.current.keyCode == KeyCode.Return)
                                        {
                                            Array.Resize(ref _Blip.Value.MeshMaterials, _Blip.Value.MatCount);
                                        }

                                        GUILayout.BeginVertical();
                                        for (int i = 0; i < _Blip.Value.MeshMaterials.Count(); i++)
                                        {
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Material " + i);
                                            try
                                            {

                                                _Blip.Value.MeshMaterials[i] = (Material)EditorGUILayout.ObjectField(_Blip.Value.MeshMaterials[i], typeof(Material), false, GUILayout.Width(220));
                                            }
                                            catch { }
                                            GUILayout.EndHorizontal();

                                        }
                                        GUILayout.EndVertical();
                                        #endregion


                                    }
                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();
                                    Separator();

                                    EditorGUILayout.Space();
                                    #endregion

                                    #region prefab
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(20);
                                    GUILayout.BeginVertical();
                                    HelpMessage("If using sprites as blips; set the Prefab here");

                                    _Blip.Value.ShowPrefabBlipSettings = EditorGUILayout.Foldout(_Blip.Value.ShowPrefabBlipSettings, _Blip.Value.Tag + " Prefab");
                                    if (_Blip.Value.ShowPrefabBlipSettings)
                                    {
                                        #region Prefab BLip Design
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Prefab");
                                        _Blip.Value.prefab = (Transform)EditorGUILayout.ObjectField("", _Blip.Value.prefab, typeof(Transform), true, GUILayout.Width(220));
                                        GUILayout.EndVertical();
                                        #endregion
                                    }
                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();

                                    Separator();

                                    EditorGUILayout.Separator();
                                    #endregion

                                    #region Tracking Line
                                    HelpMessage("Tracking lines allows you to visualize the y distance of your blips from the radar");
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(20);
                                    GUILayout.BeginVertical();


                                    string trackingLineState = (Current_Selected3D_Radar.Blips[_Blip.Index].UseTrackingLine) ? "Active" : "Inactive";

                                    GUILayout.BeginHorizontal();
                                    Current_Selected3D_Radar.Blips[_Blip.Index].ShowTrackingLineSettings = EditorGUILayout.Foldout(Current_Selected3D_Radar.Blips[_Blip.Index].ShowTrackingLineSettings, "Tracking Line is " + trackingLineState);
                                    if (GUILayout.Button((_Blip.Value.UseTrackingLine) ? On : Off, "Label", GUILayout.Width(20))) _Blip.Value.UseTrackingLine = !_Blip.Value.UseTrackingLine;
                                    GUILayout.EndHorizontal();
                                    cursorchange();

                                    if (Current_Selected3D_Radar.Blips[_Blip.Index].ShowTrackingLineSettings)
                                    {
                                        HelpMessage("Set the Colour and Material here. You can set any material");
                                        #region Tracking Line Design
                                        GUILayout.BeginVertical();

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Material");
                                        Current_Selected3D_Radar.Blips[_Blip.Index].TrackingLineMaterial = (Material)EditorGUILayout.ObjectField("", Current_Selected3D_Radar.Blips[_Blip.Index].TrackingLineMaterial, typeof(Material), true, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();


                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Start Colour");
                                        Current_Selected3D_Radar.Blips[_Blip.Index].TrackingLineStartColour = EditorGUILayout.ColorField("", Current_Selected3D_Radar.Blips[_Blip.Index].TrackingLineStartColour, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("End Colour");
                                        Current_Selected3D_Radar.Blips[_Blip.Index].TrackingLineEndColour = EditorGUILayout.ColorField("", Current_Selected3D_Radar.Blips[_Blip.Index].TrackingLineEndColour, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Scale");
                                        GUILayout.Space(153);
                                        Current_Selected3D_Radar.Blips[_Blip.Index].TrackingLineDimention = EditorGUILayout.FloatField(" ", Current_Selected3D_Radar.Blips[_Blip.Index].TrackingLineDimention, GUILayout.MaxWidth(Screen.width));
                                        GUILayout.EndHorizontal();


                                        #region BaseTracker Design
                                        HelpMessage("Base Trackers are sprites which appear that the base of the reacking line and are optional");
                                        GUILayout.BeginHorizontal();
                                        Current_Selected3D_Radar.Blips[_Blip.Index].ShowBaseTrackerSettings = EditorGUILayout.Foldout(Current_Selected3D_Radar.Blips[_Blip.Index].ShowBaseTrackerSettings, "Base Tracker settings");
                                        if (GUILayout.Button((_Blip.Value.UseBaseTracker) ? On : Off, "Label", GUILayout.Width(20))) _Blip.Value.UseBaseTracker = !_Blip.Value.UseBaseTracker;
                                        GUILayout.EndHorizontal();
                                        cursorchange();
                                        if (Current_Selected3D_Radar.Blips[_Blip.Index].ShowBaseTrackerSettings)
                                        {
                                            GUILayout.BeginHorizontal();
                                            GUILayout.BeginVertical();
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Base Tracker");
                                            _Blip.Value.BaseTracker = (Sprite)EditorGUILayout.ObjectField(_Blip.Value.BaseTracker, typeof(Sprite), false, GUILayout.Width(220));
                                            GUILayout.EndHorizontal();

                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Material");
                                            Current_Selected3D_Radar.Blips[_Blip.Index].BaseTrackerMaterial = (Material)EditorGUILayout.ObjectField("", Current_Selected3D_Radar.Blips[_Blip.Index].BaseTrackerMaterial, typeof(Material), true, GUILayout.Width(220));
                                            GUILayout.EndHorizontal();

                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Colour");
                                            Current_Selected3D_Radar.Blips[_Blip.Index].BaseTrackerColour = EditorGUILayout.ColorField("", Current_Selected3D_Radar.Blips[_Blip.Index].BaseTrackerColour, GUILayout.Width(220));
                                            GUILayout.EndHorizontal();

                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Size");
                                            Current_Selected3D_Radar.Blips[_Blip.Index].BaseTrackerSize = EditorGUILayout.FloatField("", Current_Selected3D_Radar.Blips[_Blip.Index].BaseTrackerSize, GUILayout.Width(220));
                                            GUILayout.EndHorizontal();

                                            GUILayout.EndVertical();

                                            try
                                            {
                                                GUILayout.Box(_Blip.Value.BaseTracker.texture, GUILayout.Height(50),
                                                    GUILayout.Width(50));
                                            }
                                            catch
                                            {
                                                // nothing

                                            }
                                            GUILayout.EndHorizontal();


                                        }
                                        #endregion

                                        GUILayout.EndVertical();

                                        #endregion
                                    }
                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();
                                    Separator();
                                    EditorGUILayout.Separator();
                                    #endregion

                                    #region Additional Options
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(20);
                                    GUILayout.BeginVertical();

                                    HelpMessage("Displaying additional options for your blip");
                                    Current_Selected3D_Radar.Blips[_Blip.Index].ShowAdditionalOptions = EditorGUILayout.Foldout(Current_Selected3D_Radar.Blips[_Blip.Index].ShowAdditionalOptions, "Additional Options");
                                    if (Current_Selected3D_Radar.Blips[_Blip.Index].ShowAdditionalOptions)
                                    {
                                        HelpMessage("When eabled all " + _Blip.Value.Tag + "blips will not disppear when at they pass the bounderies of the radar, but will remain at the edge and will be scaled based on its distance from the center object");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Always Show " + _Blip.Value.Tag + " in radar");
                                        Current_Selected3D_Radar.Blips[_Blip.Index].AlwaysShowBlipsInRadarSpace = GUILayout.Toggle(Current_Selected3D_Radar.Blips[_Blip.Index].AlwaysShowBlipsInRadarSpace, "", GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        HelpMessage("True by default, ensures that blips do not go under the radar");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Always show " + _Blip.Value.Tag + " above the radar plane");
                                        Current_Selected3D_Radar.Blips[_Blip.Index].KeepBlipsAboveRadarPlane = GUILayout.Toggle(Current_Selected3D_Radar.Blips[_Blip.Index].KeepBlipsAboveRadarPlane, "", GUILayout.Width(220));
                                        GUILayout.EndHorizontal();
                                    }


                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();

                                    Separator();

                                    EditorGUILayout.Space();
                                    #endregion

                                    #region Optimization 

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(20);
                                    GUILayout.BeginVertical();

                                    HelpMessage("Options for optimization the radar proceses");
                                    GUILayout.BeginHorizontal();
                                    Current_Selected3D_Radar.Blips[_Blip.Index].ShowOptimizationSettings = EditorGUILayout.Foldout(Current_Selected3D_Radar.Blips[_Blip.Index].ShowOptimizationSettings, "Optimization Options");
                                    GUILayout.Box(Optimizeicon, "Label", GUILayout.Width(120), GUILayout.Height(20));
                                    GUILayout.EndHorizontal();
                                    if (Current_Selected3D_Radar.Blips[_Blip.Index].ShowOptimizationSettings)
                                    {
                                        if (Current_Selected3D_Radar.Blips[_Blip.Index].optimization.objectFindingMethod == ObjectFindingMethod.Pooling)
                                        {
                                            HelpMessage("If you are spawning any new objects into the scene then call radar3D.DoInstanceObjectCheck() at instance or at the end of instancing");
                                        }


                                        if (Current_Selected3D_Radar.Blips[_Blip.Index].optimization.objectFindingMethod != ObjectFindingMethod.Recursive)
                                        {
                                            HelpMessage("This requires that you call ' _3DRadar.doInstanceObjectCheck() whenever you want to make the radar search for objects to create blips from. This can also be used to icrease your internal pool size if you need to track more objects'");

                                            HelpMessage(" If you know exactly ow many scene objects this blip should represet then you can set the pool size manually");
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Set Pool Size");
                                            Current_Selected3D_Radar.Blips[_Blip.Index].optimization.SetPoolSizeManually = EditorGUILayout.Toggle(Current_Selected3D_Radar.Blips[_Blip.Index].optimization.SetPoolSizeManually, GUILayout.Width(220));
                                            GUILayout.EndHorizontal();
                                            if (Current_Selected3D_Radar.Blips[_Blip.Index].optimization.SetPoolSizeManually)
                                            {
                                                HelpMessage("The mumber of scene objects that this blip will represent");
                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Pool Size");
                                                Current_Selected3D_Radar.Blips[_Blip.Index].optimization.poolSize = EditorGUILayout.IntField(Current_Selected3D_Radar.Blips[_Blip.Index].optimization.poolSize, GUILayout.Width(220));
                                                GUILayout.EndHorizontal();

                                                HelpMessage("If your pool size is too large then the count will be calculated DOWN");
                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Recalculate Pool Size On Start");
                                                Current_Selected3D_Radar.Blips[_Blip.Index].optimization.RecalculatePoolSizeBasedOnFirstFoundObjects = EditorGUILayout.Toggle(Current_Selected3D_Radar.Blips[_Blip.Index].optimization.RecalculatePoolSizeBasedOnFirstFoundObjects, GUILayout.Width(220));
                                                GUILayout.EndHorizontal();
                                            }

                                        }




                                        HelpMessage("This method allows you to use object pooling to store your blips");
                                        GUILayout.BeginHorizontal();
                                        var info = Current_Selected3D_Radar.Blips[_Blip.Index].optimization.objectFindingMethod == ObjectFindingMethod.Pooling ? " (Fast)" : (Current_Selected3D_Radar.Blips[_Blip.Index].optimization.RequireInstanceObjectCheck) ? " (Fast)" : " (Slower)";
                                        GUILayout.Label("Use Recursive Object Finding" + info);
                                        Current_Selected3D_Radar.Blips[_Blip.Index].optimization.objectFindingMethod = (ObjectFindingMethod)EditorGUILayout.EnumPopup(Current_Selected3D_Radar.Blips[_Blip.Index].optimization.objectFindingMethod, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        if (Current_Selected3D_Radar.Blips[_Blip.Index].optimization.objectFindingMethod == ObjectFindingMethod.Recursive)
                                        {
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Require Instance Object Check");
                                            if (Current_Selected3D_Radar.Blips[_Blip.Index].optimization.RequireInstanceObjectCheck)
                                                HelpMessage("This requires that you call ' _3DRadar.doInstanceObjectCheck() whenever you want to make the radar search for objects to create blips from. This can also be used to icrease your internal pool size if you need to track more objects'");
                                            Current_Selected3D_Radar.Blips[_Blip.Index].optimization.RequireInstanceObjectCheck = EditorGUILayout.Toggle(Current_Selected3D_Radar.Blips[_Blip.Index].optimization.RequireInstanceObjectCheck, GUILayout.Width(220));
                                            GUILayout.EndHorizontal();
                                        }

                                        HelpMessage("Allows blips to be removed whenever the object it represent  , changes its tag");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Remove Blip On Tag Change");
                                        Current_Selected3D_Radar.Blips[_Blip.Index].optimization.RemoveBlipsOnTagChange = EditorGUILayout.Toggle(Current_Selected3D_Radar.Blips[_Blip.Index].optimization.RemoveBlipsOnTagChange, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        HelpMessage("Allows for the blip to be turned off when the object its represents is disabled");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Remove Blip On Disable");
                                        Current_Selected3D_Radar.Blips[_Blip.Index].optimization.RemoveBlipsOnDisable = EditorGUILayout.Toggle(Current_Selected3D_Radar.Blips[_Blip.Index].optimization.RemoveBlipsOnDisable, GUILayout.Width(220));
                                        GUILayout.EndHorizontal();


                                    }

                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();

                                    Separator();

                                    EditorGUILayout.Space();

                                    #endregion

                                    #region Scale and Rotation Settings

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(20);
                                    GUILayout.BeginVertical();

                                    _Blip.Value.ShowGeneralSettings = EditorGUILayout.Foldout(_Blip.Value.ShowGeneralSettings, "Rotation and Scale");
                                    if (_Blip.Value.ShowGeneralSettings)
                                    {
                                        HelpMessage("Set the scale of the blip. If 'Scale by distance' is being used then the blps will scale in the radar based on their distance from the center object . the visibility of the scaling varies based on the size of the radar ");

                                        #region Blip Scale Settings
                                        if (!Current_Selected3D_Radar.Blips[_Blip.Index].BlipCanScleBasedOnDistance)
                                        {
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Scale");
                                            GUILayout.Space(153);
                                            Current_Selected3D_Radar.Blips[_Blip.Index].BlipSize = EditorGUILayout.FloatField(" ", Current_Selected3D_Radar.Blips[_Blip.Index].BlipSize, GUILayout.MaxWidth(Screen.width));
                                            GUILayout.EndHorizontal();
                                        }
                                        else
                                        {
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Min Scale");
                                            Current_Selected3D_Radar.Blips[_Blip.Index].BlipMinSize = EditorGUILayout.FloatField("", Current_Selected3D_Radar.Blips[_Blip.Index].BlipMinSize, GUILayout.Width(220));
                                            GUILayout.EndHorizontal();

                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Max Scale");
                                            Current_Selected3D_Radar.Blips[_Blip.Index].BlipMaxSize = EditorGUILayout.FloatField("", Current_Selected3D_Radar.Blips[_Blip.Index].BlipMaxSize, GUILayout.Width(220));
                                            GUILayout.EndHorizontal();
                                        }
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Scale by distance");
                                        Current_Selected3D_Radar.Blips[_Blip.Index].BlipCanScleBasedOnDistance = GUILayout.Toggle(Current_Selected3D_Radar.Blips[_Blip.Index].BlipCanScleBasedOnDistance, "", GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        #endregion

                                        #region Rotation Settings

                                        HelpMessage("Use custom rotation allows yo uto set a static rotation for all of these blip types");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Use Custom Rotation");
                                        Current_Selected3D_Radar.Blips[_Blip.Index].UseCustomRotation = GUILayout.Toggle(Current_Selected3D_Radar.Blips[_Blip.Index].UseCustomRotation, "", GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        if (Current_Selected3D_Radar.Blips[_Blip.Index].UseCustomRotation && !Current_Selected3D_Radar.Blips[_Blip.Index].IsTrackRotation)
                                        {
                                            HelpMessage("Set a X, Y and Z rotation for this blip type");

                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Rotation");
                                            GUILayout.Label("X");
                                            Current_Selected3D_Radar.Blips[_Blip.Index].CustomXRotation = EditorGUILayout.FloatField(Current_Selected3D_Radar.Blips[_Blip.Index].CustomXRotation);
                                            GUILayout.Label("Y");
                                            Current_Selected3D_Radar.Blips[_Blip.Index].CustomYRotation = EditorGUILayout.FloatField(Current_Selected3D_Radar.Blips[_Blip.Index].CustomYRotation);
                                            GUILayout.Label("Z");
                                            Current_Selected3D_Radar.Blips[_Blip.Index].CustomZRotation = EditorGUILayout.FloatField(Current_Selected3D_Radar.Blips[_Blip.Index].CustomZRotation);
                                            GUILayout.EndHorizontal();
                                        }

                                        HelpMessage("Track Rotation allows for the blip to rotate and match the rotation of the tracked object");

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Track Rotation");
                                        Current_Selected3D_Radar.Blips[_Blip.Index].IsTrackRotation = GUILayout.Toggle(Current_Selected3D_Radar.Blips[_Blip.Index].IsTrackRotation, "", GUILayout.Width(220));
                                        GUILayout.EndHorizontal();

                                        if (Current_Selected3D_Radar.Blips[_Blip.Index].IsTrackRotation && !Current_Selected3D_Radar.Blips[_Blip.Index].UseCustomRotation)
                                        {
                                            HelpMessage("Freeze rotation through the x,y, or z axis");

                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Freeze");
                                            Current_Selected3D_Radar.Blips[_Blip.Index].lockX = GUILayout.Toggle(Current_Selected3D_Radar.Blips[_Blip.Index].lockX, "X");
                                            Current_Selected3D_Radar.Blips[_Blip.Index].lockY = GUILayout.Toggle(Current_Selected3D_Radar.Blips[_Blip.Index].lockY, "Y");
                                            Current_Selected3D_Radar.Blips[_Blip.Index].lockZ = GUILayout.Toggle(Current_Selected3D_Radar.Blips[_Blip.Index].lockZ, "Z");
                                            GUILayout.EndHorizontal();
                                        }
                                        #endregion

                                        if (Current_Selected3D_Radar.Blips[_Blip.Index].UseCustomRotation && Current_Selected3D_Radar.Blips[_Blip.Index].IsTrackRotation)
                                        {
                                            EditorGUILayout.HelpBox("Do not use 'Track Rotation' and 'Custom Rotation at the same time'", MessageType.Warning);
                                        }
                                    }
                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();

                                    Separator();

                                    EditorGUILayout.Space();
                                    #endregion


                                    #region Universal Settings
                                    HelpMessage("With the current settings,If this blip is active, it will represent your gameobject with the tag " + _Blip.Value.Tag + " on the layer " + _Blip.Value.Layer + " and will be at scale " + Current_Selected3D_Radar.Blips[_Blip.Index].BlipSize);

                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Create Blip");

                                    _Blip.Value._CreateBlipAs = (CreateBlipAs)EditorGUILayout.EnumPopup(_Blip.Value._CreateBlipAs, GUILayout.Width(100));

                                    GUILayout.BeginHorizontal(); GUILayout.Label("From Object Tagged"); _Blip.Value.Tag = EditorGUILayout.TagField(_Blip.Value.Tag); GUILayout.EndHorizontal();

                                    GUILayout.BeginHorizontal(); GUILayout.Label("On Layer"); _Blip.Value.Layer = EditorGUILayout.LayerField(_Blip.Value.Layer); GUILayout.EndHorizontal();

                                    GUILayout.EndHorizontal();


                                    #endregion
                                }

                                Separator();
                                EditorGUILayout.Separator();
                            }
                            catch { }
                        }
                        #endregion

                        EditorGUILayout.EndScrollView();


                        break;
                    #endregion

                    #region Make New Radar
                    case 2:
                        CreationArea();

                        break;
                        #endregion
                }
            }

            #endregion

            #endregion

        }
        #endregion

        #region Separator
        void Separator()
        {
            // var tex = Application.HasProLicense() ? Textures.lightGray : Textures.gray;
            // GUI.DrawTexture(GUILayoutUtility.GetLastRect().ToLowerLeft(Screen.width - 5, 1, -5, 5), tex);

            var col = Application.HasProLicense() ? Color.gray : Colour.colour(10, 10, 10);

            Handles.BeginGUI();
            Handles.DrawBezier(GUILayoutUtility.GetLastRect().ToLowerLeft(0, 1, -5, 5).position, GUILayoutUtility.GetLastRect().ToLowerLeft(0, 1, Screen.width, 5).position,
                GUILayoutUtility.GetLastRect().ToLowerLeft(0, 1, -5, 5).position, GUILayoutUtility.GetLastRect().ToLowerLeft(0, 1, Screen.width, 5).position,
                col, null, 1);
            Handles.EndGUI();


        }
        #endregion

        #region Help Messages
        void HelpMessage(string message)
        {
            if (ShowHelpMessages) { EditorGUILayout.HelpBox(message, MessageType.Info); }
        }
        #endregion

        #region OnInspector Update
        public void OnInspectorUpdate()
        {
            Repaint();
            if (_Selection())
                if (_Selection().GetComponent<_2DRadar>() || _Selection().GetComponent<_3DRadar>())
                {
                    Repaint();
                    RadarObject = _Selection();
                    Repaint();
                }

        }
        #endregion

        void cursorchange()
        {
            if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
            }
        }

        #region Create 2D Radar
        void Create2DRadar()
        {

            GameObject RadarInstance = new GameObject("2D Radar");
            RadarInstance.AddComponent<_2DRadar>();
            RadarInstance.AddComponent<Visualization2D>();
            Selection.activeGameObject = RadarInstance;
            GameObject DesignsParent = new GameObject("Designs");
            DesignsParent.transform.parent = RadarInstance.transform;
            GameObject BlipsContainer = new GameObject("Blips");
            BlipsContainer.transform.parent = RadarInstance.transform;
            GameObject DefaultDesign = new GameObject("DefaultRadarSprite");
            DefaultDesign.transform.parent = DesignsParent.transform;
            DefaultDesign.AddComponent<SpriteRenderer>();
            DefaultDesign.GetComponent<SpriteRenderer>().sprite = Sprite.Create(ResourceManager.GetFromRoot("Default2DRadarSprite"), new Rect(0, 0, 200, 200), new Vector2(0.5f, 0.5f));
            CreateRenderCamera();


            Tab_Selection = 0;

        }
        #endregion

        #region Create 3D Radar
        void Create3DRadar()
        {

            GameObject RadarInstance = new GameObject("3D Radar");
            RadarInstance.AddComponent<_3DRadar>();
            RadarInstance.AddComponent<Visualization3D>();
            Selection.activeGameObject = RadarInstance;
            GameObject DesignsParent = new GameObject("Designs");
            DesignsParent.transform.parent = RadarInstance.transform;
            GameObject BlipsContainer = new GameObject("Blips");
            BlipsContainer.transform.parent = RadarInstance.transform;
            GameObject DefaultDesign = new GameObject("Default3DRadarSprite");
            DefaultDesign.transform.parent = DesignsParent.transform;
            DefaultDesign.transform.Rotate(new Vector2(90, 0), Space.World);
            DefaultDesign.AddComponent<SpriteRenderer>();
            DefaultDesign.GetComponent<SpriteRenderer>().sprite = Sprite.Create(ResourceManager.GetFromRoot("Default3DRadarSprite"), new Rect(0, 0, 200, 200), new Vector2(0.5f, 0.5f));
            CreateRenderCamera();




            Tab_Selection = 0;

        }
        #endregion

        #region CreateRender Camera
        void CreateRenderCamera()
        {
            GameObject RenderCamera = new GameObject("Render Camera");
            RenderCamera.AddComponent<Camera>();
            RenderCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
            RenderCamera.GetComponent<Camera>().farClipPlane = 265;
            RenderCamera.GetComponent<Camera>().nearClipPlane = 0.01f;
            RenderCamera.GetComponent<Camera>().depth = 100;
            RenderCamera.GetComponent<Camera>().cullingMask = 1;
            RenderCamera.GetComponent<Camera>().orthographicSize = 2.3f;



        }
        #endregion


        #region Create Realitime Minimap Camera

        Camera CreateRealtimeMinimapCamera(Type radarType)
        {
            GameObject RenderCamera = new GameObject("Realtime Minimap Camera");
            RenderCamera.transform.Rotate(new Vector2(90, 0), Space.World);
            RenderCamera.AddComponent<Camera>();
            RenderCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            RenderCamera.GetComponent<Camera>().farClipPlane = 50;
            RenderCamera.GetComponent<Camera>().nearClipPlane = 0.01f;
            RenderCamera.GetComponent<Camera>().depth = 0;
            RenderCamera.GetComponent<Camera>().orthographic = true;
            try
            {

                if (radarType == typeof(_2DRadar))
                    RenderCamera.GetComponent<Camera>().targetTexture = Current_Selected2D_Radar.minimapModule.renderTexture;
                else
                    RenderCamera.GetComponent<Camera>().targetTexture = Current_Selected3D_Radar.minimapModule.renderTexture;
            }
            catch
            {
                Debug.LogWarning("You have no set up a render texture your Realtime Minimap Camera yet");
            }
            return RenderCamera.GetComponent<Camera>();
        }
        #endregion




    }

}
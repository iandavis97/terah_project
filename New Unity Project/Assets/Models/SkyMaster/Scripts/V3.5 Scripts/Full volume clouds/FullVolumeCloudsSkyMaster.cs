using System;
using UnityEngine;
using System.Collections;

namespace Artngame.SKYMASTER
{
	[ExecuteInEditMode]
	[RequireComponent (typeof(Camera))]
	[AddComponentMenu ("Image Effects/Rendering/Global Fog Sky Master")]
	public class FullVolumeCloudsSkyMaster : PostEffectsBaseSkyMaster
	{

        public bool unity2018 = false; //v4.1

        [Tooltip("Apply distance-based fog?")]
		public bool  distanceFog = true;
		[Tooltip("Distance fog is based on radial distance from camera when checked")]
		public bool  useRadialDistance = false;
		[Tooltip("Apply height-based fog?")]
		public bool  heightFog = true;
		[Tooltip("Fog top Y coordinate")]
		public float height = 1.0f;
		[Range(0.00001f,10.0f)]
		public float heightDensity = 2.0f;
		[Tooltip("Push fog away from the camera by this amount")]
		public float startDistance = 0.0f;

        //v4.1f
        public float _mobileFactor = 1;
        public float _alphaFactor = 0.96f;

        //v2.1.24
        public float _HorizonZAdjust = 1;

		//v2.1.24
		public bool updateReflectionCamera = false; //add reflection of clouds to reflect camera of water module or update it if already exists
		public bool updateReflectionCameraLocalLights = false;
		public bool updateShadows = false;//update shadows material parameters based on cloud params
		public Material cloudsShadowMat;//material for cloud shadows
		public bool adaptLocalToLightning = false;//adapt local light from lightning module
		//public 

		//v2.1.21
		public bool useFluidTexture = false;
		public Material fluidFlow;

		//v2.1.20
		public bool WebGL = false;
		public bool blendBackground = false;
		public Camera backgroundCam;
		public Material backgroundMat; //blend layer between sky and clouds - https://answers.unity.com/questions/330892/why-is-the-second-camera-interfering-with-image-ef.html

        //v2.1.19
        public bool fastest = false;
        public Light localLight;
        public float localLightFalloff = 2;
        public float localLightIntensity = 1;
		public float currentLocalLightIntensity = 0; //public only for debug purposes

        //v3.5.3
        public Texture2D _InteractTexture;
		public Vector4 _InteractTexturePos = new Vector4(1,1,0,0);
		public Vector4 _InteractTextureAtr = new Vector4(1,1,0,0);//multipliers and offsets
		public Vector4 _InteractTextureOffset = new Vector4(0,0,0,0); //v4.0

		//v3.5.1
		public float _NearZCutoff = -2;
		public float _HorizonYAdjust = 0;
		public float _FadeThreshold = 0;

		//v3.5
		public Texture3D texture3Dnoise1;
		public Texture3D texture3Dnoise2;

		public float _SampleCount0 = 42;

		public float _SampleCount1=3;
		public int _SampleCountL=4;

		public float _NoiseFreq1=3.1f;
		public float _NoiseFreq2=35.1f;
		public float _NoiseAmp1=5;
		public float _NoiseAmp2=1;
		public float _NoiseBias=-0.2f;

		public Vector3 _Scroll1 = new Vector3(0.01f, 0.08f, 0.06f);
		public Vector3 _Scroll2 = new Vector3(0.01f, 0.05f, 0.03f);

		public float _Altitude0 = 1500;
		public float _Altitude1 = 3500;
		public float _FarDist = 30000;
        public float _FarDistNight = 300000000;
        public float _FarDistDay = 400000;//save day distance here to restore it

        public float _Scatter = 0.008f;
		public float _HGCoeff = 0.5f;
		public float _Extinct = 0.01f;

		public float _Exposure = 0.00001f;
		public float _ExposureUnder = 3f; //v4.0
		public Vector3 _GroundColor = new Vector3(0.369f, 0.349f, 0.341f);//
		public float _SunSize = 0.04f;
		public Vector3 _SkyTint = new Vector3(0.5f, 0.5f, 0.5f);
		public float _AtmosphereThickness = 1.0f;

		public float _BackShade = 1.0f;
		public float _UndersideCurveFactor = 0.0f;




		//SM v1.7
		public Gradient DistGradient = new Gradient();
		public Vector2 GradientBounds = Vector2.zero;
		public float luminance = 0.8f;
		public float lumFac =0.9f; 
		public float ScatterFac = 24.4f;//scatter factor
		public float TurbFac= 324.74f;//turbidity scale
		public float HorizFac = 1;//sun horizon multiplier
		public float turbidity = 10f;
		public float reileigh = 0.8f;
		public float mieCoefficient = 0.1f;
		public float mieDirectionalG = 0.7f; 		
		public float bias = 0.6f;
		public float contrast = 4.12f;
		public Transform Sun;
		public bool FogSky = false;
		public Vector3 TintColor = new Vector3(68,155,345); //68, 155, 345
		public float ClearSkyFac = 1;
		
		public Shader fogShader = null;
		private Material fogMaterial = null;
		public SkyMasterManager SkyManager;
		Texture2D colourPalette;
		bool Made_texture = false;

		//v2.1.24 - setup shadows-reflections-depth camera
		//BACK LAYER SETUP
		public bool useTOD = false;
		public bool tintUnder = false;
		public Vector3 colorMultiplier = Vector3.one;
		public Vector3 UnderColorMultiplier = Vector3.one;
        public bool adjustNightTimeDensity = false;//v4.2
        public bool followPlayer = false;

		public Transform Galaxy;
		public Transform ShadowLayer;
		public GameObject shadowDome;

		public GameObject backCamPrefab;
		public GameObject shadowDomePrefab;
		public GameObject LightningBoxPrefab;

		public bool setupDepth = false;
		public bool setupShadows = false;
		public bool setupLightning = false;

		public void createDepthSetup(){

			if(setupDepth){

				//check layer exists and if not return and message to add
				var layerToCheck = LayerMask.NameToLayer("Background");
				if (layerToCheck > -1){

					//change layer for new background camera
					SkyManager.SunObj.layer = layerToCheck;
					SkyManager.MoonObj.layer = layerToCheck;
					SkyManager.CloudDomeL1.layer = layerToCheck;
					SkyManager.CloudDomeL2.layer = layerToCheck;
					SkyManager.StarDome.layer = layerToCheck;
					SkyManager.Star_particles_OBJ.layer = layerToCheck;	

					Transform[] transforms = transform.GetComponentsInChildren<Transform>(true);
					for(int i =0;i<transforms.Length;i++){
						if (transforms[i].name.Contains("STAR_GALAXY_DOME")){
							transforms[i].gameObject.layer = layerToCheck;		
							Galaxy = transforms[i];
							break;
						}
					}

					//if not find in camera, search otherwise
					if(Galaxy == null){
						transforms = SkyManager.transform.GetComponentsInChildren<Transform>(true);
						for(int i =0;i<transforms.Length;i++){
							if (transforms[i].name.Contains("STAR_GALAXY_DOME")){
								transforms[i].gameObject.layer = layerToCheck;		
								Galaxy = transforms[i];
								break;
							}
						}
					}

					//do stars
					ParticleSystem[] transformsStars = SkyManager.Star_particles_OBJ.transform.GetComponentsInChildren<ParticleSystem>(true);
					for(int i =0;i<transformsStars.Length;i++){
						//if (transforms[i].name.Contains("STAR_GALAXY_DOME")){
							transformsStars[i].gameObject.layer = layerToCheck;
						//}
					}

					//Back Camera
					if(backgroundCam == null){
						GameObject backCam = Instantiate(backCamPrefab);
						backgroundCam = backCam.GetComponent<Camera>();	
					}		

					//Material

					//Change cameras layers
					//make 9 visible  - var newMask = oldMask | (1 << 9);
					//hide 9 - var newMask = oldMask & ~(1 << 9);

					// Turn on the bit using an OR operation:
					//private void Show() {
					// camera.cullingMask |= 1 << LayerMask.NameToLayer("SomeLayer");
					//}

					// Turn off the bit using an AND operation with the complement of the shifted int:
					//private void Hide() {
					// camera.cullingMask &=  ~(1 << LayerMask.NameToLayer("SomeLayer"));
					//}

					// Toggle the bit using a XOR operation:
					//private void Toggle() {
					// camera.cullingMask ^= 1 << LayerMask.NameToLayer("SomeLayer");
					//}

					backgroundCam.cullingMask = 1 << layerToCheck;
					Camera.main.cullingMask &=  ~(1 << layerToCheck);

					//Change view distance to match main camera
					backgroundCam.farClipPlane = Camera.main.farClipPlane;

                    //Add same to reflection script

                    //v4.1 - untag camera from MainCamera
                    backgroundCam.tag = "Untagged";

                    //Follow player option
                    if (followPlayer){
						Vector3 CamPos = Camera.main.transform.position;
						SkyManager.SunObj.layer = layerToCheck;
						SkyManager.MoonObj.layer = layerToCheck;
						SkyManager.CloudDomeL1.layer = layerToCheck;
						SkyManager.CloudDomeL2.layer = layerToCheck;
						SkyManager.StarDome.layer = layerToCheck;
						SkyManager.Star_particles_OBJ.layer = layerToCheck;
						Galaxy.position = new Vector3(Galaxy.position.x,Galaxy.position.z);
						shadowDome.transform.position = new Vector3(CamPos.x,shadowDome.transform.position.y,CamPos.z);	
					}
				}else{
					Debug.Log("Please add the Background layer to proceed with the setup");
				}
				setupDepth = false;
			}
		}

		public void createShadowDome(){
			if(setupShadows){
				if(shadowDome == null){
					GameObject shadowDome1 = Instantiate(shadowDomePrefab);
					shadowDome = shadowDome1;	//v4.1e
				}
				setupShadows = false;
			}
		}

		public void createLightningBox(){
			if(setupLightning){
				if(LightningBox == null){
					GameObject lightningDome = Instantiate(LightningBoxPrefab);
					LightningBox = lightningDome.transform;			
				}
				setupLightning  = false;
			}
		}


		//v2.1.24
		public void shadowsUpdate(){
			if (updateShadows) {

				if (cloudsShadowMat) {
					cloudsShadowMat.SetVector("_InteractTextureAtr",_InteractTextureAtr);
					cloudsShadowMat.SetVector("_InteractTextureOffset",_InteractTextureOffset);
					cloudsShadowMat.SetVector("_InteractTexturePos",_InteractTexturePos);
					cloudsShadowMat.SetTexture("_InteractTexture",_InteractTexture);
					cloudsShadowMat.SetFloat("_Altitude0",_Altitude0);
					cloudsShadowMat.SetFloat("_Altitude1",_Altitude1);

					cloudsShadowMat.SetFloat("_NoiseBias",_NoiseBias);
					cloudsShadowMat.SetFloat("_UndersideCurveFactor",_UndersideCurveFactor);
					cloudsShadowMat.SetFloat("_BackShade",_BackShade);

					cloudsShadowMat.SetVector("_Scroll1",_Scroll1);
					cloudsShadowMat.SetVector("_Scroll2",_Scroll2);

					cloudsShadowMat.SetFloat("_NoiseAmp1",_NoiseAmp1);
					cloudsShadowMat.SetFloat("_NoiseAmp2",_NoiseAmp2);
					cloudsShadowMat.SetFloat("_NoiseFreq1",_NoiseFreq1);
					cloudsShadowMat.SetFloat("_NoiseFreq2",_NoiseFreq2);
				}

				updateShadows = false;
			}
		}

		//v2.1.24 - reflections
		public FullVolumeCloudsSkyMaster reflectClouds;
		public void updateReflections(){

			if (Application.isPlaying) {
				//update local lights
				if (reflectClouds != null && updateReflectionCameraLocalLights) {
					reflectClouds.localLight = localLight;
				}
			}

			if (updateReflectionCamera) {

				if (SkyManager) {
					if (SkyManager.water) {
						PlanarReflectionSM waterScript = SkyManager.water.GetComponentInChildren<PlanarReflectionSM> ();
						if (waterScript.m_ReflectionCameraOut != null) {
							reflectClouds = waterScript.m_ReflectionCameraOut.GetComponent<FullVolumeCloudsSkyMaster> ();
							if (reflectClouds == null) {
								reflectClouds = waterScript.m_ReflectionCameraOut.gameObject.AddComponent<FullVolumeCloudsSkyMaster> ();
							} 
							//update params

							reflectClouds.startDistance = 10000000000;

							reflectClouds.downScaleFactor = 6; 
							reflectClouds.downScale = true;
							reflectClouds.splitPerFrames = 0;
							reflectClouds._needsReset = true;
							reflectClouds._SampleCount0 = _SampleCount0;
							reflectClouds._SampleCount1 = _SampleCount1;
							reflectClouds._SampleCountL = _SampleCountL;

							reflectClouds.localLightFalloff = localLightFalloff;
							reflectClouds.localLightIntensity = localLightIntensity;
							reflectClouds.updateReflectionCameraLocalLights = updateReflectionCameraLocalLights;

							//materials
							reflectClouds.cloudsShadowMat = cloudsShadowMat;
							reflectClouds.backgroundCam = backgroundCam;
							reflectClouds.backgroundMat = backgroundMat;
							reflectClouds.blendBackground = blendBackground;
							reflectClouds.WebGL = WebGL;
							//reflectClouds._InteractTexture = _InteractTexture;
							reflectClouds.texture3Dnoise1 = texture3Dnoise1;
							reflectClouds.texture3Dnoise2 = texture3Dnoise2;
							reflectClouds.Sun = Sun;
							reflectClouds.fogShader = fogShader;
							reflectClouds.SkyManager = SkyManager;
							reflectClouds.LightningPrefab = LightningPrefab;
							reflectClouds.LightningBox = LightningBox;
							reflectClouds.cameraMotionCompensate = cameraMotionCompensate;
							reflectClouds.lightning_every = lightning_every;
							reflectClouds.max_lightning_time = max_lightning_time;
							reflectClouds.lightning_rate_offset = lightning_rate_offset;

							reflectClouds._FarDist = _FarDist;
							//reflectClouds.ScatterFac = ScatterFac;
							reflectClouds._Scatter = _Scatter;
							reflectClouds._HGCoeff = _HGCoeff;
							reflectClouds._Extinct = _Extinct;
							reflectClouds._SunSize = _SunSize;
							reflectClouds._SkyTint = _SkyTint;
							reflectClouds._ExposureUnder = _ExposureUnder;
							reflectClouds._HorizonYAdjust = _HorizonYAdjust;
							reflectClouds._HorizonZAdjust = _HorizonZAdjust; //v2.1.24
							reflectClouds._GroundColor = _GroundColor;

                            //v4.1f                            
                            reflectClouds._mobileFactor = _mobileFactor;
                            reflectClouds._alphaFactor = _alphaFactor;

                            reflectClouds.luminance = luminance;
							reflectClouds.lumFac = lumFac;
							reflectClouds.ScatterFac = ScatterFac;
							reflectClouds.TurbFac = TurbFac;
							reflectClouds.turbidity = turbidity;
							reflectClouds.HorizFac = HorizFac;
							reflectClouds.reileigh = reileigh;
							reflectClouds.mieCoefficient = mieCoefficient;
							reflectClouds.mieDirectionalG = mieDirectionalG;
							reflectClouds.bias = bias;
							reflectClouds.contrast = contrast;
							reflectClouds.TintColor = TintColor;

							//same as shadows
							reflectClouds._InteractTextureAtr = _InteractTextureAtr;
							reflectClouds._InteractTextureOffset = _InteractTextureOffset;
							reflectClouds._InteractTexturePos=_InteractTexturePos;
							reflectClouds._InteractTexture=_InteractTexture;
							reflectClouds._Altitude0=_Altitude0;
							reflectClouds._Altitude1=_Altitude1;

							reflectClouds._NoiseBias=_NoiseBias;
							reflectClouds._UndersideCurveFactor=_UndersideCurveFactor;
							reflectClouds._BackShade=_BackShade;

							reflectClouds._Scroll1=_Scroll1;
							reflectClouds._Scroll2=_Scroll2;

							reflectClouds._NoiseAmp1=_NoiseAmp1;
							reflectClouds._NoiseAmp2=_NoiseAmp2;
							reflectClouds._NoiseFreq1=_NoiseFreq1;
							reflectClouds._NoiseFreq2=_NoiseFreq2;
						}
					}
				}

				updateReflectionCamera = false;
			}
		}

		//v2.1.24 - lightning
		public GameObject LightningPrefab; //Prefab to instantiate for lighting, use only 1-2 prefabs and move them around
		public bool EnableLightning = false;
		public bool useLocalLightLightn = false;
		float last_lightning_time = 0;
		public float lightning_every = 15;
		public float max_lightning_time = 2;
		public float lightning_rate_offset = 5;
		Transform LightningOne;
		Transform LightningTwo;
		public Transform LightningBox; 
		Light LightA;
		Light LightB; //keep lights here and update them in the script local light as needed
		public void LightningUpdate(){

			if (Application.isPlaying) {
				if (EnableLightning) {
					if (LightningOne == null) {
						LightningOne = Instantiate (LightningPrefab).transform;
						LightA = LightningOne.GetComponentInChildren<ChainLightning_SKYMASTER> ().startLight;
						LightningOne.gameObject.SetActive (false);
					}
					if (LightningTwo == null) {
						LightningTwo = Instantiate (LightningPrefab).transform;
						LightB = LightningTwo.GetComponentInChildren<ChainLightning_SKYMASTER> ().startLight;//v4.0 - 2.1.24 IG
						LightningTwo.gameObject.SetActive (false);
					}

					//move around
					if (LightningBox != null) {
						if (Time.fixedTime - last_lightning_time > lightning_every - UnityEngine.Random.Range (-lightning_rate_offset, lightning_rate_offset)) {

							Vector2 MinMaXLRangeX = LightningBox.position.x * Vector2.one + (LightningBox.localScale.x / 2) * new Vector2 (-1, 1);
							Vector2 MinMaXLRangeY = LightningBox.position.y * Vector2.one + (LightningBox.localScale.y / 2) * new Vector2 (-1, 1);
							Vector2 MinMaXLRangeZ = LightningBox.position.z * Vector2.one + (LightningBox.localScale.z / 2) * new Vector2 (-1, 1);

							LightningOne.position = new Vector3 (UnityEngine.Random.Range (MinMaXLRangeX.x, MinMaXLRangeX.y), 
								UnityEngine.Random.Range (MinMaXLRangeY.x, MinMaXLRangeY.y), UnityEngine.Random.Range (MinMaXLRangeZ.x, MinMaXLRangeZ.y));
							if (UnityEngine.Random.Range (0, SkyManager.WeatherSeverity + 1) == 1) { 
								//do nothing
							} else {
								LightningOne.gameObject.SetActive (true);
								localLight = LightA;
							}

							LightningTwo.position = new Vector3 (UnityEngine.Random.Range (MinMaXLRangeX.x, MinMaXLRangeX.y), 
								UnityEngine.Random.Range (MinMaXLRangeY.x, MinMaXLRangeY.y), UnityEngine.Random.Range (MinMaXLRangeZ.x, MinMaXLRangeZ.y));
							if (UnityEngine.Random.Range (0, SkyManager.WeatherSeverity + 1) == 1) { 
								//do nothing
							} else {
								LightningTwo.gameObject.SetActive (true);
								localLight = LightB;
							}

							last_lightning_time = Time.fixedTime;
						} else {
							if (Time.fixedTime - last_lightning_time > max_lightning_time) {
								if (LightningOne != null) {	
									if (LightningOne.gameObject.activeInHierarchy) {
										LightningOne.gameObject.SetActive (false); localLight = null; //v4.0
									}
								}
								if (LightningTwo != null) {	
									if (LightningTwo.gameObject.activeInHierarchy) {
										LightningTwo.gameObject.SetActive (false); localLight = null; //v4.0
									}
								}
							}
						}
					}
				} else {
					if (LightningOne != null) {	
						if (LightningOne.gameObject.activeInHierarchy) {
							LightningOne.gameObject.SetActive (false); localLight = null; //v4.0
						}
					}
					if (LightningTwo != null) {	
						if (LightningTwo.gameObject.activeInHierarchy) {
							LightningTwo.gameObject.SetActive (false); localLight = null; //v4.0
						}
					}
				}
			}

		}


		void Update(){

			//v2.1.24 - setup shadows-backlayer camera
			createDepthSetup();
			createShadowDome();
			createLightningBox();

			//v2.1.24
			shadowsUpdate();
			updateReflections ();
			if (Application.isPlaying) {
				LightningUpdate ();
			}


			if(SkyManager!=null){
//				if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <20){
//					height = 166;
//				}else{
//					height = 100;
//				}
				if(useTOD){
					if (SkyManager.UseGradients) {
						_SkyTint = new Vector3 (SkyManager.gradSkyColor.r * colorMultiplier.x, SkyManager.gradSkyColor.g * colorMultiplier.y, SkyManager.gradSkyColor.b * colorMultiplier.z);
						if (tintUnder) {
							_GroundColor = new Vector3 (SkyManager.gradSkyColor.r * UnderColorMultiplier.x, SkyManager.gradSkyColor.g * UnderColorMultiplier.y, SkyManager.gradSkyColor.b * UnderColorMultiplier.z);
						}
					}
				}

                //v4.2
                if (adjustNightTimeDensity)
                {
                    if ((!SkyManager.AutoSunPosition && (SkyManager.Current_Time >= (9 + SkyManager.Shift_dawn) & SkyManager.Current_Time <= (SkyManager.NightTimeMax + SkyManager.Shift_dawn)))
                    ||
                    (SkyManager.AutoSunPosition && SkyManager.Rot_Sun_X > 0))
                    {
                        _FarDist = _FarDistDay;
                    }
                    else
                    {
                        _FarDist = _FarDistNight;
                    }
                }//
			}
			if(colourArray == null){
				colourArray = new Color[gradientResolution];
			}
            //if (prevGradient != DistGradient) {
                //if (1 == 0) //v4.1e
                //{
                //    for (int i = 0; i < colourArray.Length; i++)
                //    {
                //        colourArray[i] = DistGradient.Evaluate((float)i / (float)colourArray.Length);
                //    }
                //    if (!Made_texture)
                //    {
                //        colourPalette = new Texture2D(gradientResolution, 10, TextureFormat.ARGB32, false);
                //        colourPalette.filterMode = FilterMode.Point;
                //        colourPalette.wrapMode = TextureWrapMode.Clamp;
                //        Made_texture = true;
                //    }
                //    for (int x = 0; x < gradientResolution; x++)
                //    {
                //        for (int y = 0; y < 10; y++)
                //        {
                //            colourPalette.SetPixel(x, y, colourArray[x]);
                //        }
                //    }
                //    colourPalette.Apply();
                //}
				//prevGradient = DistGradient;
			//}
			
		}

		//v3.4.3
		public int gradientResolution=256;
		//Gradient prevGradient;
		Color[] colourArray;

		//void Start(){
		void Awake(){

			_needsReset = true; //v4.0

			//SM v3.4.3
			//if (colourPalette == null) 
			{
				colourArray = new Color[gradientResolution];	//Color[] colourArray = new Color[gradientResolution];			
				for(int i=0;i<colourArray.Length;i++){
					colourArray[i] = DistGradient.Evaluate((float)i/(float)colourArray.Length);
				}	

				if(!Made_texture){
					colourPalette = new Texture2D (gradientResolution, 10, TextureFormat.ARGB32, false);
					colourPalette.filterMode = FilterMode.Point;
					colourPalette.wrapMode = TextureWrapMode.Clamp;
					Made_texture=true;
				}

				for(int x = 0; x < gradientResolution; x++){
					for(int y = 0; y < 10; y++){
						colourPalette.SetPixel(x,y,colourArray[x]);
					}
				}

				colourPalette.Apply();
				//prevGradient = DistGradient;
			}

			//v3.5
			//fogMaterial.SetTexture("_NoiseTex1",texture3Dnoise1);
			//fogMaterial.SetTexture("_NoiseTex2",texture3Dnoise2);

			cam = GetComponent<Camera>(); //v2.1.15

		}
		
		public override bool CheckResources ()
		{
			CheckSupport (true);
			
			fogMaterial = CheckShaderAndCreateMaterial (fogShader, fogMaterial);
			
			if (!isSupported)
				ReportAutoDisable ();
			return isSupported;
		}


		//v2.1.20
		CameraClearFlags previousFlag;

		//v2.1.15
		Camera cam;
        void OnPreRender()
        {
            if (!fastest)
            {
                cam = GetComponent<Camera>(); //v4.2

                previousFlag = cam.clearFlags; //v2.1.20
                cam.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0);
                cam.clearFlags = CameraClearFlags.SolidColor;

//				if (blendBackground) { //v2.1.20
//					backgroundCam.transform.rotation = cam.transform.rotation;
//					backgroundCam.transform.position = cam.transform.position;
//				}
            }
        }
        void OnPostRender()
        {
            if (!fastest)
            {
                cam = GetComponent<Camera>(); //v4.2

                //cam.clearFlags = CameraClearFlags.Skybox;
                cam.clearFlags = previousFlag; //v2.1.20
            }
        }


        int toggleCounter = 0;
        public int splitPerFrames = 1; //v2.1.19
        public bool cameraMotionCompensate = true;//v2.1.19

        [ImageEffectOpaque]
		void OnRenderImage (RenderTexture source, RenderTexture destination)
		{
			if (CheckResources()==false || (!distanceFog && !heightFog))
			{
				Graphics.Blit (source, destination);
				return;
			}
			
			//Camera cam = GetComponent<Camera>(); //v2.1.15
			Transform camtr = cam.transform;
			float camNear = cam.nearClipPlane;
			float camFar = cam.farClipPlane;
			float camFov = cam.fieldOfView;
			float camAspect = cam.aspect;
			
			Matrix4x4 frustumCorners = Matrix4x4.identity;
			
			float fovWHalf = camFov * 0.5f;
			
			Vector3 toRight = camtr.right * camNear * Mathf.Tan (fovWHalf * Mathf.Deg2Rad) * camAspect;
			Vector3 toTop = camtr.up * camNear * Mathf.Tan (fovWHalf * Mathf.Deg2Rad);
			
			Vector3 topLeft = (camtr.forward * camNear - toRight + toTop);
			float camScale = topLeft.magnitude * camFar/camNear;
			
			topLeft.Normalize();
			topLeft *= camScale;
			
			Vector3 topRight = (camtr.forward * camNear + toRight + toTop);
			topRight.Normalize();
			topRight *= camScale;
			
			Vector3 bottomRight = (camtr.forward * camNear + toRight - toTop);
			bottomRight.Normalize();
			bottomRight *= camScale;
			
			Vector3 bottomLeft = (camtr.forward * camNear - toRight - toTop);
			bottomLeft.Normalize();
			bottomLeft *= camScale;
			
			frustumCorners.SetRow (0, topLeft);
			frustumCorners.SetRow (1, topRight);
			frustumCorners.SetRow (2, bottomRight);
			frustumCorners.SetRow (3, bottomLeft);
			
			var camPos= camtr.position;
			float FdotC = camPos.y-height;
			float paramK = (FdotC <= 0.0f ? 1.0f : 0.0f);
			fogMaterial.SetMatrix ("_FrustumCornersWS", frustumCorners);
			fogMaterial.SetVector ("_CameraWS", camPos);
			fogMaterial.SetVector ("_HeightParams", new Vector4 (height, FdotC, paramK, heightDensity*0.5f));
			fogMaterial.SetVector ("_DistanceParams", new Vector4 (-Mathf.Max(startDistance,0.0f), 0, 0, 0));

			//v3.5.3
			if (!useFluidTexture) {
				fogMaterial.SetTexture ("_InteractTexture", _InteractTexture);
			} else {
				fogMaterial.SetTexture ("_InteractTexture", fluidFlow.GetTexture("_MainTex")); //v4.0
			}
			fogMaterial.SetVector("_InteractTexturePos",_InteractTexturePos);
			fogMaterial.SetVector("_InteractTextureAtr",_InteractTextureAtr);
			fogMaterial.SetVector("_InteractTextureOffset",_InteractTextureOffset); //v4.0

			//v3.5.1
			fogMaterial.SetFloat("_NearZCutoff",_NearZCutoff);
			fogMaterial.SetFloat("_HorizonYAdjust",_HorizonYAdjust);
			fogMaterial.SetFloat("_HorizonZAdjust",_HorizonZAdjust);//v2.1.24
			fogMaterial.SetFloat("_FadeThreshold",_FadeThreshold);

            //v4.1f
            fogMaterial.SetFloat("_mobileFactor", _mobileFactor); //v4.1f
            fogMaterial.SetFloat("_alphaFactor", _alphaFactor);

            //v3.5
            fogMaterial.SetFloat("_SampleCount0",_SampleCount0);
			fogMaterial.SetFloat("_SampleCount1",_SampleCount1);
			fogMaterial.SetInt("_SampleCountL",_SampleCountL);

			fogMaterial.SetFloat("_NoiseFreq1",_NoiseFreq1);
			fogMaterial.SetFloat("_NoiseFreq2",_NoiseFreq2);
			fogMaterial.SetFloat("_NoiseAmp1",_NoiseAmp1);
			fogMaterial.SetFloat("_NoiseAmp2",_NoiseAmp2);
			fogMaterial.SetFloat("_NoiseBias",_NoiseBias);

			fogMaterial.SetVector ("_Scroll1",_Scroll1);
			fogMaterial.SetVector ("_Scroll2",_Scroll2);

			fogMaterial.SetFloat("_Altitude0",_Altitude0);
			fogMaterial.SetFloat("_Altitude1",_Altitude1);
			fogMaterial.SetFloat("_FarDist",_FarDist);

			fogMaterial.SetFloat("_Scatter",_Scatter);
			fogMaterial.SetFloat("_HGCoeff",_HGCoeff);
			fogMaterial.SetFloat("_Extinct",_Extinct);


			fogMaterial.SetFloat("_Exposure",_ExposureUnder); //v4.0
			fogMaterial.SetVector ("_GroundColor",_GroundColor);//
			fogMaterial.SetFloat("_SunSize",_SunSize);
			fogMaterial.SetVector ("_SkyTint",_SkyTint);
			fogMaterial.SetFloat("_AtmosphereThickness",_AtmosphereThickness);


			//v3.5
			fogMaterial.SetFloat("_BackShade",_BackShade);
			fogMaterial.SetFloat("_UndersideCurveFactor",_UndersideCurveFactor);

            //v2.1.19
			if (localLight != null) {
				Vector3 localLightPos = localLight.transform.position;
				//float intensity = Mathf.Pow(10, 3 + (localLightFalloff - 3) * 3);
				currentLocalLightIntensity = Mathf.Pow (10, 3 + (localLightFalloff - 3) * 3);
				//fogMaterial.SetVector ("_LocalLightPos", new Vector4 (localLightPos.x, localLightPos.y, localLightPos.z, localLight.intensity * localLightIntensity * intensity));
				fogMaterial.SetVector ("_LocalLightPos", new Vector4 (localLightPos.x, localLightPos.y, localLightPos.z, localLight.intensity * localLightIntensity * currentLocalLightIntensity));
				fogMaterial.SetVector ("_LocalLightColor", new Vector4 (localLight.color.r, localLight.color.g, localLight.color.b, localLightFalloff));
			} else {
				if (currentLocalLightIntensity > 0) {
					currentLocalLightIntensity = 0;
					//fogMaterial.SetVector ("_LocalLightPos", new Vector4 (localLightPos.x, localLightPos.y, localLightPos.z, localLight.intensity * localLightIntensity * intensity));
					fogMaterial.SetVector ("_LocalLightColor", Vector4.zero);
				}
			}

			//VFOG
			//fogMaterial.SetMatrix ("_WorldClip", cam.cameraToWorldMatrix * cam.projectionMatrix.inverse); 

//			fogMaterial.SetFloat("_SampleCount0",42);
//			fogMaterial.SetFloat("_SampleCount1",43);
//			fogMaterial.SetInt("_SampleCount",44);
//
//			fogMaterial.SetFloat("_NoiseFreq1",3.1f);
//			fogMaterial.SetFloat("_NoiseFreq2",35.1f);
//			fogMaterial.SetFloat("_NoiseAmp1",5f);
//			fogMaterial.SetFloat("_NoiseAmp2",1f);
//			fogMaterial.SetFloat("_NoiseBias",-0.2f);
//
//			fogMaterial.SetVector ("_Scroll1",new Vector3 (0.01f, 0.08f, 0.06f));
//			fogMaterial.SetVector ("_Scroll2", new Vector3 (0.01f, 0.05f, 0.03f));
//
//			fogMaterial.SetFloat("_Altitude0",1500f);
//			fogMaterial.SetFloat("_Altitude1",3500f);
//			fogMaterial.SetFloat("_FarDist",30000f);
//
//			fogMaterial.SetFloat("_Scatter",0.008f);
//			fogMaterial.SetFloat("_HGCoeff",0.5f);
//			fogMaterial.SetFloat("_Extinct",0.01f);
//
//
//			fogMaterial.SetFloat("_Exposure",1.3f);
//			fogMaterial.SetVector ("_GroundColor", new Vector3 (0.369f, 0.349f, 0.341f));//
//			fogMaterial.SetFloat("_SunSize",0.04f);
//			fogMaterial.SetVector ("_SkyTint", new Vector3 (0.5f, 0.5f, 0.5f));
//			fogMaterial.SetFloat("_AtmosphereThickness",1.0f);




			//SM v1.7
			fogMaterial.SetFloat("luminance",luminance);
			fogMaterial.SetFloat("lumFac",lumFac);
			fogMaterial.SetFloat("Multiplier1",ScatterFac);
			fogMaterial.SetFloat("Multiplier2",TurbFac);
			fogMaterial.SetFloat("Multiplier3",HorizFac);
			fogMaterial.SetFloat("turbidity",turbidity);
			fogMaterial.SetFloat("reileigh",reileigh);
			fogMaterial.SetFloat("mieCoefficient",mieCoefficient);
			fogMaterial.SetFloat("mieDirectionalG",mieDirectionalG);
			fogMaterial.SetFloat("bias",bias);
			fogMaterial.SetFloat("contrast",contrast);
			fogMaterial.SetVector("v3LightDir",-Sun.forward);
			fogMaterial.SetVector("_TintColor",new Vector4(TintColor.x,TintColor.y,TintColor.z,1));//68, 155, 345
			
			float Foggy = 0;
			if(FogSky){
				Foggy=1;
			}
			fogMaterial.SetFloat("FogSky",Foggy);
			fogMaterial.SetFloat("ClearSkyFac",ClearSkyFac);
			
			var sceneMode= RenderSettings.fogMode;
			var sceneDensity = 0.01f; //RenderSettings.fogDensity;//v3.0
			var sceneStart= RenderSettings.fogStartDistance;
			var sceneEnd= RenderSettings.fogEndDistance;
			Vector4 sceneParams;
			bool  linear = (sceneMode == FogMode.Linear);
			float diff = linear ? sceneEnd - sceneStart : 0.0f;
			float invDiff = Mathf.Abs(diff) > 0.0001f ? 1.0f / diff : 0.0f;
			sceneParams.x = sceneDensity * 1.2011224087f; // density / sqrt(ln(2)), used by Exp2 fog mode
			sceneParams.y = sceneDensity * 1.4426950408f; // density / ln(2), used by Exp fog mode
			sceneParams.z = linear ? -invDiff : 0.0f;
			sceneParams.w = linear ? sceneEnd * invDiff : 0.0f;
			fogMaterial.SetVector ("_SceneFogParams", sceneParams);
			fogMaterial.SetVector ("_SceneFogMode", new Vector4((int)sceneMode, useRadialDistance ? 1 : 0, 0, 0));
			
			int pass = 0;
			if (distanceFog && heightFog)
				pass = 0; // distance + height
			else if (distanceFog)
				pass = 1; // distance only
			else
				pass = 2; // height only


            //SM v1.7
            //if (colourPalette == null)  //v3.4.3
            //			{
            //				Color[] colourArray = new Color[256];			
            //				for(int i=0;i<colourArray.Length;i++){
            //					colourArray[i] = DistGradient.Evaluate((float)i/(float)colourArray.Length);
            //				}	
            //				
            //				if(!Made_texture){
            //					colourPalette = new Texture2D (256, 10, TextureFormat.ARGB32, false);
            //					colourPalette.filterMode = FilterMode.Point;
            //					colourPalette.wrapMode = TextureWrapMode.Clamp;
            //					Made_texture=true;
            //				}
            //				
            //				
            //				for(int x = 0; x < 256; x++){
            //					for(int y = 0; y < 10; y++){
            //						colourPalette.SetPixel(x,y,colourArray[x]);
            //					}
            //				}
            //				
            //				colourPalette.Apply();
            //				
            //				
            //			}

            //			if(colourArray == null){
            //				colourArray = new Color[gradientResolution];
            //			}
            //			if (prevGradient != DistGradient) {
            //				for(int i=0;i<colourArray.Length;i++){
            //					colourArray[i] = DistGradient.Evaluate((float)i/(float)colourArray.Length);
            //				}	
            //				if(!Made_texture){
            //					colourPalette = new Texture2D (gradientResolution, 10, TextureFormat.ARGB32, false);
            //					colourPalette.filterMode = FilterMode.Point;
            //					colourPalette.wrapMode = TextureWrapMode.Clamp;
            //					Made_texture=true;
            //				}
            //				for(int x = 0; x < gradientResolution; x++){
            //					for(int y = 0; y < 10; y++){
            //						colourPalette.SetPixel(x,y,colourArray[x]);
            //					}
            //				}
            //				colourPalette.Apply();
            //				prevGradient = DistGradient;
            //			}

            if (!downScale)
            {
				CustomGraphicsBlit(source, destination, fogMaterial, pass, DistGradient, GradientBounds, colourPalette, texture3Dnoise1, texture3Dnoise2, cam, WebGL);
            }
            else
            {
                //if (Time.fixedTime - LastUpdateTime > updateRate) {
                // LastUpdateTime = Time.fixedTime; //v4.1e


                //v2.1.15
                if (!fastest)
                {
                    if (tmpBuffer == null)
                    {
                        var format = cam.allowHDR ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;
                        tmpBuffer = RenderTexture.GetTemporary(source.width+125, source.height+125, 0, format);//v2.1.19 - add extra pixels to cover for frame displacement

                        RenderTexture.active = tmpBuffer;
                        GL.ClearWithSkybox(false, cam);
						if (blendBackground) { //v2.1.20
							backgroundCam.transform.rotation = cam.transform.rotation;
							backgroundCam.transform.position = cam.transform.position;
							//v2.1.23
							float ratio = (float)Screen.height / (float)Screen.width;						
							//backgroundCam.rect = new Rect (backgroundCam.rect.position, new Vector2 (backgroundCam.rect.size.x, ratio)); //v4.1
                            if (!unity2018)
                            {
                                backgroundCam.rect = new Rect(backgroundCam.rect.position, new Vector2(backgroundCam.rect.size.x, ratio));
                            }
                            else
                            {
                                //v4.1f
                                //backgroundCam.rect = new Rect(backgroundCam.rect.position, new Vector2(1, 1));
                                if (backgroundCam.targetTexture != null)
                                {
                                    backgroundCam.targetTexture.Release();
                                }
                                backgroundCam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
                            }

                            backgroundCam.Render ();
							Graphics.Blit(backgroundCam.targetTexture, tmpBuffer, backgroundMat);
						}
						CustomGraphicsBlitOpt(source, destination, tmpBuffer, fogMaterial, pass, DistGradient, GradientBounds, colourPalette, texture3Dnoise1, texture3Dnoise2, cam, true, false, WebGL);
						CustomGraphicsBlitOpt(source, destination, tmpBuffer, fogMaterial, pass, DistGradient, GradientBounds, colourPalette, texture3Dnoise1, texture3Dnoise2, cam, false, false, WebGL);
                    }
                    else
                    {
                        //v2.1.19
                        if (toggleCounter != 0 && Application.isPlaying && splitPerFrames > 0)
                        {
                            //Debug.Log("in1 =  "+splitPerFrames);
                            toggleCounter--;
							CustomGraphicsBlitOpt(source, destination, tmpBuffer, fogMaterial, pass, DistGradient, GradientBounds, colourPalette, texture3Dnoise1, texture3Dnoise2, cam, false, true, WebGL);
                        }else
                        {                            
                            toggleCounter = splitPerFrames;
                            RenderTexture.active = tmpBuffer;
                            GL.ClearWithSkybox(false, cam);
							if (blendBackground) { //v2.1.20
								backgroundCam.transform.rotation = cam.transform.rotation;
								backgroundCam.transform.position = cam.transform.position;
								//v2.1.23
								float ratio = (float)Screen.height / (float)Screen.width;						
								//backgroundCam.rect = new Rect (backgroundCam.rect.position, new Vector2 (backgroundCam.rect.size.x, ratio));//v4.1
                                if (!unity2018)
                                {
                                    backgroundCam.rect = new Rect(backgroundCam.rect.position, new Vector2(backgroundCam.rect.size.x, ratio));
                                }
                                else
                                {
                                    //v4.1f
                                    //backgroundCam.rect = new Rect(backgroundCam.rect.position, new Vector2(1, 1));
                                    if (backgroundCam.targetTexture != null)
                                    {
                                        backgroundCam.targetTexture.Release();
                                    }
                                    backgroundCam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
                                }

                                backgroundCam.Render ();
                                tmpBuffer.DiscardContents();//v4.1f
                                Graphics.Blit(backgroundCam.targetTexture, tmpBuffer, backgroundMat);
							}
							CustomGraphicsBlitOpt(source, destination, tmpBuffer, fogMaterial, pass, DistGradient, GradientBounds, colourPalette, texture3Dnoise1, texture3Dnoise2, cam, true, false, WebGL);
                            destination.DiscardContents();//v4.1f
                            tmpBuffer.DiscardContents();//v4.1f
                            CustomGraphicsBlitOpt(source, destination, tmpBuffer, fogMaterial, pass, DistGradient, GradientBounds, colourPalette, texture3Dnoise1, texture3Dnoise2, cam, false, false, WebGL);
                        }
                    }
                    
                }
                else
                {
					CustomGraphicsBlitOpt (source, destination,null, fogMaterial, pass, DistGradient, GradientBounds, colourPalette, texture3Dnoise1, texture3Dnoise2, cam, true, false, WebGL);
					CustomGraphicsBlitOpt (source, destination, null, fogMaterial, pass, DistGradient, GradientBounds, colourPalette, texture3Dnoise1, texture3Dnoise2, cam, false, false, WebGL);
                }
				//} else {
					//Graphics.Blit (source, destination);
					//CustomGraphicsBlitOpt (source, destination, fogMaterial, pass, DistGradient, GradientBounds, colourPalette, texture3Dnoise1, texture3Dnoise2, cam, false);
				//}
			}

		}

		RenderTexture tmpBuffer;//v2.1.15

		static void CustomGraphicsBlit (RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr,Gradient DistGradient, 
			Vector2 GradientBounds, Texture2D colourPalette, Texture3D tex3D1, Texture3D tex3D2, Camera cam, bool WebGL )
		{
			RenderTexture.active = dest;
			
			fxMaterial.SetTexture ("_MainTex", source);
			
			
			//v3.5
			fxMaterial.SetTexture("_NoiseTex1",tex3D1);
			fxMaterial.SetTexture("_NoiseTex2",tex3D2);
			//VFOG

			//v2.1.20
			int signer=1;
			if(WebGL){
				signer = -1;
			}

			//v4.0
			Camera cam1=cam;
			if (Camera.main != null) {
				cam1 = Camera.main;
			}

			fxMaterial.SetMatrix ("_WorldClip",  cam.cameraToWorldMatrix *Matrix4x4.Scale(new Vector3(1,1*signer,-1)) * cam1.projectionMatrix.inverse*Matrix4x4.Scale(new Vector3(-1,1,1))  ); 
			//fxMaterial.SetMatrix ("_WorldClip",  cam.cameraToWorldMatrix* cam.projectionMatrix.inverse  ); 

			
			fxMaterial.SetTexture ("_ColorRamp", colourPalette);
			
			if(GradientBounds != Vector2.zero){
				fxMaterial.SetFloat ("_Close", GradientBounds.x);
				fxMaterial.SetFloat ("_Far", GradientBounds.y);
			}			
			
			GL.PushMatrix ();
			GL.LoadOrtho ();
			
			fxMaterial.SetPass (passNr);
			
			GL.Begin (GL.QUADS);
			
			GL.MultiTexCoord2 (0, 0.0f, 0.0f);
			GL.Vertex3 (0.0f, 0.0f, 3.0f); // BL
			
			GL.MultiTexCoord2 (0, 1.0f, 0.0f);
			GL.Vertex3 (1.0f, 0.0f, 2.0f); // BR
			
			GL.MultiTexCoord2 (0, 1.0f, 1.0f);
			GL.Vertex3 (1.0f, 1.0f, 1.0f); // TR
			
			GL.MultiTexCoord2 (0, 0.0f, 1.0f);
			GL.Vertex3 (0.0f, 1.0f, 0.0f); // TL
			
			GL.End ();
			GL.PopMatrix ();
		}





	//float LastUpdateTime; //v4.1e
	public float updateRate = 0.3f;
	public int resolution = 256;
	public int downScaleFactor =1;
	public bool downScale = false;

	//K
	RenderTexture CreateBuffer()
	{
		//var width = (_columns + 1) * 2;
		//var height = _totalRows + 1;
			RenderTexture buffer = new RenderTexture(1280/downScaleFactor, 720/downScaleFactor, 0, RenderTextureFormat.ARGB32); //SM v4.0
		//buffer.hideFlags = HideFlags.DontSave;
		//buffer.hideFlags = HideFlags.None;
		buffer.filterMode = FilterMode.Point;
		buffer.wrapMode = TextureWrapMode.Repeat;
		//	buffer.c
		//	buffer.Create()
		return buffer;
	}
	RenderTexture _cloudBuffer;
	RenderTexture _prevcloudBuffer;
	void ResetResources()
	{
		if (_cloudBuffer) DestroyImmediate(_cloudBuffer);
		_cloudBuffer = CreateBuffer();

			if (_prevcloudBuffer) DestroyImmediate(_prevcloudBuffer);
			_prevcloudBuffer = CreateBuffer();

            //v4.5
            if (new1) DestroyImmediate(new1);
            new1 = CreateBuffer();

            _needsReset = false;
	}
	void LateUpdate()
	{
		if (_needsReset) ResetResources();
	}
	void Reset()
	{
		_needsReset = true;
	}
	public bool _needsReset = true;

    Vector3 prevCameraRot;

    RenderTexture new1; //v4.5

    //void CustomGraphicsBlitOpt (RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr,Gradient DistGradient, Vector2 GradientBounds, Texture2D colourPalette, Texture3D tex3D1, Texture3D tex3D2, Camera cam, bool toggle )
    void CustomGraphicsBlitOpt (RenderTexture source, RenderTexture dest, RenderTexture skysource, Material fxMaterial, int passNr,Gradient DistGradient, 
			Vector2 GradientBounds, Texture2D colourPalette, Texture3D tex3D1, Texture3D tex3D2, Camera cam, bool toggle, bool splitPerFrame, bool WebGL)
	{

            if (!fastest)
            {
                fxMaterial.SetInt("_fastest", 0);
            }
            else {
                fxMaterial.SetInt("_fastest", 1);
            }

            if (toggle)
            {

                //v2.1.15
                if (!fastest)
                {
                    fxMaterial.SetTexture("_SkyTex", skysource);
                }

                fxMaterial.SetTexture("_CloudTex", _cloudBuffer);

                //v4.1f
                //RenderTexture.active = _cloudBuffer;
                //RenderTexture new1 = CreateBuffer(); //v4.5
                //v4.5
                if (new1 == null)
                {
                    new1 = CreateBuffer();
                }

                RenderTexture.active = new1;

                //fxMaterial.SetTexture ("_MainTex", source);
                fxMaterial.SetTexture("_MainTex", null);

                //v3.5
                fxMaterial.SetTexture("_NoiseTex1", tex3D1);
                fxMaterial.SetTexture("_NoiseTex2", tex3D2);

				//VFOG (v3.4.9c - put -1 in second 1 for WebGL)
				//v2.1.20
				int signer=1;
				if(WebGL){
					signer = -1;
				}
				//v4.0
				Camera cam1=cam;
				if (Camera.main != null) {
					cam1 = Camera.main;
				}
				Matrix4x4 scaler = cam.cameraToWorldMatrix * Matrix4x4.Scale(new Vector3(1, 1*signer, -1)) * cam1.projectionMatrix.inverse * Matrix4x4.Scale(new Vector3(-1, 1, 1));//v2.1.19
                                                                                                                                                                            //scaler[0,3] = scaler[0,3] + 0.1f;
                fxMaterial.SetMatrix("_WorldClip", scaler);
                //fxMaterial.SetMatrix ("_WorldClip",  cam.cameraToWorldMatrix* cam.projectionMatrix.inverse  ); 


                fxMaterial.SetTexture("_ColorRamp", colourPalette);

                if (GradientBounds != Vector2.zero)
                {
                    fxMaterial.SetFloat("_Close", GradientBounds.x);
                    fxMaterial.SetFloat("_Far", GradientBounds.y);
                }




                GL.PushMatrix();
                GL.LoadOrtho();

                fxMaterial.SetPass(passNr);

                GL.Begin(GL.QUADS);

                GL.MultiTexCoord2(0, 0.0f, 0.0f);
                GL.Vertex3(0.0f, 0.0f, 3.0f); // BL

                GL.MultiTexCoord2(0, 1.0f, 0.0f);
                GL.Vertex3(1.0f, 0.0f, 2.0f); // BR

                GL.MultiTexCoord2(0, 1.0f, 1.0f);
                GL.Vertex3(1.0f, 1.0f, 1.0f); // TR

                GL.MultiTexCoord2(0, 0.0f, 1.0f);
                GL.Vertex3(0.0f, 1.0f, 0.0f); // TL

                GL.End();
                GL.PopMatrix();

                _cloudBuffer = new1;// RenderTexture.active; //v4.1f
                //_prevcloudBuffer = RenderTexture.active;

            }
            else
            {
                fxMaterial.SetTexture("_MainTex", source);
                if (!fastest)
                {
                    fxMaterial.SetTexture("_SkyTex", skysource);//v2.1.15
                }
                fxMaterial.SetTexture("_CloudTex", _cloudBuffer);
                RenderTexture.active = dest;

                //v2.1.19
				//v2.1.20
				int signer=1;
				if(WebGL){
					signer = -1;
				}
				//v4.0
				Camera cam1=cam;
				if (Camera.main != null) {
					cam1 = Camera.main;
				}
				Matrix4x4 scaler = cam.cameraToWorldMatrix * Matrix4x4.Scale(new Vector3(2, 1*signer, -1)) * cam1.projectionMatrix.inverse * Matrix4x4.Scale(new Vector3(-1, 1, 1));//v2.1.19

                //if (splitPerFrames > 0 && toggleCounter != splitPerFrames) {
                if (splitPerFrame)
                {
                    float Xdisp = cam.transform.eulerAngles.y - prevCameraRot.y;
                    float Ydisp = -(cam.transform.eulerAngles.x - prevCameraRot.x);

                    if(Xdisp > 12f || Xdisp < -12f)
                    {
                        //Debug.Log("Xdisp = " + Xdisp);
                        //Debug.Log("tmpBuffer width=" + tmpBuffer.width);
                       // Debug.Log("tmpBuffer height=" + tmpBuffer.height);
                        Xdisp = 0;
                    }
                    if (Ydisp > 12f || Ydisp < -12f)
                    {
                        //Debug.Log("Ydisp = " + Ydisp);
                        Ydisp = 0;
                    }

                    //Debug.Log("in " + (cam.transform.eulerAngles.x - prevCameraRot.x).ToString());
                    //Debug.Log("inA " + (cam.transform.eulerAngles.y - prevCameraRot.y).ToString());
                    //scaler[0, 3] = scaler[0, 3] - 1110.71f * (splitPerFrames - toggleCounter);//move in X, depending on camera motion and frames skipped
                    scaler[0, 3] = 0.009f * Xdisp;//0.005f * Xdisp;
                    scaler[1, 3] = 0.012f * Ydisp;
                    scaler[2, 3] = 1;// 1f - 0.005f * (Xdisp+Ydisp); // 0.95f+ 0.005f*(splitPerFrames - toggleCounter); //image scaler
                }
                else
                {
                    prevCameraRot = cam.transform.eulerAngles;
                }
                fxMaterial.SetMatrix("_WorldClip", scaler);

                GL.PushMatrix();
                GL.LoadOrtho();

                if (splitPerFrame && cameraMotionCompensate)
                {
                    fxMaterial.SetPass(4);
                }
                else {
                    fxMaterial.SetPass(3);
                }

			GL.Begin (GL.QUADS);

			GL.MultiTexCoord2 (0, 0.0f, 0.0f);
			GL.Vertex3 (0.0f, 0.0f, 3.0f); // BL

			GL.MultiTexCoord2 (0, 1.0f, 0.0f);
			GL.Vertex3 (1.0f, 0.0f, 2.0f); // BR

			GL.MultiTexCoord2 (0, 1.0f, 1.0f);
			GL.Vertex3 (1.0f, 1.0f, 1.0f); // TR

			GL.MultiTexCoord2 (0, 0.0f, 1.0f);
			GL.Vertex3 (0.0f, 1.0f, 0.0f); // TL

			GL.End ();
			GL.PopMatrix ();

		}

	}



	}
}

//Part of the code is based on MIT licensed code 
//MIT License
//Copyright(c) 2016 Unity Technologies
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files 
//(the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge,
//publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
//MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
//ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
//THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
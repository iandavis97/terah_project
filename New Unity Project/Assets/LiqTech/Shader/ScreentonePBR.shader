Shader "Screentone/PBR" {
	Properties {
		_BaseTexture ("Base Texture", 2D) = "white" {}
		_BaseTextureSize("Texture size", Int) = 32
		_BaseRotationAngle("Base Texture Rotation", Range(0, 6.3)) = 0
		_BasePrimaryColor ("Base Primary Color", Color) = (1,1,1,1)
		_BaseSecondaryColor ("Base Secondary Color", Color) = (0,0,0,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		

		[Toggle(FULLCOLOR)]
		_ToggleColor("Use full color", Float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows

		#pragma shader_feature FULLCOLOR

		#pragma target 3.0


		struct Input {
			float4 screenPos;
		};

		half _Glossiness;
		half _Metallic;
		
		sampler2D _BaseTexture;
		fixed4 _BasePrimaryColor;
		fixed4 _BaseSecondaryColor;
		float _BaseRotationAngle;
		int  _BaseTextureSize;

		inline float2 rotate(float2 pos, float angle){
				float sina = sin(angle);
				float cosa = cos(angle);
				return float2(pos.x * cosa - pos.y * sina, pos.y * cosa + pos.x * sina);
			}

		//As the default shader, but using screenspace UVs
		void surf (Input IN, inout SurfaceOutputStandard o) {
			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			screenUV.x *= _ScreenParams.x / _ScreenParams.y;
			screenUV *= _ScreenParams.y / _BaseTextureSize;
			screenUV = rotate(screenUV, _BaseRotationAngle);
			
			#ifdef FULLCOLOR
			o.Albedo = tex2D (_BaseTexture, screenUV) * _BasePrimaryColor;
			#else
			float c = tex2D (_BaseTexture, screenUV).a;
			o.Albedo = lerp(_BaseSecondaryColor, _BasePrimaryColor, c).rgb;
			#endif
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = 1;
			
		}
		ENDCG
	} 
	FallBack "Diffuse"
	CustomEditor "ScreentonePBREditor"
}

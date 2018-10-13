Shader "Screentone/Unlit" {
        Properties {
            _BaseTexture ("Base Texture", 2D) = "white" {}
			_BaseTextureSize("Base Texture Size", Int) = 32
			_BaseRotationAngle("Base Texture Rotation", Range(0, 6.3)) = 0
			_BasePrimaryColor ("Base Primary Color", Color) = (1,1,1,1)
			_BaseSecondaryColor ("Base Secondary Color", Color) = (0,0,0,1)
			

			[Toggle(FULLCOLOR)]
			_ToggleColor("Use full color", Float) = 0

			[MaterialKeywordEnum(Off, Back, Front)]
			_Cull("Cull", Int) = 2
        }

        SubShader {
        Tags { "RenderType" = "Opaque" }

		BlendOp Max
		Cull [_Cull]

        CGPROGRAM
		#include "UnityCG.cginc"
        #pragma surface surf SimpleLambert noambient
		
		#pragma shader_feature FULLCOLOR
		#pragma target 3.0

		
  
        sampler2D _BaseTexture;
		fixed4 _BasePrimaryColor;
		fixed4 _BaseSecondaryColor;
		float _BaseRotationAngle;
		int  _BaseTextureSize;

		sampler2D _ShadedTexture;
		fixed4 _ShadedPrimaryColor;
		fixed4 _ShadedSecondaryColor;
		float _ShadedRotationAngle;
		int  _ShadedTextureSize;

		float _ShadedThreshold;  

        struct Input {
            float4 screenPos;
			float4 color:COLOR;
        };

		struct SurfaceOutputCustom {
			fixed3 Albedo;
			fixed3 Shaded;
			fixed3 Gloss;
			fixed3 Normal;
			fixed3 Emission;
			half Specular;
			fixed Alpha;
		};
        
		//Just return the albedo color
		half4 LightingSimpleLambert (SurfaceOutputCustom s, half3 lightDir, half atten) {
			return half4(s.Albedo, 1);
        }

		inline float2 rotate(float2 pos, float angle){
			float sina = sin(angle);
			float cosa = cos(angle);
			return float2(pos.x * cosa - pos.y * sina, pos.y * cosa + pos.x * sina);
		}
		//Mapping screenspace to textures
        void surf (Input IN, inout SurfaceOutputCustom o) {
            float2 screenUVA = IN.screenPos.xy/ IN.screenPos.w;
			
			//screenUVA.x *= _ScreenParams.x / _ScreenParams.y;
			screenUVA.xy *= _ScreenParams.xy;
			screenUVA -= IN.color.xy;
			screenUVA /= _BaseTextureSize;
			
			
			screenUVA = rotate(screenUVA, _BaseRotationAngle);
			#ifdef FULLCOLOR
			o.Albedo = tex2D(_BaseTexture, screenUVA) * _BasePrimaryColor;
			#else
			float ca = tex2D (_BaseTexture, screenUVA).a;
			o.Albedo = lerp(_BaseSecondaryColor, _BasePrimaryColor, ca);
			#endif

			//o.Albedo = IN.color;
        }
        ENDCG
        }
        Fallback "Diffuse"
		CustomEditor "ScreentoneUnlitEditor"
    }
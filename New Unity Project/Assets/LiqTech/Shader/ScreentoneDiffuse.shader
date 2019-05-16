Shader "Screentone/Diffuse" {
        Properties {
            _BaseTexture ("Base Texture", 2D) = "white" {}
			_BaseTextureSize("Base Texture Size", Int) = 32
			_BaseRotationAngle("Base Texture Rotation", Range(0, 6.3)) = 0
			_BasePrimaryColor ("Base Primary Color", Color) = (1,1,1,1)
			_BaseSecondaryColor ("Base Secondary Color", Color) = (0,0,0,1)
			

			[Space(50)]

			_ShadedTexture ("Shaded Texture", 2D) = "white" {}
			_ShadedTextureSize("Shaded Texture Size", Int) = 32
			_ShadedRotationAngle("Shaded Texture Rotation", Range(0, 6.3)) = 0
			_ShadedPrimaryColor ("Shaded Primary Color", Color) = (1,1,1,1)
			_ShadedSecondaryColor ("Shaded Secondary Color", Color) = (0,0,0,1)
			

			_ShadedThreshold("Brightness threshold", Range(0.00001, 1)) = 0.5

			[Toggle(FULLCOLOR)]
			_ToggleColor("Use full color", Float) = 0

			[MaterialKeywordEnum(Off, Back, Front)]
			_Cull("Cull", Int) = 1
        }
        SubShader {
        Tags { "RenderType" = "Opaque" }

		BlendOp Max
		Cull [_Cull]

        CGPROGRAM
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
			
        };

		//A custom surface output is used to accomodate for separate gloss and shaded colors.
		struct SurfaceOutputCustom {
			fixed3 Albedo;
			fixed3 Shaded;
			fixed3 Gloss;
			fixed3 Normal;
			fixed3 Emission;
			half Specular;
			fixed Alpha;
		};
        
		//A simple calculation to determine brightness and select between textures based on it.
		half4 LightingSimpleLambert (SurfaceOutputCustom s, half3 lightDir,  half3 viewDir, half atten) {
			half3 normal = s.Normal * sign(dot(s.Normal, viewDir));
            half NdotL = dot (normal, lightDir);
			float brightness = saturate(NdotL) * atten * _LightColor0.rgb;


			if(brightness < _ShadedThreshold)
			{
				return half4(s.Shaded, 1);
			}
			else
			{
				return half4(s.Albedo, 1);
			}
        }

		inline float2 rotate(float2 pos, float angle){
			float sina = sin(angle);
			float cosa = cos(angle);
			return float2(pos.x * cosa - pos.y * sina, pos.y * cosa + pos.x * sina);
		}

		//Mapping screenspace to textures
        void surf (Input IN, inout SurfaceOutputCustom o) {
            float2 screenUVA = IN.screenPos.xy / IN.screenPos.w;
			screenUVA.x *= _ScreenParams.x / _ScreenParams.y;
			screenUVA *= _ScreenParams.y / _BaseTextureSize;
			screenUVA = rotate(screenUVA, _BaseRotationAngle);
			#ifdef FULLCOLOR
			o.Albedo = tex2D(_BaseTexture, screenUVA) * _BasePrimaryColor;
			#else
			float ca = tex2D (_BaseTexture, screenUVA).a;
			o.Albedo = lerp(_BaseSecondaryColor, _BasePrimaryColor, ca);
			#endif

			float2 screenUVB = IN.screenPos.xy / IN.screenPos.w;
			screenUVB.x *= _ScreenParams.x / _ScreenParams.y;
			screenUVB *= _ScreenParams.y / _ShadedTextureSize;
			screenUVB = rotate(screenUVB, _ShadedRotationAngle);
			#ifdef FULLCOLOR
			o.Shaded = tex2D(_ShadedTexture, screenUVB) * _ShadedPrimaryColor;
			#else
			float cb = tex2D (_ShadedTexture, screenUVB).a;
			o.Shaded = lerp(_ShadedSecondaryColor, _ShadedPrimaryColor, cb);
			#endif
        }
        ENDCG
        }
        Fallback "Diffuse"
		CustomEditor "ScreentoneDiffuseEditor"
    }
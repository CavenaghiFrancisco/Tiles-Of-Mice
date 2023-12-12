Shader"SauCa/Hologram"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_NormalTex ("Normal Texture", 2D) = "white" {}
		_NoiseVertexTex ("Noise Texture", 2D) = "black" {}
		_NoiseColorTex ("Noise Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
		_Bias ("Bias", Range(-1,10)) = 1
		_ClipAmount ("Clip Amount", Float) = 0
		[Enum (VerticalVertex, 0, HorizontalVertex, 1, VerticalUV, 2, HorizontalUV, 3)] _ClipType ("Clip Type", Range (0, 2)) = 0

		[Space(10)]
		_ScanningFrequency ("Scanning Frequency", Float) = 100
		_ScanningSpeed ("Scanning Speed", Float) = 100
		[Enum (Vertical, 0, Horizontal, 1, Front, 2, All, 3)] _HologramDirection ("Hologram Direction", Int) = 0
		[Enum (Front, 0, None, 1, Back, 2)] _HologramSignDirection ("Hologram Sign Direction", Int) = 0

		// Wobble
		[Space(10)]
		[Toggle] _EnableWobble ("Wobble Enabled", Float) = 1
		_WobbleFreq ("Wobble Frequency", Range(0,350)) = 100
		_WobbleSpeed ("Wobble Speed", Range(0,100)) = 20
		_WobbleAmount ("Wobble Amount", Range(0.005,0.15)) = 0.02

		//Noise
		[Space(10)]
		_NoiseVertex ("Noise Vertex", Range (0, 1)) = 1
		_NoiseNormal ("Noise Normal", Range (0, 20)) = 1
		_NoiseColors ("Noise Colors", Range (0, 10)) = 1

	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100
		ZWrite Off
		Blend SrcAlpha One
		Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				UNITY_FOG_COORDS(2)
				float4 vertex : SV_POSITION;
				float4 objVertex : TEXCOORD1;
			};

			fixed4 _Color;
			sampler2D _MainTex;
			sampler2D _NormalTex;
			sampler2D _NoiseVertexTex;
			sampler2D _NoiseColorTex;
			float4 _NoiseTex_ST;
			float4 _MainTex_ST;

			float _Bias;
			fixed _ClipAmount;
			float _ClipType;
			float _ScanningFrequency;
			float _ScanningSpeed;
			float _HologramDirection;
			float _HologramSignDirection;

			float _EnableWobble;
			float _WobbleFreq;
			float _WobbleAmount;
			float _WobbleSpeed;


			float _NoiseVertex;
			float _NoiseNormal;
			float _NoiseColors;

			v2f vert(appdata v)
			{
				v2f o;

				float3 uvNormal = tex2Dlod(_NormalTex, float4(v.uv.xy, 0, 0)).rgb;
				float3 uvNoise = tex2Dlod(_NoiseVertexTex, float4(v.uv.xy, 0, 0)).rgb;

				float3 wobble = float3(0,0,0);
				float3 noise = float3(0,0,0);
				float3 normal = float3(0,0,0);

				if (_EnableWobble > 0.5)				
					wobble =  v.normal * sin(_Time.y * _WobbleSpeed * v.vertex)  * _WobbleAmount;

				noise = (uvNoise / 50) * sin(_Time.w * _NoiseVertex * v.vertex.xyz);
				normal = (uvNormal / 50) * sin(_Time.w * _NoiseNormal * v.vertex.xyz);

				v.vertex.xyz += wobble + noise + normal;
				o.objVertex = mul(unity_ObjectToWorld, v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o, o.vertex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 uvNoise = tex2Dlod(_NoiseColorTex, float4(i.uv + (_Time.x)* _NoiseColors, 0, 0)).rgba;
				float4 colorNoise = uvNoise;

				if (_ClipType == 0)
				{
					float transition = _ClipAmount - i.objVertex.y;
					clip(transition + (tex2D(_NoiseColorTex, i.uv)));
				}
				if (_ClipType == 1)
				{
					float transition = _ClipAmount - i.objVertex.x;
					clip(transition + (tex2D(_NoiseColorTex, i.uv)));
				}
				if (_ClipType == 2)
					if (i.uv.y > _ClipAmount)
						discard;
				if (_ClipType == 3)
					if (i.uv.x > _ClipAmount)
						discard;

				if(colorNoise.r < 0.5f)
					discard;

				fixed4 colTex = tex2D(_MainTex, i.uv);
				UNITY_APPLY_FOG(i.fogCoord, colTex);

				float currentTime = _Time.x * (float)(_HologramSignDirection - 1);
				fixed4 colVertex = _Color;


				if (_HologramDirection == 0)
				{
					colVertex *= max(0, cos(i.objVertex.y * _ScanningFrequency + currentTime * _ScanningSpeed) + _Bias);
				}
				else if (_HologramDirection == 1)
				{
					colVertex *= max(0, cos(i.objVertex.x * _ScanningFrequency + currentTime * _ScanningSpeed) + _Bias);
				}
				else if (_HologramDirection == 2)
				{
					colVertex *= max(0, cos(i.objVertex.z * _ScanningFrequency + currentTime * _ScanningSpeed) + _Bias);
				}
				else if(_HologramDirection == 3)
				{
					colVertex.r *= max(0, cos(i.objVertex.x * _ScanningFrequency + currentTime * _ScanningSpeed) + _Bias);
					colVertex.g *= max(0, cos(i.objVertex.y * _ScanningFrequency + currentTime * _ScanningSpeed) + _Bias);
					colVertex.b *= max(0, cos(i.objVertex.z * _ScanningFrequency + currentTime * _ScanningSpeed) + _Bias);

					colVertex.a *= max(0, cos(i.objVertex.x * _ScanningFrequency + currentTime * _ScanningSpeed) + _Bias);
					colVertex.a *= max(0, cos(i.objVertex.y * _ScanningFrequency + currentTime * _ScanningSpeed) + _Bias);
					colVertex.a *= max(0, cos(i.objVertex.z * _ScanningFrequency + currentTime * _ScanningSpeed) + _Bias);
				}

				fixed4 finalColor = colTex * colVertex;
				finalColor.a *= colorNoise.a;

				return finalColor;
			}

			ENDCG
		}

	}

	CustomEditor "HologramMaterialEditor"
}
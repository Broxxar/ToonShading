Shader "MSLGiVG/ToonShading"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ToonLut ("Toon LUT", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_RimColor ("Rim Color", Color) = (1,1,1,1)
		_RimPower ("Rim Power", Range(0, 10)) = 1
	}
	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
		}

		Pass
		{
			Tags
			{
				"LightMode" = "ForwardBase"
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "Lighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal: NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float3 viewDir : TEXCOORD2;
			};

			sampler2D _MainTex;
			sampler2D _ToonLut;
			half3 _RimColor;
			half _RimPower;
			fixed4 _Color;

			v2f vert (appdata v)
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = normalize(UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, v.vertex)));

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 normal = normalize(i.normal);
				float ndotl = dot(normal, _WorldSpaceLightPos0);
				float ndotv = saturate(dot(normal, i.viewDir));

				float3 lut = tex2D(_ToonLut, float2(ndotl, 0));
				float3 rim = _RimColor * pow(1 - ndotv, _RimPower) * ndotl;

				float3 directDiffuse = lut * _LightColor0;
				float3 indirectDiffuse = unity_AmbientSky;

				fixed4 col = tex2D(_MainTex, i.uv) * _Color;
				col.rgb *= directDiffuse + indirectDiffuse;
				col.rgb += rim;
				col.a = 1.0;

				return col;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}

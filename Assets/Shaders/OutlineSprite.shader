Shader "MSLGiVG/OutlineSprite"
{
	Properties
	{
		[NoScaleOffset]
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Test Color", Color) = (1,1,1,1)
		_OutlineDist ("Outline Sample Distance", Range(0, 5)) = 2.0
	}
	SubShader
	{
		Tags
		{
			"PreviewType" = "Plane"
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}

		Pass
		{
			Cull Off
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}			

			sampler2D _MainTex;
			float2 _MainTex_TexelSize;
			float4 _Color;
			float _OutlineDist;

			float Outline(float2 uv, float size)
			{
				float2 Disc[16] =
				{
					float2(0, 1),
					float2(0.3826835, 0.9238796),
					float2(0.7071069, 0.7071068),
					float2(0.9238796, 0.3826834),
					float2(1, 0),
					float2(0.9238795, -0.3826835),
					float2(0.7071068, -0.7071068),
					float2(0.3826833, -0.9238796),
					float2(0, -1),
					float2(-0.3826835, -0.9238796),
					float2(-0.7071069, -0.7071067),
					float2(-0.9238797, -0.3826832),
					float2(-1, 0),
					float2(-0.9238795, 0.3826835),
					float2(-0.7071066, 0.707107),
					float2(-0.3826834, 0.9238796)
				};

				float maxAlpha = 0;

				for (int d = 0; d < 16; d++)
				{
					float sampleAlpha = tex2D(_MainTex, uv + Disc[d] * _MainTex_TexelSize * size).a;
					maxAlpha = max(sampleAlpha, maxAlpha);
				}

				return maxAlpha;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				col.a = max(col.a, (Outline(i.uv, _OutlineDist)));

				return col;
			}
			ENDCG
		}
	}
}

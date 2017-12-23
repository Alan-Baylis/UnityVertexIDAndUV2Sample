Shader "Unlit/UV2"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Cull off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			#define POS_SIZE 8

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				fixed4 color : TEXCOORD1;
			};


			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _posList[POS_SIZE];

			v2f vert (appdata v) // uint vid : SV_VertexID you can do this way as well but it needs openGLES3 above
			{
				v2f o;

				int vid = v.uv2.x; // replace for SV_VertexID

				v.vertex.x += _posList[vid].x;
				v.vertex.y += _posList[vid].y;
				v.vertex.z += _posList[vid].z;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				float f = (float)vid;
                o.color = half4(sin(f/10),sin(f/100),sin(f/1000),0) * 0.5 + 0.5;

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target{
				return i.color;
			}
			ENDCG
		}
	}
}

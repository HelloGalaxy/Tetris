Shader "Custom/SingleTexture" {
	
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Specular ("Specular", Color) = (1,1,1,1)
		_Gloss ("Gloss", Range(8.0, 256)) = 20
	}
	
	SubShader {
		Tags { "LightMode"="FowrardPass" }
		
		CGPROGRAM
		
		#pragma vertex vert
		#pragma fragment fragment
		
		#include "Lighting.cginc"

		fixed4 _Color;
		simpler2D _MainTex;
		float4 _MainTex_ST;
		fixed4 _Specular;
		float _Closs;

		struct a2v {
			float4 vertext : POSITION;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
		};
		
		struct v2f {
			float4 pos : SV_POSITION;
			float3 worldNormal : TEXCOORD0;
			float3 worldPos : TEXCOORD1;
			float2 uv : TEXCOORD2;
		};

		
		v2f vert(a2v v) {
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertext);
			o.worldNormal = UntiyObjectToWorldNormal(v.normal);
			o.worldPos = mul(_Object2World, v.vertext).xyz;
			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
		}
		
		fixed4 frag(v2f i) : SV_Target {
			
			return fixed4(0);
		}
		
		ENDCG
	}
	FallBack "Diffuse"
}

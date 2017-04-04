// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/RampTexture" {
	
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_RampTex ("Ramp Tex", 2D) = "white" {}
		_Specular ("Specular", Color) = (1,1,1,1)
		_Gloss ("Gloss", Range(8.0, 256)) = 20
	}
	
	SubShader {
		Pass {
			Tags { "LightMode"="ForwardBase" }
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "Lighting.cginc"

			fixed4 _Color;
			sampler2D _RampTex;
			float4 _RampTex_ST;
			fixed4 _Specular;
			float _Gloss;

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
				o.pos = UnityObjectToClipPos(v.vertext);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertext).xyz;
				
				o.uv = TRANSFORM_TEX(v.texcoord, _RampTex);
				//o.uv =  v.texcoord.xy * _RampTex_ST.xy + _RampTex_ST.zw;
				
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target {
				
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
				

				//罗伯特算法
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				fixed halfLambert = 0.5 * dot(worldNormal, lightDir) + 0.5;
				fixed2 newUV =  fixed2(halfLambert, halfLambert);
				fixed3 diffuse = tex2D(_RampTex, newUV).rgb * _Color.rgb;
				
				
				//fixed3 albedo = tex2D(_RampTex, i.uv).rgb * _Color.rgb;
				//fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
				//fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(worldNormal, lightDir));
				
				fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
				fixed3 halfDir = normalize(lightDir + viewDir);
				fixed3 specualr = _LightColor0.rgb * _Specular.rgb * pow( max(0, dot(worldNormal, halfDir)), _Gloss);
				
				return fixed4(ambient + diffuse + specualr, 1.0);
			}
			
			ENDCG
		}
	}
	FallBack "Specular"
}
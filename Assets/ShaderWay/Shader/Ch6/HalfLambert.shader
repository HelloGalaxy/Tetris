// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/HalfLambert" {

		Properties{
			_Diffuse ("Difusse", Color) = (1,1,1,1)
		}

		SubShader {
			Pass {

				Tags { "LightMode" = "ForwardBase" }
			
				CGPROGRAM
				
				#pragma vertex vert
				#pragma fragment frag
				#include "Lighting.cginc"

				fixed4 _Diffuse;
				
				struct a2v {
					float4 vertex: POSITION;
					float3 normal: NORMAL;
				};
				
				struct v2f {
					fixed4 pos : SV_POSITION;
					float3 worldNormal: TEXCOORD0;
				};
				
				//由顶点处理光照
				v2f vert(a2v v) {				
					v2f o;
					
					//物体的顶点变换到世界坐标系
					o.pos = UnityObjectToClipPos(v.vertex);
					o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);// mul(UNITY_MATRIX_MVP, v.normal);				
					return o;
				}
				
				fixed4 frag(v2f i) : SV_Target {
					
					fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
					fixed3 worldNormal = normalize(i.worldNormal);
					fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.pos));//_WorldSpaceLightPos0.xyz
					
					fixed3 diffuse = (_LightColor0.rgb * _Diffuse.rgb) * (0.5 + 0.5 * dot(worldNormal, worldLightDir));

					fixed3 color = ambient + diffuse;
					return fixed4(color, 1.0);
				}
				
				ENDCG
			}
		}

		FallBack "Diffuse"
}

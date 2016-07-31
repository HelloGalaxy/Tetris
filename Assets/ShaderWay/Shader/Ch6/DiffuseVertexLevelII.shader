// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/DiffuseVertexLevelII" {

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
					fixed3 color : COLOR;
				};
				
				//由顶点处理光照
				v2f vert(a2v v) {				
					v2f o;
					
					//物体的顶点变换到世界坐标系
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					
					fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
					fixed3 worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
					fixed3 worldLight = normalize(WorldSpaceLightDir(o.pos));
					fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * saturate(dot(worldNormal, worldLight));

					o.color = ambient + diffuse;					
					
					return o;
				}
				
				fixed4 frag(v2f i) : SV_Target {
					return fixed4(i.color, 1.0);
				}
				
				ENDCG
			}
		}

		FallBack "Diffuse"
}

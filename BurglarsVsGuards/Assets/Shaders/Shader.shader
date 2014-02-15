Shader "Custom/Shader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (0.3,0.3,0.3,1)
		_Hidden ("Hidden", Color) = (0.1,0.1,0.1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry+3" }
		LOD 200
		
		Pass {
			Material {
                Diffuse [_Hidden]
            }
			Lighting off
		}
		Pass {
			Stencil {
				Ref 3
				Comp equal
				Pass keep
				Fail keep
			}
		
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float4 _Color;

			float4 frag(v2f_img i) : COLOR {
				float4 texcolor = tex2D(_MainTex, i.uv);
				float4 outcolor = float4(0,0,0,1);

				outcolor.r = (texcolor.r+texcolor.g+texcolor.b)/3;
				outcolor.g = (texcolor.r+texcolor.g+texcolor.b)/3;
				outcolor.b = (texcolor.r+texcolor.g+texcolor.b)/3;

				return outcolor*_Color;
			}
			ENDCG
		}
		Pass {
			Stencil {
				Ref 2
				Comp equal
				Pass keep
				Fail keep
			}
		
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;

			float4 frag(v2f_img i) : COLOR {
				return tex2D(_MainTex, i.uv);
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}

Shader "Custom/Shader Object" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry+3" }
		LOD 200

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
			uniform float4 _Color;

			float4 frag(v2f_img i) : COLOR {
				float4 c = tex2D(_MainTex, i.uv);
				if(c.a < 0.5f){
					discard;
				}
				return c*_Color;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}

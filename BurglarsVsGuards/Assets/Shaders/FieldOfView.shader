Shader "Custom/FieldOfView" {
	Properties {
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry+2" }
		LOD 200
		Pass {
			Stencil {
				Ref 2
				Comp never
				Fail replace
			}
		}
	} 
}

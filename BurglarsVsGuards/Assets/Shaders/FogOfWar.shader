﻿Shader "Custom/FogOfWar" {
	Properties {
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry+1" }
		LOD 200
		Pass {
			Stencil {
				Ref 3
				Comp never
				Fail replace
			}
		}
	} 
}

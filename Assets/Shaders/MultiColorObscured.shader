Shader "Custom/Standard"
{
	Properties
	{
		_Albedo("Albedo (RGB), Alpha (A)", Color) = (1.0,1.0,1.0,1.0)
		_Metalic("Metalic", Range(0,1)) = 0.5
		_Smoothness("Smoothness", Range(0,1)) = 0.5
		_Alpha("Obscured Alpha", Range(0,1)) = 0.5
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Geometry"
			"RenderType" = "Transparent"
		}

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		ZTest GEqual

		CGINCLUDE
		#define _GLOSSYENV 1
		ENDCG

		CGPROGRAM
		#pragma target 3.0
		#include "UnityPBSLighting.cginc"
		#pragma surface surf Standard alpha
		#pragma exclude_renderers gles

		struct Input
		{
			float2 uv_Albedo;
		};

		float4 _Albedo;
		float _Metallic;
		float _Smoothness;
		float _Alpha;

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Albedo.rgb;
			o.Alpha = _Alpha;
			o.Smoothness = _Smoothness;
			o.Metallic = _Metallic;
		}
		ENDCG

		CGINCLUDE
		#define _GLOSSYENV 1
		ENDCG

		ZTest Less
		ZWrite On

		CGPROGRAM
		#pragma target 3.0
		#include "UnityPBSLighting.cginc"
		#pragma surface surf Standard
		#pragma exclude_renderers gles

		struct Input
		{
			float2 uv_Albedo;
		};

		float4 _Albedo;
		float _Metallic;
		float _Smoothness;
		float _Alpha;

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Albedo.rgb;
			o.Alpha = _Albedo.a;
			o.Smoothness = _Smoothness;
			o.Metallic = _Metallic;
		}
		ENDCG
	}

		FallBack "Standard"
}
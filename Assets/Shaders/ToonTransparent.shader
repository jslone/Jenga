Shader "Custom/ToonTransparent"
{
	Properties
	{
		
		_Color("Main Color", Color) = (1.0,1.0,1.0,1.0)
		_ObscuredColor("Obscured Color", Color) = (1.0, 1.0, 1.0, 0.5)
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Geometry"
		"RenderType" = "Transparent"
	}

		Pass
		{
			Name "BASE"
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest GEqual
			Color[_ObscuredColor]
		}

		Pass
		{
			Name "BASE"
			ZTest Less
			ZWrite On
			Color[_Color]
		}
		
	}

		FallBack "Standard"
}
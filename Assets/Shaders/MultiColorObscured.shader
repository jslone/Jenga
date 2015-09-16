Shader "UI/MultiColorObscured"
{
	Properties
	{
		_Color1("VisibleColor", Color) = (1,1,1,1)
		_Color2("ObscuredColor", Color) = (1,1,1,1)
	}
		SubShader
	{
		Tags{ "Queue" = "Transparent" }
		Pass
	{
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		ZTest Greater
		Color[_Color2]
	}
		Pass
	{
		ZTest Less
		Color[_Color1]
	}
	}
}
Shader "Custom/ToonOutlined" {
	Properties{
		_Color("Main Color", Color) = (1.0,1.0,1.0,1.0)
		_ObscuredColor("Obscured Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_ObscuredOutlineColor("Obscured Outline Color", Color) = (0,0,0,0.5)
		_Outline("Outline width", Range(.002, 0.03)) = .005
	}

	SubShader{
		UsePass "Custom/ToonTransparent/BASE"
		UsePass "Toon/Basic Outline/OUTLINE"
	}
}

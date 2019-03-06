// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Custom/RimLight" {

	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)

		_MainTex("Base (RGB)", 2D) = "white" {}

	_IlluminPower("Illumin Power", Range(0, 2)) = 1

		_RimColor("Rim Color", Color) = (1, 1, 1, 1)

		_RimPower("Rim Power", Float) = 0.7

	}

	SubShader
	{
	Pass
	{
		Lighting Off

		CGPROGRAM



#pragma vertex vert

#pragma fragment frag

#include "UnityCG.cginc"



	uniform sampler2D _MainTex;

	uniform fixed4 _Color;

	uniform float4 _MainTex_ST;

	float _IlluminPower;

	uniform fixed4 _RimColor;

	float _RimPower;



	struct appdata {

		float4 vertex : POSITION;

		float3 normal : NORMAL;

		float2 texcoord : TEXCOORD0;

	};



	struct v2f {

		float4 pos : SV_POSITION;

		float2 uv : TEXCOORD0;

		fixed3 color : COLOR;

	};



	v2f vert(appdata_base v)
	{
		v2f o;

		o.pos = UnityObjectToClipPos(v.vertex);

		float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
		float dotProduct = 1 - dot(v.normal, viewDir);

		o.color = _RimColor * pow(dotProduct, _RimPower);//smoothstep(1 - _RimPower, 1.0, dotProduct);

		o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

		return o;
	}





	fixed4 frag(v2f i) : COLOR
	{
		fixed4 texcol = tex2D(_MainTex, i.uv);

		texcol.rgb = texcol.rgb  * _Color.rgb * _IlluminPower + i.color;

		return texcol;
	}



		ENDCG

	}

	}

}
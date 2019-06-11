// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/FlippedColorBodyShader" {
    Properties {
        _BodyTex ("Body (RGB)", 2D) = "white" {}
        _ColorTex ("Color (RGB)", 2D) = "white" {}
        _ColorFlipH ("Horizontal Flip", Int) = 0
        _ColorFlipV ("Vertical Flip", Int) = 1
    }
    
	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
		
			CGPROGRAM
			#pragma target 5.0
			//#pragma enable_d3d11_debug_symbols

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _BodyTex;
			uniform sampler2D _ColorTex;
			
			uniform int _ColorFlipH;
			uniform int _ColorFlipV;
			
			struct v2f {
				float4 pos : SV_POSITION;
			    float2 uv : TEXCOORD0;
			};

			v2f vert (appdata_base v)
			{
				v2f o;
				
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv = v.texcoord;
				
				return o;
			}

			float4 frag (v2f i) : COLOR
			{
				float player = tex2D(_BodyTex, i.uv).w;
				if (player != 0)
				{
					float2 c_uv;
					c_uv.x = _ColorFlipH ? 1 - i.uv.x : i.uv.x;
					c_uv.y = _ColorFlipV ? 1 - i.uv.y : i.uv.y;

					float4 clr = tex2D (_ColorTex, c_uv);
					clr.w = player < 0.8 ? player : 1;
					return clr;
				}
				
				return float4(0, 0, 0, 0);
			}

			ENDCG
		}
	}

	Fallback Off
}
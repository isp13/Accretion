Shader "Hidden/Chunky"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_SprTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex; // текстура экрана
			sampler2D _SprTex; // лист chunky
			float4 _Color = float4(1, 1, 1, 1); // управление яркостью экрана
			float2 BlockCount; // количество блоков в экране по Ox и Oy
			float2 BlockSize; // размер блока в экранной облости

			fixed4 frag(v2f_img i) : SV_Target
			{
				// (1)
				float2 blockPos = floor(i.uv * BlockCount);
				float2 blockCenter = blockPos * BlockSize + BlockSize * 0.5;

				// (2)
				float4 del = float4(1, 1, 1, 1) - _Color;

				// (3)
				float4 tex = tex2D(_MainTex, blockCenter) - del;
				float grayscale = dot(tex.rgb, float3(0.3, 0.59, 0.11));
				grayscale = clamp(grayscale, 0.0, 1.0);

				// (4)
				float dx = floor(grayscale * 16.0);

				// (5)
				float2 sprPos = i.uv;
				sprPos -= blockPos*BlockSize;
				sprPos.x /= 16;
				sprPos *= BlockCount;
				sprPos.x += 1.0 / 16.0 * dx;

				// (6)
				float4 tex2 = tex2D(_SprTex, sprPos);
				return tex2;
			}
			ENDCG
		}
	}
}

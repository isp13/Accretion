/************************************************************************************************************************/

Shader "Unlit/Simple Sun"
{
	/************************************************************************************************************************/

	Properties
	{
		[KeywordEnum(Basic, Dual Color, Textured)]
		_Mode("Mode", float) = 0

		_Color("Primary Color", Color) = (1,1,1,1)
		_SecondaryColor("Secondary Color", Color) = (1,1,1,1)

		_BallRadius("Ball Radius", Range(0, 0.5)) = 0.45
		_BallSharpness("Ball Sharpness", float) = 6

		_RayFrequency("Ray Frequency", float) = 2// Whole numbers are recommended to avoid creating a seam.
		_RayIntensity("Ray Intensity", float) = 100
		_RayRoughness("Ray Roughness", Range(0, 1.5)) = 1
		
		_Speed("Speed", float) = 3

		[NoScaleOffset]
		_Noise("Noise", 2D) = "white" {}
		
		_TextureScale("Texture Scale", float) = 4
	}

	/************************************************************************************************************************/

	Category
	{
		/************************************************************************************************************************/

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
		}

		LOD 200

		Blend SrcAlpha OneMinusSrcAlpha
		Alphatest Greater 0
		ZWrite Off

		SubShader
		{
			Pass
			{
				/************************************************************************************************************************/

				CGPROGRAM

				#include "UnityCG.cginc"
				#pragma vertex VertexProgram
				#pragma fragment FragmentProgram

				#pragma multi_compile _MODE_BASIC _MODE_DUAL_COLOR _MODE_TEXTURED

				float4 _Color;
				float4 _SecondaryColor;

				float _BallRadius;
				float _BallSharpness;

				float _RayFrequency;
				float _RayIntensity;
				float _RayRoughness;
				float _TextureScale;
				float _Speed;
				sampler2D _Noise;

				/************************************************************************************************************************/

				struct VertexToFragment
				{
					float4 position : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				/************************************************************************************************************************/

				VertexToFragment VertexProgram(appdata_base v)
				{
					VertexToFragment output;
					output.position = UnityObjectToClipPos(v.vertex);
					output.uv = v.texcoord;
					return output;
				}

				/************************************************************************************************************************/

				float4 FragmentProgram(VertexToFragment IN) : COLOR
				{
					float time = _Time.x;

					// Get angle and distance from the centre (uv 0.5, 0.5) to the current fragment.
					IN.uv.x -= 0.5;
					IN.uv.y -= 0.5;
					float angle = atan2(IN.uv.y, IN.uv.x) * 0.159154943;// 0.5 / Pi = 0.159154943...
					float distance = sqrt(IN.uv.x * IN.uv.x + IN.uv.y * IN.uv.y);

					float tex = tex2D(_Noise, float2(IN.uv.x + time * _Speed, IN.uv.y) * _TextureScale).x;
					tex = sin(tex * 3.14 * 4 + time * _Speed * 5) * 0.15 + 0.45;
					//tex *= tex;

					IN.uv /= distance;// Normalise.

					// Calculate the ball intensity.
					float ball = saturate(_BallRadius - distance) * _BallSharpness;
					ball *= ball;

					// Calculate the ray intensity.
					time *= abs(_Speed);
					float noise = tex2D(_Noise, IN.uv + float2(time, time * 0.2)).x;

					IN.uv.x = (distance - time) * _RayRoughness;
					IN.uv.y = angle * _RayFrequency;
					float ray = tex2D(_Noise, IN.uv).x;
					ray = ray * noise * 1.5;
					ray = saturate(0.5 - distance) * ray * ray;
					ray = ray * ray * _RayIntensity;

					float rayBrightness = (ray + ball - noise) * 0.25;

					#ifdef _MODE_TEXTURED
					_Color.rgb = lerp(_Color.rgb, _SecondaryColor.rgb, ball * tex);
					#elif _MODE_DUAL_COLOR
					_Color.rgb = lerp(_Color.rgb, _SecondaryColor.rgb, noise * noise);
					#endif

					_Color.rgb += rayBrightness.xxx;
					_Color.a *= ball + ray;
					return _Color;

					// The angle * 0.159154943 calculation should result in a value that goes from -0.5 to +0.5 around the circle so that
					// the subsequent texture lookup wraps perfectly through the texture (using Repeat as the Wrap Mode). Unfortunately it
					// doesn't quite line up and if you look closely at the right side you can see a seam at certain times.
					// If you are able to solve this issue, please let us know at kybernetikgames@gmail.com.
				}

				/************************************************************************************************************************/

				ENDCG

				/************************************************************************************************************************/
			}
		}
	}

	/************************************************************************************************************************/

	FallBack "Diffuse"

	/************************************************************************************************************************/
}

/************************************************************************************************************************/
// note that when it is stored in the Hidden cateogory
// we will not be able to associate it with our 
// materials in unity
//Shader "Hidden/NewImageEffectShader"

// it seems in general that these shaders are attached or associated with materials
// inside unity. 
Shader "Explorer/mandelbrot"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}

// the subshader represents the code that is 
// run for every pixel in our image!
	SubShader
	{
		// there is a possibility of adding tags here as well
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				float2 c = i.uv;
				float2 z;
				float iter;
				for (iter = 0; iter < 255; iter++) {
					z = float2(z.x*z.x - z.y*z.y, 2 * z.x*z.y) + c;
					if (length(z) > 2) {
						break;
					}
					
				}
				return iter / 255;
			}
			ENDCG
		}
	}
}

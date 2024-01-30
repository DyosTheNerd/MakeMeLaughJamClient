Shader "Unlit/UniverseBackground"
{
    Properties
    {
        _FineNoiseTex("Fine Noise Texture", 2D) = "white" {}
        _CoarseNoiseTex("Coarse Noise Texture", 2D) = "white" {}
        _NoiseSlice ("NoiseSlice", Vector) = (1,1,1,1)
        _StarHarmonics ("Star Harmonics", Vector) = (0,3,0,0)
        _StarGridSide ("Bright Star Grid Side", Float) = 5
        _StarBright ("Star Brightness", Float) = 0.3
        _StarSat ("Star Saturation", Float) = 0.6
        _TimeScale ("TimeScale", Float) = 1.0
        _BGSat ("BG Saturation", Float) = 0.2
        _BGBright ("BG Brightnes", Float) = 0.2
        _BGIntensity ("BGIntensity", Float) = 0.3
        _SlideIntensity ("SlideIntensity", Float) = 5
        _StarDensity ("Star Density", Float) = 0.2
        _NebulaIntensity ("Nebula Density", Float) = 0.2
        _NebulaSat ("Nebula Saturation", Float) = 0.2
        _NebulaBright ("Nebula Brightness", Float) = 0.2
        _NebulaHarmonics("Nebula Harmonics", Vector) = (2, 5, 0, 0)
        _NebulaHue ("Nebula Hue", Float) = 0.3
        _NebulaRampTex("Nebula Ramp Texture", 2D) = "white" {}
        _StarDirection("StarDirection", Vector) = (1,0,0,0)
        _RampLine("Ramp Line", Int) = 0
        _NebulaValue("NebulaValue", Float) = 1.0
        _NebulaSpeed("NebulaSpeed", Float) = 1.0
        _NebulaOffset("NebulaOffset", Float) = 1.0

        _NebulaDistortion("NebulaDistortion", Float) = 1.0
        _NebulaDistTex ("NebulaDistortionTex", 2D)  = "black" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Include/coordinates.hlsl"
            #include "Include/hsv.hlsl"

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

            float4 _CoarseNoiseTex_ST;
            sampler2D _FineNoiseTex, _CoarseNoiseTex, _NebulaRampTex;
            float2 _NoiseSlice;
            uint2 _StarHarmonics;
            float _StarGridSide, _NebulaIntensity, _StarDensity, _StarBright, _StarSat, _TimeScale, _BGSat, _BGBright, _BGIntensity, _SlideIntensity;
            float _NebulaSat, _NebulaBright, _NebulaHue;
            uint2 _NebulaHarmonics;
            //float2 _MousePosition;
            float _RampLine;
            float2 _StarDirection;
            float _NebulaValue, _NebulaSpeed, _NebulaOffset, _NebulaDistortion;
            sampler2D  _NebulaDistTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _CoarseNoiseTex);
                return o;
            }


            float2 hash22(float2 xy) {
                return float2(frac(sin(dot(xy, float2(64.432432, 27.564749))) * 514.9456)
                    , frac(sin(dot(xy, float2(631.6554897, 78.16556))) * 897.5898489));
            }

            float hash21(float2 x)
            {
                return frac(sin(dot(x, float2(29.54983, 3.123))) * 54398.13124);
            }

            
            float hash11(float x)
            {
                return frac(sin(x + 445.5234) * 117.523354);
            }

            


            float4 BrightStar(float2 uv, float4 noise_col, float harmonic) 
            {
                harmonic *= 0.3f;
                //float2 mouse_offset = (_MousePosition / _ScreenParams.xy) * 5* _SlideIntensity * harmonic;
                float2 time_offset = _StarDirection * (_Time.y / harmonic) / _TimeScale;
                float2 harmonic_offset = uv * harmonic;

                float4 star_grid_coords = cell_coordinates(uv /*+ mouse_offset */+ time_offset + harmonic_offset, _StarGridSide * harmonic);


                float4 color = 0.0f;
                float intensity = abs(sin((_Time.x / 10 + star_grid_coords.z / 10)) /2 );

                

                for (int k = -1; k <= 1; k++)
                {
                    for (int l = -1; l <= 1; l++)
                    {
                        float2 offset = float2(k, l);
                        float2 offsetcoords = star_grid_coords.xy +  offset;

                        float3 col = HSV2RGB(float3(hash11(hash21(offsetcoords)), _StarSat, _StarBright));

                        float2 rand_point = hash22(offsetcoords * harmonic);

                        float density = hash21(offsetcoords + 37.0f);

                        float d = distance(star_grid_coords.zw  , rand_point.xy + offset);
                        color += float4(col / (d * d), 1.0) * step(density, _StarDensity);
                    }
                }
                return color * intensity;
            }



            fixed4 frag(v2f i) : SV_Target
            {
                float4 col = float4(0.0f, 0.0f, 0.0f, 0.0f);
                // sample the texture
                //float4 sliding_coarse_noise = tex2D(_CoarseNoiseTex, (i.uv + _SinTime.x / 30) /2);
                float4 fine_noise = tex2D(_FineNoiseTex, i.uv);
                
                //float4 bg_color = float4( HSV2RGB(float3(sliding_coarse_noise.x, _BGSat, _BGBright)) * sliding_coarse_noise.y * _BGIntensity, 1.0f);
                float4 bg_color = float4(0.0f, 0.0f, 0.0f, 0.0f);

                float4 front_stars = float4(0.0f, 0.0f, 0.0f, 1.0f);
                float4 back_stars = float4(0.0f, 0.0f, 0.0f, 1.0f);

                for (float h = _StarHarmonics.x; h < _StarHarmonics.y; h++)
                {
                    front_stars += BrightStar(i.uv , fine_noise, h) / h;
                }

                col = front_stars;

                return bg_color + col;
            }
            ENDCG
        }
    }
}

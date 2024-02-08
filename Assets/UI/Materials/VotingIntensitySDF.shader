Shader "Unlit/VotingIntensitySDF"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        _Color2("Tint2", Color) = (1,1,1,1)

        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255

        _ColorMask("Color Mask", Float) = 15

        _DistScale("Distance Scale", Float) = 1
        _SizeScale("Size Scale", Float) = 1
        _MinimumScale("MinimumSize", Float) = 0.1
        _CentralPosition("Central Position", Vector) = (0, 0,0,0)
        _Frequency("Frequency", Float) = 0.7
        _TimeScale("TimeScale", Float) = 5
        _PulseWidth("PulseWidth", Float) = 0.1
        _Rotation("Rotation", Float) = 0
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
    }

        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }

            Stencil
            {
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }

            Cull Off
            Lighting Off
            ZWrite Off
            ZTest[unity_GUIZTestMode]
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask[_ColorMask]

            Pass
            {
                Name "Default"
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 because it uses wrong array syntax (type[size] name)
                #pragma vertex vert
                #pragma fragment frag
                //#pragma target 2.0

                #include "UnityCG.cginc"
                #include "UnityUI.cginc"

                #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
                #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

                struct appdata_t
                {
                    float4 vertex   : POSITION;
                    float4 color    : COLOR;
                    float2 texcoord : TEXCOORD0;
                    uint id : SV_VertexID;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct v2f
                {
                    float4 vertex   : SV_POSITION;
                    fixed4 color : COLOR;
                    float2 texcoord  : TEXCOORD0;
                    float4 worldPosition : TEXCOORD1;
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                sampler2D _MainTex;
                fixed4 _Color;
                fixed4 _Color2;
                fixed4 _TextureSampleAdd;
                float4 _ClipRect;
                float4 _MainTex_ST;
                float _DistScale;
                float _SizeScale;
                float _MinimumScale;
                float2 _CentralPosition;
                float _Frequency;
                float _TimeScale;
                float _PulseWidth;
                //float _IntensityValues[8] = {0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0};
                float4 _VertexPositions[8];
                float _Rotation;
                float2 GetVertexPosition(uint vert_id) 
                {
                    //float2  dir = normalize(position.xy - _CentralPosition) * _Scale; //* ((_IntensityValues[vert_id] *  _Scale * 0.9) + _Scale * 0.1);
                    float2 position = _VertexPositions[vert_id];
                    //float2  dir = normalize(position.xy - _CentralPosition);
                    //dir *= ((_IntensityValues[vert_id] * (1.0 - _MinimumScale) * _Scale) + _Scale * _MinimumScale);
                    return position;
                }

                float2x2 rotationMatrix(float angle)
                {
                    angle *= UNITY_PI / 180.0;
                    float sine = sin(angle), cosine = cos(angle);
                    return float2x2(cosine, -sine,
                        sine, cosine);
                }

                // By Inigo Quilez https://www.shadertoy.com/view/WdSGRd
                //float sdPoly(float2 p)
                //{
                //    float d = dot(p - GetVertexPosition(0), p - GetVertexPosition(0));
                //    float s = 1.0;
                //    for (int i = 0, j = 7; i < 8; j = i, i++)
                //    {
                //        // distance
                //        float2 vj = GetVertexPosition(j);
                //        float2 vi = GetVertexPosition(i);
                //        float2 e = vj - vi;
                //        float2 w = p - vi;
                //        float2 b = w - e * clamp(dot(w, e) / dot(e, e), 0.0, 1.0);
                //        d = min(d, dot(b, b));

                //        // winding number from http://geomalgorithms.com/a03-_inclusion.html
                //        bool3 cond = bool3(p.y >= vi.y, p.y<vj.y, e.x* w.y>e.y * w.x);
                //        if (all(cond) || all(!(cond))) s *= -1.0;

                //    }

                //    return s * sqrt(d);
                //}

                 // By Inigo Quilez https://www.shadertoy.com/view/WdSGRd
                float sdPoly(float2 p)
                {
                    float d = dot(p - GetVertexPosition(0), p - GetVertexPosition(0));
                    float s = 1.0;
                    for (int i = 0; i < 8; i++)
                    {
                        // distance
                        int i2 = (i + 1) % 8;

                        float2 p1 = GetVertexPosition(i);
                        float2 p2 = GetVertexPosition(i2);

                        float2 e = p2 - p1;
                        float2 v = p - p1;
                        float2 pq = v - e * clamp(dot(v, e) / dot(e, e), 0.0, 1.0);
                        d = min(d, dot(pq, pq));

                        // winding number from http://geomalgorithms.com/a03-_inclusion.html
                        float2 v2 = p - p2;
                        float val3 = v.x * e.y - e.x * v.y;//isRight
                        bool3 cond = bool3(v.y >= 0.0, v2.y < 0.0, val3>0.0);
                        if (all(cond) || all(!(cond))) s *= -1.0;  // have a valid up or down intersect
                    }

                    return sqrt(d) * s;
                }

                v2f vert(appdata_t v)
                {
                    v2f OUT;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                    OUT.worldPosition = v.vertex;
                    OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                    OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                    OUT.color = v.color * _Color;
                    return OUT;
                }

                fixed4 frag(v2f IN) : SV_Target
                {
                    float2 worldPos = float2(IN.worldPosition.x, 100.0 - IN.worldPosition.y);
                     
                    float len = sdPoly(mul(rotationMatrix(_Rotation) , (_CentralPosition.xy - worldPos) / _SizeScale)) * _DistScale;
                    //len -= _Time.x * _TimeScale;
                    //float freq = 1.0/ _Frequency;
                    //float period = _Frequency;
                    //len = fmod(len, freq) * period;
                    //len = abs(len);

                    //len = lerp(0, smoothstep(0.5 - _PulseWidth, 0.5, 1.0-len), len > 0.5);

                    ////return color;
                    return float4(lerp(_Color * fmod((len + _Time.x * _TimeScale), _Frequency), _Color2, 1.0 - len).xyz, len);
                    return float4(_Color.xyz * len, len);
                    //return _Color;
                }
            ENDCG
            }
        }
}

Shader "Unlit/VotingIntensityBorder"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)

        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255

        _ColorMask("Color Mask", Float) = 15

        _Scale("Scale", Float) = 1
        _MinimumScale("MinimumSize", Float) = 0.1
        _CentralPosition("Central Position", Vector) = (0, 0,0,0)
        _Frequency("Frequency", Float) = 0.7
        _TimeScale("TimeScale", Float) = 5
        _PulseWidth("PulseWidth", Float) = 0.1
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
                fixed4 _TextureSampleAdd;
                float4 _ClipRect;
                float4 _MainTex_ST;
                float _Scale;
                float _MinimumScale;
                float2 _CentralPosition;
                float _Frequency;
                float _TimeScale;
                float _PulseWidth;
                float _IntensityValues[9] = {0.0, 0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0};

                float4 GetVertexPosition(uint vert_id, float4 position) 
                {
                    if (vert_id == 0) return position;
                    //float2  dir = normalize(position.xy - _CentralPosition) * _Scale; //* ((_IntensityValues[vert_id] *  _Scale * 0.9) + _Scale * 0.1);
                    float2  dir = normalize(position.xy - _CentralPosition);
                    dir *= ((_IntensityValues[vert_id] * (1.0 - _MinimumScale) * _Scale) + _Scale * _MinimumScale);
                    return float4(_CentralPosition + dir, position.zw);
                }

                v2f vert(appdata_t v)
                {
                    v2f OUT;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
        
                    //v.vertex = GetVertexPosition(v.id, v.vertex);
                    //v.vertex += float4(100.0, 0.0, 0.0, 0.0);

                    OUT.worldPosition = v.vertex;
                    OUT.worldPosition = GetVertexPosition(v.id, OUT.worldPosition);
                    OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                    //OUT.vertex = (OUT.vertex) + (OUT.vertex) / 2.0;
                    OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                    OUT.color = v.color * _Color;
                    return OUT;
                }

                fixed4 frag(v2f IN) : SV_Target
                {

                    //float len = length(IN.worldPosition.xy - _CentralPosition.xy) / _Scale;
                    //len -= _Time.x * _TimeScale;
                    //float freq = 1.0/ _Frequency;
                    //float period = _Frequency;
                    //len = fmod(len, freq) * period;
                    //len = abs(len);

                    //return color;
                    return _Color;
                }
            ENDCG
            }
        }
}

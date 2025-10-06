Shader "UI/Gradient Ramp"
{
    Properties
    {
        [PerRendererData]_MainTex("Sprite", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        _RampTex("Gradient Ramp", 2D) = "white" {}
        _Angle("Angle (deg)", Range(0,360)) = 90
        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
        _ColorMask("Color Mask", Float) = 15
        _UseUIAlphaClip("Use Alpha Clip", Float) = 0
        _ClipRect("Clip Rect", Vector) = (-32767,-32767,32767,32767)
        _UIMaskSoftnessX("Mask SoftnessX", Float) = 0
        _UIMaskSoftnessY("Mask SoftnessY", Float) = 0
    }

    SubShader
    {
        Tags{ "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "CanUseSpriteAtlas"="True" }
        Stencil{ Ref [_Stencil] Comp [_StencilComp] Pass [_StencilOp] ReadMask [_StencilReadMask] WriteMask [_StencilWriteMask] }
        Cull Off ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "UI-GradientRamp"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            sampler2D _MainTex; float4 _MainTex_ST;
            fixed4 _Color;

            sampler2D _RampTex;
            float _Angle;
            float4 _ClipRect; float _UIMaskSoftnessX; float _UIMaskSoftnessY;

            struct appdata { float4 vertex:POSITION; float4 color:COLOR; float2 uv:TEXCOORD0; };
            struct v2f { float4 pos:SV_POSITION; float2 uv:TEXCOORD0; float2 uv0:TEXCOORD1; fixed4 col:COLOR; float4 world:TEXCOORD2; };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv0 = v.uv;
                o.col = v.color * _Color;
                o.world = v.vertex;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 baseCol = tex2D(_MainTex, i.uv) * i.col;

                float a = radians(_Angle);
                float2 dir = normalize(float2(cos(a), sin(a)));
                float2 uvCentered = i.uv0 - 0.5;
                float t = saturate(dot(uvCentered, dir) + 0.5);

                fixed4 ramp = tex2D(_RampTex, float2(t, 0.5));
                fixed4 col = baseCol * ramp;

                #ifdef UNITY_UI_CLIP_RECT
                float2 ps = 1.0 / float2(_ScreenParams.x, _ScreenParams.y);
                col.a *= UnityGet2DClipping(i.world.xy, _ClipRect);
                col.a *= UnityGetSmoothCorner(i.world.xy, _ClipRect, _UIMaskSoftnessX*ps.x, _UIMaskSoftnessY*ps.y);
                #endif
                #ifdef UNITY_UI_ALPHACLIP
                clip(col.a - 0.001);
                #endif
                return col;
            }
            ENDCG
        }
    }
}

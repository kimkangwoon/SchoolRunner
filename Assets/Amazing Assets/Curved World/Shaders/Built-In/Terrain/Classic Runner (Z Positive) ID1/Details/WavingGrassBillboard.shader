//Shader "Hidden/Amazing Assets/Curved World/Nature/Terrain/Classic Runner (Z Positive) 1/Details/BillboardWavingDoublePass"
Shader "Hidden/TerrainEngine/Details/BillboardWavingDoublePass"
{
    Properties 
    {
        _WavingTint ("Fade Color", Color) = (.7,.6,.5, 0)
        _MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
        _WaveAndDistance ("Wave and distance", Vector) = (12, 3.6, 1, 1)
        _Cutoff ("Cutoff", float) = 0.5
    }

    SubShader 
    {
        Tags { "Queue" = "Geometry+200"
               "IgnoreProjector"="True"
               "RenderType"="CurvedWorld_GrassBillboard"
               "DisableBatching"="True"
             }
        Cull Off
        LOD 200
        ColorMask RGB

        CGPROGRAM
        #pragma surface surf Lambert vertex:WavingGrassBillboardVert addshadow fullforwardshadows exclude_path:deferred



#define CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_Z_POSITIVE
#define CURVEDWORLD_BEND_ID_1
#include "../../../../Core/CurvedWorldTransform.cginc" 

        #include "UnityCG.cginc"
        #include "../../TerrainEngine.cginc"

        sampler2D _MainTex;
        fixed _Cutoff;

        struct Input 
        {
            float2 uv_MainTex;
            fixed4 color : COLOR;
        };

        void surf (Input IN, inout SurfaceOutput o) 
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            clip (o.Alpha - _Cutoff);
            o.Alpha *= IN.color.a;
        }

        ENDCG
    }

    Fallback Off
}

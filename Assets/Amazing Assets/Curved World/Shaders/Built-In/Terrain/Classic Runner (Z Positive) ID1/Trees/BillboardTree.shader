// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Hidden/Amazing Assets/Curved World/Nature/Terrain/Classic Runner (Z Positive) 1/BillboardTree" 
{
    Properties {
        _MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
    }

    SubShader {
        Tags { "Queue" = "Transparent-100" "IgnoreProjector"="True" "RenderType"="CurvedWorld_TreeBillboard" }

        Pass {
            ColorMask rgb
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"
            #include "TerrainEngine.cginc"


#define CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_Z_POSITIVE
#define CURVEDWORLD_BEND_ID_1
#include "../../../../Core/CurvedWorldTransform.cginc" 


            struct v2f {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR0;
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert (appdata_tree_billboard v) {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                TerrainBillboardTree(v.vertex, v.texcoord1.xy, v.texcoord.y);


                CURVEDWORLD_TRANSFORM_VERTEX(v.vertex);


                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv.x = v.texcoord.x;
                o.uv.y = v.texcoord.y > 0;
                o.color = v.color;
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            sampler2D _MainTex;
            fixed4 frag(v2f input) : SV_Target
            {
                fixed4 col = tex2D( _MainTex, input.uv);
                col.rgb *= input.color.rgb;
                clip(col.a);
                UNITY_APPLY_FOG(input.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }

    Fallback Off
}

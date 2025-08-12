Shader "Custom/StencilMask2D"
{
    SubShader
    {
        Tags { 
            "Queue" = "Geometry-100" 
            "RenderType"="Opaque"
            "ForceNoShadowCasting"="True" 
        }
        
        ColorMask 0   // 不写入颜色
        ZWrite Off    // 不写入深度
        Cull Off      // 确保双面渲染

        Stencil {
            Ref 1
            Pass Replace
        }

        Pass  // 必须包含实际绘制操作
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                return 0; // 实际不输出颜色（被ColorMask 0禁用）
            }
            ENDCG
        }
    }
}
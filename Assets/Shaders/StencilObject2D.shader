Shader "Custom/StencilObject2D"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        [Toggle(FLIP_UV)] _FlipUV ("Flip UV", Float) = 0
    }
    SubShader
    {
        Tags { 
            "Queue" = "Transparent" 
            "RenderType"="Transparent" 
            "IgnoreProjector"="True" 
        }
        LOD 100
        
        ZWrite Off
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off // 重要：2D精灵通常需要双面渲染

        Stencil
        {
            Ref 1
            Comp Equal
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature FLIP_UV
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR; // 捕获Sprite Renderer颜色
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color; // 传递顶点颜色
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                
                #ifdef FLIP_UV
                uv.y = 1.0 - uv.y;
                #endif
                
                fixed4 tex = tex2D(_MainTex, uv);
                fixed4 col = tex * _Color * i.color; // 包含Sprite Renderer颜色
                
                // 调试输出
                // if (tex.a < 0.01) discard; // 检查alpha裁剪
                // return fixed4(uv, 0, 1); // UV可视化
                // return i.color; // 顶点颜色可视化
                
                return col;
            }
            ENDCG
        }
    }
}
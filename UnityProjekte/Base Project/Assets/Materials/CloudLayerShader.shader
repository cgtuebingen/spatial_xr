Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        //_Color ("Main Color", Color) = (1,1,1,1)
        _TexList ("Map Parts", 2DArray) = "white" {}
        _ZoomLevel ("Zoom Level", Integer) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            // Blending mode
            Blend SrcAlpha OneMinusSrcAlpha

            // Cull back faces
            Cull Back

            // Depth test
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma require 2darray
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal: NORMAL;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            //sampler2D _MainTex;
            float4 _MainTex_ST;
            //float4 _Color;
            int _ZoomLevel;
            UNITY_DECLARE_TEX2DARRAY(_TexList);
            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(v.normal);
                //float longitude = atan2(normal.z, normal.x);
                //float latitude = asin(normal.y);

                //o.uv.x = (longitude / UNITY_PI) * 0.5 + 0.5;
                //o.uv.y = (latitude / (UNITY_PI * 0.5)) * 0.5 + 0.5;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float longitude = atan2(i.normal.z, i.normal.x);
                float latitude = asin(i.normal.y);

                i.uv.x = (longitude / UNITY_PI) * 0.5 + 0.5;
                i.uv.y = (latitude / (UNITY_PI * 0.5)) * 0.5 + 0.5;
                //fixed4 texColor = tex2D(_MainTex, i.uv);
                float tileSize = pow(2,_ZoomLevel);
                int index = int(i.uv.x*tileSize) + int(tileSize-i.uv.y*tileSize)*tileSize;
                //if(i.uv.y > 0.5) index = 3;
                float2 uv = frac(i.uv * tileSize);
                fixed4 texColor = UNITY_SAMPLE_TEX2DARRAY(_TexList,float3(uv,index));
                //texColor = fixed4(uv,0,1);
                return texColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ZoomLevel ("Zoom Level", Float) = 10
        _ZoomOffset ("Zoom Offset (for the animation)", Float) = 1.0
        _TexList ("Map Parts", 2DArray) = "white"{}
        _OffsetX("OffsetX", Float)= 0.0
        _OffsetY("OffsetY",Float) = 0.0
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ZoomLevel;
            float _ZoomOffset;
            UNITY_DECLARE_TEX2DARRAY(_TexList);
            float _OffsetX;
            float _OffsetY;
            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                float z = sqrt(_ZoomLevel - v.vertex[0]*v.vertex[0]*(1/25.0f) -v.vertex[2]*v.vertex[2]*(1/25.0f)) - sqrt(_ZoomLevel);
                v.vertex = v.vertex + float4(0,z*5,0,0);
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);

                int index = int((i.uv[0]-0.5)*_ZoomOffset + 1.5 + _OffsetX) + 3*int(i.uv[1]*_ZoomOffset+1.0+ _OffsetY);
                float2 uv = frac(i.uv*_ZoomOffset + float2(_OffsetX,_OffsetY));
                fixed4 col = UNITY_SAMPLE_TEX2DARRAY(_TexList,float3(uv,index));
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}

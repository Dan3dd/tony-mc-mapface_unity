Shader "Hidden/TonyMcMapface"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            Texture3D<float3> _Lut;
            SamplerState sampler_linear_clamp;

            float3 tony_mc_mapface(float3 stimulus)
            {
                // Apply a non-linear transform that the LUT is encoded with.
                const float3 encoded = stimulus / (stimulus + 1.0);

                // Align the encoded range to texel centers.
                const float LUT_DIMS = 48.0;
                const float3 uv = encoded * ((LUT_DIMS - 1.0) / LUT_DIMS) + 0.5 / LUT_DIMS;

                // Note: for OpenGL, do `uv.y = 1.0 - uv.y`

                return _Lut.SampleLevel(sampler_linear_clamp, uv, 0);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                col.rgb = tony_mc_mapface(col.rgb);
                return col;
            }
            ENDCG
        }
    }
}

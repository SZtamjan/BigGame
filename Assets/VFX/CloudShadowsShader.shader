Shader "Custom/CloudShadows"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EdgeSmoothness ("Edge Smoothness", Range(0, 1)) = 0.5
        _CloudSpeed ("Cloud Speed", Vector) = (0.1, 0.1, 0, 0)
        _CloudDensity ("Cloud Density", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            ColorMask RGB
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _EdgeSmoothness;
            float2 _CloudSpeed;
            float _CloudDensity;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                float2 uv = i.uv + _CloudSpeed * _Time.y;
                float noise = tex2D(_MainTex, uv).r;
                noise = smoothstep(0.5 - _EdgeSmoothness, 0.5 + _EdgeSmoothness, noise);
                noise *= _CloudDensity;
                float alpha = noise; // Białe elementy są nieprzezroczyste, czarne przezroczyste
                return half4(noise, noise, noise, alpha);
            }
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags {"LightMode" = "ShadowCaster"}

            ZWrite On
            ColorMask 0

            HLSLPROGRAM
            #pragma vertex ShadowVert
            #pragma fragment ShadowFrag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 posWorld : TEXCOORD1;
            };

            sampler2D _MainTex;
            float _EdgeSmoothness;
            float2 _CloudSpeed;
            float _CloudDensity;

            Varyings ShadowVert(Attributes v)
            {
                Varyings o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = v.uv;
                o.posWorld = TransformObjectToWorld(v.vertex);
                return o;
            }

            float4 ShadowFrag(Varyings i) : SV_Target
            {
                float2 uv = i.uv + _CloudSpeed * _Time.y;
                float noise = tex2D(_MainTex, uv).r;
                noise = smoothstep(0.5 - _EdgeSmoothness, 0.5 + _EdgeSmoothness, noise);
                noise *= _CloudDensity;
                float alpha = noise;
                clip(alpha - 0.5); // Przezroczyste fragmenty nie rzucają cienia
                return float4(0, 0, 0, 1); // Poprawka: zwracamy float4, aby odpowiadało oczekiwanemu typowi
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}

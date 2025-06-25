Shader "Custom/SkyboxOnly"
{
    Properties
    {
        [NoScaleOffset] _SkyboxCubemap("Skybox Cubemap", Cube) = "white" {}
        _Alpha("Alpha", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent"
            "Queue" = "Transparent+100" // Render after other transparent objects
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Name "SkyboxMask"
            Tags { "LightMode" = "UniversalForward" }

            // Render settings
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest LEqual
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 viewDirWS : TEXCOORD0;
            };

            TEXTURECUBE(_SkyboxCubemap);
            SAMPLER(sampler_SkyboxCubemap);
            float _Alpha;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                
                // Calculate world-space view direction
                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.viewDirWS = GetWorldSpaceNormalizeViewDir(positionWS);
                
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Sample the skybox cubemap using view direction
                half4 skyboxColor = SAMPLE_TEXTURECUBE(_SkyboxCubemap, sampler_SkyboxCubemap, -IN.viewDirWS);
                return half4(skyboxColor.rgb, _Alpha);
            }
            ENDHLSL
        }
    }
}
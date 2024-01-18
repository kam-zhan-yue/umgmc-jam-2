Shader "Custom/BurningTreeShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _BurnColor ("Burn Color", Color) = (1, 0.5, 0, 1)
        _BurnSpeed ("Burn Speed", Range(0.1, 2)) = 0.5
    }
    
    CGINCLUDE
    #include "UnityCG.cginc"
    ENDCG

    SubShader
    {
        Tags { "Queue" = "Overlay" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        fixed4 _BurnColor;
        float _BurnSpeed;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            // Sample the main texture
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            // Calculate the burning effect based on time
            float burn = sin(_Time.y * _BurnSpeed) * 0.5 + 0.5;

            // Apply the burning effect to the color
            c.rgb = lerp(c.rgb, _BurnColor.rgb, burn);

            // Assign the final color to the output
            o.Albedo = c.rgb;
        }
        ENDCG
    }

    FallBack "Diffuse"
}

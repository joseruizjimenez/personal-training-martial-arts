//------------------------------------------------------------------------------
// <copyright file="KinectColorVisualizer.fx" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

sampler sprite : register(s0);

//--------------------------------------------------------------------------------------
// Pixel Shader
// 
// Convert from BGRX to RGBX
//--------------------------------------------------------------------------------------
float4 BGR2RGB(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 tex = tex2D(sprite, texCoord);
    return tex.bgra;
}

technique KinectColor
{
    pass KinectColor
    {
        PixelShader = compile ps_2_0 BGR2RGB();
    }
}

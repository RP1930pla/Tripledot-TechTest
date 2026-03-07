#ifndef BOX_BLUR_INCLUDED
    #define BOX_BLUR_INCLUDED

    float InterleavedGradientNoiseFunc(half2 uv, half offset)
    {
        uv += (frac(_Time.y * 100 + offset)) * (half2(47, 17) * 0.695f);
        half3 magic = half3(0.06711056f, 0.00583715f, 52.9829189f);
        return frac(magic.z * frac(dot(uv, magic.xy)));
    }

    float2 Random2DFrom2D(float2 p)
    {
        float3 p3 = frac(float3(p.xyx) * float3(0.1031, 0.1030, 0.0973));
        
        p3 += dot(p3, p3.yzx + 33.33);
        return frac((p3.xx + p3.yz) * p3.zy);
    }

    #define diff3(f, x, y) (((f((x) + (y))) + (f((x) - (y))) - 2.*(f((x)))) / dot(y,y))

    float hash21(float2 p)
    {
        float3 q = frac(float3(p.xyx) * .1031);
        q += dot(q, q.yzx + 19.19);
        return frac((q.x + q.y) * q.z);
    }

    float bluenoise(float2 p)
    {
        return 1. / 4. * (
            diff3(hash21, p, float2(1, 0))
        + diff3(hash21, p, float2(0, 1))
        ) + .5;
    }

    void BoxBlur_half(in Texture2D MainTex, in SamplerState sampler_MainTex, in float2 uv, in float2 texelSize, in float radius, out float4 result)
    {
        result = half4(0,0,0,0);
        float diameter = (float(radius) * 2.0) + 1.0;  
        // float numberOfSamples = diameter * diameter;
        float numberOfSamples = 0;
        float2 res = uv * (1/texelSize);

        //STOCHASTIC SAMPLING TESTS
        // float blueNoise = bluenoise(res + frac(_Time.y * 10));
        // float2 randomNoise = Random2DFrom2D(half2(blueNoise, 1-blueNoise));
        // // randomNoise = (randomNoise - 0.5)*2;
        // randomNoise = 0;

        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                half2 pixelOffset = half2(x,y);
                half distanceToPixel = length(pixelOffset);

                if(distanceToPixel > float(radius))
                {
                    continue;
                }

                half2 uvOffset = pixelOffset * texelSize;

                result += SAMPLE_TEXTURE2D(MainTex, sampler_MainTex, uv + uvOffset);
                numberOfSamples++;

            }
        }

        result /= numberOfSamples;

        // result = half4(randomNoise.xy, 0,1);

    }

    void BoxBlur_float(in Texture2D MainTex, in SamplerState sampler_MainTex, in float2 uv, in float2 texelSize, in float radius, out float4 result)
    {
        BoxBlur_half(MainTex, sampler_MainTex, uv, texelSize, radius, result);
    }
#endif
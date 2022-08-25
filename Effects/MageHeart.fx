sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;
float2 uOffset;
float uScale;
float2 uTargetPosition;
float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;
/*float2 uMin;
float2 uMax;*/

float4 MageHeart(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0 {
	float4 color = tex2D(uImage0, coords);
	//float3 color2 = tex2D(uImage1, coords).rgb * uColor;
	const float pi = 3.14159;
	float time = uTime + uSaturation;
	float time1 = time % pi;
	float time2 = trunc((time / pi) % 3);
	float3 colorMult = float3(0, 0, 0);
	if (time2 == 0) {
		colorMult.r = sin(time1) + 0.5;
	} else if (time2 == 1) {
		colorMult.g = sin(time1) + 0.5;
	}else if (time2 == 2) {
		colorMult.b = sin(time1) + 0.5;
	}
	float3 color2 = tex2D(uImage1, coords).rgb * colorMult;
	float value = pow(max(max(color2.r, color2.g), color2.b), 8);
	return color + float4(value, 0, value * 0.1, 0);
}

technique Technique1 {
	pass MageHeart {
		PixelShader = compile ps_2_0 MageHeart();
	}
}
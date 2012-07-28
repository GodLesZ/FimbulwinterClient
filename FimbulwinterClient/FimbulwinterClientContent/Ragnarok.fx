// Matrices
float4x4 World;
float4x4 View;
float4x4 Projection;

// Lights
float3 AmbientColor;
float AmbientIntensity = 0.5F;

// Material
float3 LightPosition;
float3 DiffuseColor;

// Samplers
texture Texture;
texture ShadowLightmap;
texture ColorLightmap;
sampler TextureSampler = sampler_state
{
	Texture = <Texture>;
	MagFilter = LINEAR;
	MinFilter = LINEAR;
	MipFilter = LINEAR;
	AddressU = mirror;
	AddressV = mirror;
};

sampler ShadowSampler = sampler_state
{
	Texture = <ShadowLightmap>;
	MagFilter = LINEAR;
	MinFilter = LINEAR;
	MipFilter = LINEAR;
	AddressU = WRAP;
	AddressV = WRAP;
};

sampler ColorSampler = sampler_state
{
	Texture = <ColorLightmap>;
	MagFilter = LINEAR;
	MinFilter = LINEAR;
	MipFilter = LINEAR;
	AddressU = WRAP;
	AddressV = WRAP;
};

// Map Ground Shader
struct MapGroundInput
{
	float4	Position	: POSITION0;
	float3	Normal		: NORMAL0;
	float2	Texture		: TEXCOORD0;
	float2	Lightmap	: TEXCOORD1;
};

struct MapGroundOutput
{
	float4	Position	: POSITION0;
	float3	Normal		: NORMAL0;
	float2	Texture		: TEXCOORD0;
	float2	Lightmap	: TEXCOORD1;
	float4  PositionOrig: TEXCOORD2;
};

MapGroundOutput MapGroundVS(MapGroundInput Input)
{
    MapGroundOutput Output;
	
	float4 pos_ws = mul(Input.Position, World);
	float4 pos_vs = mul(pos_ws, View);
	float4 pos_ps = mul(pos_vs, Projection);
	
	Output.Position			= pos_ps;
	Output.Normal			= normalize(mul(Input.Normal, World));
	Output.Texture			= Input.Texture;
	Output.Lightmap			= Input.Lightmap;
	Output.PositionOrig		= Input.Position;

    return Output;
}

float4 MapGroundPS(MapGroundOutput Input) : COLOR0
{
	float4 color;
	float4 totalLightDiffuse = float4(0, 0, 0, 1);

	float3 lightDir = normalize(float4(LightPosition, 1) - Input.PositionOrig);
	totalLightDiffuse.rgb += DiffuseColor * max(0, dot(Input.Normal, lightDir));
	totalLightDiffuse.a = 1.0F;

	color = tex2D(TextureSampler, Input.Texture);
	color.rgb *= tex2D(ShadowSampler, Input.Lightmap).a;
	color.rgb += tex2D(ColorSampler, Input.Lightmap).rgb;
	
	return color * (totalLightDiffuse + float4(AmbientColor, 1));
}

technique MapGround
{
    pass Main
    {
		CullMode = None;
		ZFunc = Less;

        VertexShader = compile vs_3_0 MapGroundVS();
        PixelShader = compile ps_3_0 MapGroundPS();
    }
}


// Water Shader
struct WaterInput
{
	float4	Position	: POSITION0;
	float3	Normal		: NORMAL0;
	float2	Texture	: TEXCOORD0;
};

struct WaterOutput
{
	float4	Position	: POSITION;
	float3	Normal		: NORMAL0;
	float2	Texture	: TEXCOORD0;
	float4	PositionOrig: TEXCOORD1;
};

WaterOutput WaterVS(WaterInput Input)
{
    WaterOutput Output;
	
	float4 pos_ws = mul(Input.Position, World);
	float4 pos_vs = mul(pos_ws, View);
	float4 pos_ps = mul(pos_vs, Projection);
	
	Output.Position		= pos_ps;
	Output.Normal		= Input.Normal;
	Output.Texture		= Input.Texture;
	Output.PositionOrig	= Input.Position;

    return Output;
}

float4 WaterPS(WaterOutput Input) : COLOR0
{
	float4 color;
	float4 totalLightDiffuse = float4(0, 0, 0, 1);

	float3 lightDir = normalize(float4(LightPosition, 1) - Input.PositionOrig);
	totalLightDiffuse.rgb += DiffuseColor * max(0, dot(Input.Normal, lightDir));
	totalLightDiffuse.a = 1.0F;

	color = tex2D(TextureSampler, Input.Texture) * (totalLightDiffuse + float4((AmbientColor * AmbientIntensity), 1));

	return float4(color.rgb, 0.5);
}

technique Water
{
    pass Pass0
    {
		CullMode = None;
		ZFunc = Less;

		AlphaBlendEnable = True;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;

        VertexShader = compile vs_3_0 WaterVS();
        PixelShader = compile ps_3_0 WaterPS();
    }
}
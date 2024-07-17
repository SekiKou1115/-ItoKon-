// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "LowPolyPackage/WaterFx"
{
	Properties
	{
		_WaveTexture("WaveTexture", 2D) = "white" {}
		_Speed("Speed", Range( 0 , 5)) = 0
		_Smoothness("Smoothness", Float) = 0.5
		[HideInInspector]_ScaleNormalTex_0("ScaleNormalTex_0", Float) = 1.9
		_NormalTexture_0("NormalTexture_0", 2D) = "white" {}
		_FoamColor("Foam Color", Color) = (1,1,1,0)
		[HideInInspector]_ScaleNormalTex_1("ScaleNormalTex_1", Float) = 1.9
		_Foam("Foam", 2D) = "white" {}
		_WaveHeight("Wave Height", Range( 0 , 5)) = 0
		_FoamDistortion("Foam Distortion", 2D) = "white" {}
		_WaterColor("Water Color", Color) = (0.2313726,0.4980392,0.3930376,0)
		_FoamDist("Foam Dist", Range( 0 , 1)) = 0.1
		_DepthColor("Depth Color", Color) = (0.3137237,0.6263926,0.8867924,0)
		_TextureSample_1("Texture Sample_1", 2D) = "bump" {}
		_Distortion("Distortion", Float) = 0
		_Depth("Depth", Float) = 0
		_Coastline("Coastline", Float) = 0
		_FlatRange("FlatRange", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf StandardSpecular alpha:fade keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform sampler2D _WaveTexture;
		uniform float _Speed;
		uniform float _WaveHeight;
		uniform float _ScaleNormalTex_0;
		uniform sampler2D _NormalTexture_0;
		uniform float _ScaleNormalTex_1;
		uniform sampler2D _TextureSample_1;
		uniform float _FlatRange;
		uniform float4 _WaterColor;
		uniform float4 _DepthColor;
		uniform sampler2D _CameraDepthTexture;
		uniform float _Depth;
		uniform float _Coastline;
		uniform sampler2D _GrabTexture;
		uniform float _Distortion;
		uniform float4 _FoamColor;
		uniform sampler2D _Foam;
		uniform sampler2D _FoamDistortion;
		uniform float _FoamDist;
		uniform float _Smoothness;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float4 WaveSpeed425 = ( _Time * _Speed );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float2 uv_TexCoord413 = v.texcoord.xy + ( WaveSpeed425 + (ase_vertex3Pos).y ).xy;
			float3 ase_vertexNormal = v.normal.xyz;
			float3 myVarName0420 = ( ( tex2Dlod( _WaveTexture, float4( uv_TexCoord413, 0, 1.0) ).r - 0.5 ) * ( ase_vertexNormal * _WaveHeight ) );
			v.vertex.xyz += myVarName0420;
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 normalizeResult406 = normalize( ( cross( ddx( ase_worldPos ) , ddy( ase_worldPos ) ) + float3( 1E+13,0,0 ) ) );
			float4 WaveSpeed425 = ( _Time * _Speed );
			float2 uv_TexCoord320 = i.uv_texcoord * float2( 0,0 );
			float2 panner323 = ( WaveSpeed425.x * float2( 0.02,0.02 ) + uv_TexCoord320);
			float2 uv_TexCoord429 = i.uv_texcoord * ( _FlatRange * float2( 9999,0 ) );
			float2 panner322 = ( WaveSpeed425.x * float2( 0.02,0.02 ) + uv_TexCoord429);
			float3 temp_output_330_0 = BlendNormals( UnpackScaleNormal( tex2D( _NormalTexture_0, panner323 ) ,_ScaleNormalTex_0 ) , UnpackScaleNormal( tex2D( _TextureSample_1, panner322 ) ,_ScaleNormalTex_1 ) );
			o.Normal = ( normalizeResult406 + temp_output_330_0 );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float eyeDepth321 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float temp_output_339_0 = saturate( pow( ( abs( ( eyeDepth321 - ase_screenPos.w ) ) + _Depth ) , _Coastline ) );
			float4 lerpResult342 = lerp( _WaterColor , _DepthColor , temp_output_339_0);
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 appendResult331 = (float2(ase_screenPosNorm.x , ase_screenPosNorm.y));
			float2 panner434 = ( WaveSpeed425.x * float2( 0.02,0.02 ) + i.uv_texcoord);
			float4 screenColor343 = tex2D( _GrabTexture, ( float3( ( appendResult331 / ase_screenPosNorm.w ) ,  0.0 ) + ( _Distortion * BlendNormals( UnpackNormal( tex2D( _NormalTexture_0, panner434 ) ) , UnpackNormal( tex2D( _NormalTexture_0, panner434 ) ) ) * temp_output_330_0 ) ).xy );
			float4 lerpResult346 = lerp( lerpResult342 , screenColor343 , temp_output_339_0);
			o.Albedo = lerpResult346.rgb;
			float2 uv_TexCoord454 = i.uv_texcoord * float2( 10,10 );
			float2 panner443 = ( WaveSpeed425.x * float2( 0.5,0.5 ) + uv_TexCoord454);
			float cos445 = cos( WaveSpeed425.x );
			float sin445 = sin( WaveSpeed425.x );
			float2 rotator445 = mul( panner443 - float2( 0,0 ) , float2x2( cos445 , -sin445 , sin445 , cos445 )) + float2( 0,0 );
			float clampResult442 = clamp( tex2D( _FoamDistortion, rotator445 ).r , 0.0 , 1.0 );
			float screenDepth446 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth446 = abs( ( screenDepth446 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _FoamDist ) );
			float clampResult451 = clamp( ( clampResult442 * distanceDepth446 ) , 0.0 , 1.0 );
			float4 lerpResult452 = lerp( ( _FoamColor * tex2D( _Foam, rotator445 ) ) , float4(0,0,0,0) , clampResult451);
			o.Emission = lerpResult452.rgb;
			o.Specular = lerpResult346.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
1960;29;1728;1007;2133.752;2074.677;3.945389;True;False
Node;AmplifyShaderEditor.CommentaryNode;421;-489.7502,-1864.828;Float;False;914.394;362.5317;Comment;4;425;424;423;422;Wave Speed;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;436;-485.798,-1383.562;Float;False;1170.797;1035.546;Comment;13;344;426;428;429;326;322;433;432;329;430;438;439;459;Blend Normal;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;423;-439.7499,-1617.296;Float;False;Property;_Speed;Speed;1;0;Create;True;0;0;False;0;0;0.07;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;422;-368.8528,-1814.828;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;428;-474.0576,-537.9683;Float;False;Constant;_Tiling2;Tiling2;16;0;Create;True;0;0;False;0;9999,0;999,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;438;-455.5632,-731.7334;Float;False;Property;_FlatRange;FlatRange;17;0;Create;True;0;0;False;0;0;-0.39;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;424;-39.8147,-1679.807;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;426;-404.2505,-896.6425;Float;False;Constant;_Tiling;Tiling;17;0;Create;True;0;0;False;0;0,0;9999,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;408;275.0837,-184.5243;Float;False;1627.834;561.4852;Comment;12;415;416;409;411;410;412;413;414;420;419;418;417;Vertex Animation;1,1,1,1;0;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;319;250.7341,-2039.132;Float;False;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;439;-283.7543,-631.6931;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;458;-1740.705,-313.0813;Float;False;1778.051;796.6082;Comment;15;452;453;451;450;449;455;456;442;446;448;447;445;443;454;444;Foam;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;425;181.6455,-1772.152;Float;False;WaveSpeed;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;435;-141.9615,-1303.796;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;409;294.7784,129.3546;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;429;-139.0285,-656.3979;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenDepthNode;321;450.9382,-2038.486;Float;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;320;-192.5147,-915.0704;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;459;-364.1393,-1083.996;Float;False;425;0;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;454;-1702.416,-17.50262;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;10,10;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;444;-1654.296,118.2658;Float;False;425;0;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;430;132.622,-766.6979;Float;False;Property;_ScaleNormalTex_0;ScaleNormalTex_0;3;1;[HideInInspector];Create;True;0;0;False;0;1.9;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;437;735.3074,-696.634;Float;False;925.819;315.9324;Comment;6;406;351;350;348;349;347;Low Poly Normal;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;344;132.6325,-454.7797;Float;False;Property;_ScaleNormalTex_1;ScaleNormalTex_1;6;1;[HideInInspector];Create;True;0;0;False;0;1.9;0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;324;673.9872,-2034.101;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;323;75.34732,-915.1756;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.02,0.02;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;411;501.987,82.19602;Float;False;False;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;410;512.1093,-42.19803;Float;False;425;0;1;FLOAT4;0
Node;AmplifyShaderEditor.PannerNode;443;-1402.247,29.78431;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;434;115.0383,-1303.795;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.02,0.02;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;322;86.7504,-656.4788;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.02,0.02;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;433;378.4568,-1140.024;Float;True;Property;_TextureSample4;Texture Sample 4;4;0;Create;True;0;0;False;0;None;dd2fd2df93418444c8e280f1d34deeb5;True;0;True;white;Auto;True;Instance;329;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;347;761.2131,-597.1217;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ScreenPosInputsNode;327;963.1959,-1418.948;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;432;376.4096,-1323.76;Float;True;Property;_TextureSample3;Texture Sample 3;4;0;Create;True;0;0;False;0;None;dd2fd2df93418444c8e280f1d34deeb5;True;0;True;white;Auto;True;Instance;329;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;329;378.0934,-944.3842;Float;True;Property;_NormalTexture_0;NormalTexture_0;4;0;Create;True;0;0;False;0;None;4600a73ac188e5944a668147d764c909;True;0;True;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;325;692.4961,-1786.942;Float;False;Property;_Depth;Depth;15;0;Create;True;0;0;False;0;0;0.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;326;373.4293,-715.8843;Float;True;Property;_TextureSample_1;Texture Sample_1;13;0;Create;True;0;0;False;0;dd2fd2df93418444c8e280f1d34deeb5;c01da006a7ac4214aa360209439e62b1;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;445;-1218.108,31.62243;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;412;721.235,-37.1767;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.AbsOpNode;328;814.205,-2033.111;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DdyOpNode;349;984.213,-527.1217;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendNormalsNode;431;851.5706,-1042.949;Float;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;334;1121.906,-1149.669;Float;False;Property;_Distortion;Distortion;14;0;Create;True;0;0;False;0;0;0.16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;331;1192.248,-1387.444;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;413;847.3977,-105.0051;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;332;922.5961,-1939.039;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;448;-1112.615,347.0739;Float;False;Property;_FoamDist;Foam Dist;11;0;Create;True;0;0;False;0;0.1;0.518;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;333;766.58,-1675.944;Float;False;Property;_Coastline;Coastline;16;0;Create;True;0;0;False;0;0;-1.52;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;447;-1003.738,63.31373;Float;True;Property;_FoamDistortion;Foam Distortion;9;0;Create;True;0;0;False;0;None;86ed2da639b7efa41aba3a03ae1d7fcb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DdxOpNode;348;985.213,-596.1217;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendNormalsNode;330;850.2899,-941.9196;Float;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;442;-699.4421,107.5366;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;336;1029.553,-1769.219;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CrossProductOpNode;350;1186.627,-594.394;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DepthFade;446;-812.3898,353.1016;Float;False;True;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;414;1142.624,207.1791;Float;False;Property;_WaveHeight;Wave Height;8;0;Create;True;0;0;False;0;0;0.18;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;335;1362.172,-1022.207;Float;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalVertexDataNode;415;1195.94,52.36869;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;337;1323.214,-1234.366;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;416;1073.055,-133.485;Float;True;Property;_WaveTexture;WaveTexture;0;0;Create;True;0;0;False;0;None;86ed2da639b7efa41aba3a03ae1d7fcb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;455;-972.7987,-136.2693;Float;True;Property;_Foam;Foam;7;0;Create;True;0;0;False;0;None;86ed2da639b7efa41aba3a03ae1d7fcb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;417;1401.062,52.73346;Float;False;2;2;0;FLOAT3;1,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;340;1068.34,-2280.776;Float;False;Property;_WaterColor;Water Color;10;0;Create;True;0;0;False;0;0.2313726,0.4980392,0.3930376,0;0.08343716,0.5214771,0.7075472,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;339;1346.394,-1762.433;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;418;1382.366,-104.9319;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;341;1069.381,-2108.997;Float;False;Property;_DepthColor;Depth Color;12;0;Create;True;0;0;False;0;0.3137237,0.6263926,0.8867924,0;1,0.8726415,0.9904988,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;351;1354.926,-594.1754;Float;False;2;2;0;FLOAT3;1E-09,0,0;False;1;FLOAT3;1E+13,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;456;-577.7838,333.8632;Float;False;2;2;0;FLOAT;0.075;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;338;1512.109,-1139.469;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;449;-735.5335,-253.6362;Float;False;Property;_FoamColor;Foam Color;5;0;Create;True;0;0;False;0;1,1,1,0;0.6415094,0.6415094,0.6415094,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;419;1557.371,-104.4017;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ScreenColorNode;343;1669.795,-1145.556;Float;False;Global;_GrabScreen0;Grab Screen 0;3;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;451;-436.3567,332.0985;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;342;1539.706,-1981.565;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;453;-550.0665,132.1653;Float;False;Constant;_Color5;Color 5;9;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalizeNode;406;1491.068,-593.6337;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;450;-550.7817,17.62178;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;452;-298.3318,19.92667;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;390;1680.266,-751.2142;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;19;2167.61,-559.0261;Float;False;Property;_Smoothness;Smoothness;2;0;Create;True;0;0;False;0;0.5;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;420;1691.729,-109.3796;Float;False;myVarName0;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;346;1951.394,-1335.888;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2387.854,-758.799;Float;False;True;2;Float;ASEMaterialInspector;0;0;StandardSpecular;LowPolyPackage/WaterFx;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;424;0;422;0
WireConnection;424;1;423;0
WireConnection;439;0;438;0
WireConnection;439;1;428;0
WireConnection;425;0;424;0
WireConnection;429;0;439;0
WireConnection;321;0;319;0
WireConnection;320;0;426;0
WireConnection;324;0;321;0
WireConnection;324;1;319;4
WireConnection;323;0;320;0
WireConnection;323;1;459;0
WireConnection;411;0;409;0
WireConnection;443;0;454;0
WireConnection;443;1;444;0
WireConnection;434;0;435;0
WireConnection;434;1;459;0
WireConnection;322;0;429;0
WireConnection;322;1;459;0
WireConnection;433;1;434;0
WireConnection;432;1;434;0
WireConnection;329;1;323;0
WireConnection;329;5;430;0
WireConnection;326;1;322;0
WireConnection;326;5;344;0
WireConnection;445;0;443;0
WireConnection;445;2;444;0
WireConnection;412;0;410;0
WireConnection;412;1;411;0
WireConnection;328;0;324;0
WireConnection;349;0;347;0
WireConnection;431;0;432;0
WireConnection;431;1;433;0
WireConnection;331;0;327;1
WireConnection;331;1;327;2
WireConnection;413;1;412;0
WireConnection;332;0;328;0
WireConnection;332;1;325;0
WireConnection;447;1;445;0
WireConnection;348;0;347;0
WireConnection;330;0;329;0
WireConnection;330;1;326;0
WireConnection;442;0;447;1
WireConnection;336;0;332;0
WireConnection;336;1;333;0
WireConnection;350;0;348;0
WireConnection;350;1;349;0
WireConnection;446;0;448;0
WireConnection;335;0;334;0
WireConnection;335;1;431;0
WireConnection;335;2;330;0
WireConnection;337;0;331;0
WireConnection;337;1;327;4
WireConnection;416;1;413;0
WireConnection;455;1;445;0
WireConnection;417;0;415;0
WireConnection;417;1;414;0
WireConnection;339;0;336;0
WireConnection;418;0;416;1
WireConnection;351;0;350;0
WireConnection;456;0;442;0
WireConnection;456;1;446;0
WireConnection;338;0;337;0
WireConnection;338;1;335;0
WireConnection;419;0;418;0
WireConnection;419;1;417;0
WireConnection;343;0;338;0
WireConnection;451;0;456;0
WireConnection;342;0;340;0
WireConnection;342;1;341;0
WireConnection;342;2;339;0
WireConnection;406;0;351;0
WireConnection;450;0;449;0
WireConnection;450;1;455;0
WireConnection;452;0;450;0
WireConnection;452;1;453;0
WireConnection;452;2;451;0
WireConnection;390;0;406;0
WireConnection;390;1;330;0
WireConnection;420;0;419;0
WireConnection;346;0;342;0
WireConnection;346;1;343;0
WireConnection;346;2;339;0
WireConnection;0;0;346;0
WireConnection;0;1;390;0
WireConnection;0;2;452;0
WireConnection;0;3;346;0
WireConnection;0;4;19;0
WireConnection;0;11;420;0
ASEEND*/
//CHKSM=BE3FF51E1119D36E393186AFE4251CB9817727A5
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Texgen_CubeMap" {
	Properties{


		_Reflectivity("Reflectivity", Range(0,1)) = 0.5 // 反射强度


		_MainTex("Base", 2D) = "white"// 主材质


		_Environment("Environment", Cube) = "white" // 立方环境贴图


	}


		SubShader{


		Pass{


		CGPROGRAM


#pragma vertex vert


#pragma fragment frag


#include "UnityCG.cginc"


		sampler2D _MainTex;


	samplerCUBE _Environment;


	float4 _MainTex_ST;


	float _Reflectivity;


	struct v2f {


		float4  pos : SV_POSITION;


		float2  uv : TEXCOORD0;


		float3  R:TEXCOORD1;


	};


	float3 reflect(float3 I,float3 N)


	{


		return I - 2.0*N *dot(N,I);


	}


	v2f vert(appdata_base v)


	{


		v2f o;


		o.pos = UnityObjectToClipPos(v.vertex);


		o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);


		// 将顶点转换到世界坐标


		float3 posW = mul(unity_ObjectToWorld,v.vertex).xyz;


		// 世界空间摄像机射向该顶点的向量


		float3 I = posW - _WorldSpaceCameraPos.xyz;


		// 法线转换到世界空间


		float3 N = mul((float3x3)unity_ObjectToWorld,v.normal);


		N = normalize(N);


		// 通过摄像机射线和法线求反射向量


		o.R = reflect(I,N);


		return o;


	}


	float4 frag(v2f i) : COLOR


	{


		// 通过反射向量，使用texCUBE进行纹理映射获取颜色值


		float4 reflectiveColor = texCUBE(_Environment,i.R);


		float4 decalColor = tex2D(_MainTex,i.uv);


		//float4 outp = lerp(decalColor,reflectiveColor,_Reflectivity);


		return decalColor + reflectiveColor * _Reflectivity;


	}


		ENDCG


	}


	}


}

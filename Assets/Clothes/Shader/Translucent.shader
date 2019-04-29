Shader "Cloth/TranslucentCloth"
{
	Properties
	{
		_MainTex("Main Tex",2D) = "white"{}
		_Specular("Specular", Color) = (1, 1, 1, 1)  //反射光颜色
		_Ambient("Ambient Light",Color) = (1, 1, 1, 1) //为了方便，从这里调环境光
		_Light("Direction Light",Color) = (1, 1, 1, 1)//为了方便，没有使用动态光照，从这儿调光照的颜色
		_Gloss("Gloss",Range(8.0 ,256.0)) = 20
		_Detail("Detal Tex",2D) = "white"{}
		_DetailBump("Detal Bump Tex",2D) = "white"{}
	}

	SubShader
	{
		Pass
		{
			Tags{ "IgnoreProject" = "True" }

			ZWrite On
			ZTest LEqual

			CGPROGRAM

			#pragma vertex vert  
			#pragma fragment frag  

			#include "UnityCG.cginc"  

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Specular;
			fixed4 _Light;
			fixed4 _Ambient;
			float _Gloss;

			sampler2D _Detail;
			sampler2D _DetailBump;
			float4 _DetailBump_ST;

			struct a2v
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float4 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 texcoord:TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
			};

			v2f vert(a2v v)
			{
				v2f o;
				// Transform the vertex from object space to projection space  
				//把局部顶点光照投射到空间  
				o.pos = UnityObjectToClipPos(v.vertex);

				o.texcoord.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.texcoord.zw = TRANSFORM_TEX(v.texcoord, _DetailBump);

				//Transform the normal from object  space to world  space;  
				//把局部法线转换到空间法线  
				o.worldNormal = mul(v.normal, unity_WorldToObject);

				//Transform the vertex from obeject space to world space;  
				//把局部顶点光照转换到空间中  
				o.worldPos = mul(unity_ObjectToWorld , v.vertex).xyz;

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{

				fixed3 detail = UnpackNormal(tex2D(_DetailBump,i.texcoord.zw)).xyz;
				fixed3 worldNormal = normalize(i.worldNormal + detail);

				half3 worldLight = float3(0,0,-30);//为了方便，没有使用动态光照，这儿固定的光照方向

				fixed3 worldLightDir = normalize(worldLight);


				//Coumpute diffuse term
				fixed4 base = tex2D(_MainTex,i.texcoord.xy) * tex2D(_Detail,i.texcoord.xy) * 2;
				fixed3 diffuse = _Light.rgb * base.rgb * saturate(dot(worldNormal,worldLightDir));

				//Get the reflect direction in world space;  
				//获取空间中的光照方向  
				fixed3 reflectDir = normalize(reflect(-worldLightDir,worldNormal));


				//Get view direction in world space;  
				//获得视窗的光照方向  
				fixed3 viewDir = normalize(worldLight - i.worldPos.xyz);

				//Get the half direction in world space  
				fixed3 halfDir = normalize(worldLightDir + viewDir);

				//Compute sepcular term  
				fixed3 specular = _Light.rgb * _Specular.rgb * pow(saturate(dot(reflectDir,halfDir)),_Gloss);


				return fixed4(_Ambient*base.rgb + diffuse + specular, 1.0);
			}
			ENDCG
		}
	}
}
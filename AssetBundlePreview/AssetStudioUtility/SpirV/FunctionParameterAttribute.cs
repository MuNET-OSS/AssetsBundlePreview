namespace SpirV;

public enum FunctionParameterAttribute : uint
{
	Zext,
	Sext,
	ByVal,
	Sret,
	NoAlias,
	NoCapture,
	NoWrite,
	NoReadWrite
}

namespace SpirV;

public enum StorageClass : uint
{
	UniformConstant,
	Input,
	Uniform,
	Output,
	Workgroup,
	CrossWorkgroup,
	Private,
	Function,
	Generic,
	PushConstant,
	AtomicCounter,
	Image,
	StorageBuffer
}

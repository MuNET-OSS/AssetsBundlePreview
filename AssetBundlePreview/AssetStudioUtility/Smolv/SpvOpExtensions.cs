namespace Smolv;

public static class SpvOpExtensions
{
	public static bool OpHasResult(this SpvOp _this)
	{
		if (_this < SpvOp.Nop || _this >= SpvOp.KnownOpsCount)
		{
			return false;
		}
		return OpData.SpirvOpData[(int)_this].hasResult != 0;
	}

	public static bool OpHasType(this SpvOp _this)
	{
		if (_this < SpvOp.Nop || _this >= SpvOp.KnownOpsCount)
		{
			return false;
		}
		return OpData.SpirvOpData[(int)_this].hasType != 0;
	}

	public static int OpDeltaFromResult(this SpvOp _this)
	{
		if (_this < SpvOp.Nop || _this >= SpvOp.KnownOpsCount)
		{
			return 0;
		}
		return OpData.SpirvOpData[(int)_this].deltaFromResult;
	}

	public static bool OpVarRest(this SpvOp _this)
	{
		if (_this < SpvOp.Nop || _this >= SpvOp.KnownOpsCount)
		{
			return false;
		}
		return OpData.SpirvOpData[(int)_this].varrest != 0;
	}

	public static bool OpDebugInfo(this SpvOp _this)
	{
		if (_this != SpvOp.SourceContinued && _this != SpvOp.Source && _this != SpvOp.SourceExtension && _this != SpvOp.Name && _this != SpvOp.MemberName && _this != SpvOp.String && _this != SpvOp.Line && _this != SpvOp.NoLine)
		{
			return _this == SpvOp.ModuleProcessed;
		}
		return true;
	}
}

using System.Text;

namespace SpirV;

public class MatrixType : Type
{
	public VectorType ColumnType { get; }

	public int ColumnCount { get; }

	public int RowCount => ColumnType.ComponentCount;

	public MatrixType(VectorType vectorType, int columnCount)
	{
		ColumnType = vectorType;
		ColumnCount = columnCount;
	}

	public override string ToString()
	{
		return $"{ColumnType}x{ColumnCount}";
	}

	public override StringBuilder ToString(StringBuilder sb)
	{
		return sb.Append(ColumnType).Append('x').Append(ColumnCount);
	}
}

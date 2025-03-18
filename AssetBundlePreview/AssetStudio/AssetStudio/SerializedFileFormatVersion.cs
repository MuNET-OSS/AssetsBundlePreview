namespace AssetStudio;

public enum SerializedFileFormatVersion
{
	Unsupported = 1,
	Unknown_2 = 2,
	Unknown_3 = 3,
	Unknown_5 = 5,
	Unknown_6 = 6,
	Unknown_7 = 7,
	Unknown_8 = 8,
	Unknown_9 = 9,
	Unknown_10 = 10,
	HasScriptTypeIndex = 11,
	Unknown_12 = 12,
	HasTypeTreeHashes = 13,
	Unknown_14 = 14,
	SupportsStrippedObject = 15,
	RefactoredClassId = 16,
	RefactorTypeData = 17,
	RefactorShareableTypeTreeData = 18,
	TypeTreeNodeWithTypeFlags = 19,
	SupportsRefObject = 20,
	StoresTypeDependencies = 21,
	LargeFilesSupport = 22
}

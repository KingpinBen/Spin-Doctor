using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace GameLibrary.Helpers.Serializer
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager resourceMan;
		private static CultureInfo resourceCulture;
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Microsoft.Xna.Framework.Content.Pipeline.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}
		internal static string AssemblyReferenceWrongVersion
		{
			get
			{
				return Resources.ResourceManager.GetString("AssemblyReferenceWrongVersion", Resources.resourceCulture);
			}
		}
		internal static string AudioDataReadFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("AudioDataReadFailed", Resources.resourceCulture);
			}
		}
		internal static string AudioDurationInvalid
		{
			get
			{
				return Resources.ResourceManager.GetString("AudioDurationInvalid", Resources.resourceCulture);
			}
		}
		internal static string AudioFileIsDRMProtected
		{
			get
			{
				return Resources.ResourceManager.GetString("AudioFileIsDRMProtected", Resources.resourceCulture);
			}
		}
		internal static string AudioFileOpenFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("AudioFileOpenFailed", Resources.resourceCulture);
			}
		}
		internal static string AudioFormatConversionFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("AudioFormatConversionFailed", Resources.resourceCulture);
			}
		}
		internal static string AudioFormatIsUnsupported
		{
			get
			{
				return Resources.ResourceManager.GetString("AudioFormatIsUnsupported", Resources.resourceCulture);
			}
		}
		internal static string AudioFormatReadFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("AudioFormatReadFailed", Resources.resourceCulture);
			}
		}
		internal static string AudioProcessorParameterQuality
		{
			get
			{
				return Resources.ResourceManager.GetString("AudioProcessorParameterQuality", Resources.resourceCulture);
			}
		}
		internal static string AudioProcessorParameterQualityDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("AudioProcessorParameterQualityDescription", Resources.resourceCulture);
			}
		}
		internal static string BadBitmapSize
		{
			get
			{
				return Resources.ResourceManager.GetString("BadBitmapSize", Resources.resourceCulture);
			}
		}
		internal static string BadGenericTypeHandler
		{
			get
			{
				return Resources.ResourceManager.GetString("BadGenericTypeHandler", Resources.resourceCulture);
			}
		}
		internal static string BadImporterFileExtension
		{
			get
			{
				return Resources.ResourceManager.GetString("BadImporterFileExtension", Resources.resourceCulture);
			}
		}
		internal static string BadPixelBitmapType
		{
			get
			{
				return Resources.ResourceManager.GetString("BadPixelBitmapType", Resources.resourceCulture);
			}
		}
		internal static string BadTextureType
		{
			get
			{
				return Resources.ResourceManager.GetString("BadTextureType", Resources.resourceCulture);
			}
		}
		internal static string BadTypeDelegate
		{
			get
			{
				return Resources.ResourceManager.GetString("BadTypeDelegate", Resources.resourceCulture);
			}
		}
		internal static string BadTypeNameString
		{
			get
			{
				return Resources.ResourceManager.GetString("BadTypeNameString", Resources.resourceCulture);
			}
		}
		internal static string BadTypeOpenGeneric
		{
			get
			{
				return Resources.ResourceManager.GetString("BadTypeOpenGeneric", Resources.resourceCulture);
			}
		}
		internal static string BadTypePointerOrReference
		{
			get
			{
				return Resources.ResourceManager.GetString("BadTypePointerOrReference", Resources.resourceCulture);
			}
		}
		internal static string BadVertexBufferType
		{
			get
			{
				return Resources.ResourceManager.GetString("BadVertexBufferType", Resources.resourceCulture);
			}
		}
		internal static string BuildAndLoadAssetNotIntermediate
		{
			get
			{
				return Resources.ResourceManager.GetString("BuildAndLoadAssetNotIntermediate", Resources.resourceCulture);
			}
		}
		internal static string BuildCancelled
		{
			get
			{
				return Resources.ResourceManager.GetString("BuildCancelled", Resources.resourceCulture);
			}
		}
		internal static string BuildLogAllUpToDate
		{
			get
			{
				return Resources.ResourceManager.GetString("BuildLogAllUpToDate", Resources.resourceCulture);
			}
		}
		internal static string BuildLogAlreadyUpToDate
		{
			get
			{
				return Resources.ResourceManager.GetString("BuildLogAlreadyUpToDate", Resources.resourceCulture);
			}
		}
		internal static string BuildLogBuilding
		{
			get
			{
				return Resources.ResourceManager.GetString("BuildLogBuilding", Resources.resourceCulture);
			}
		}
		internal static string BuildLogCompiling
		{
			get
			{
				return Resources.ResourceManager.GetString("BuildLogCompiling", Resources.resourceCulture);
			}
		}
		internal static string BuildLogDeserializing
		{
			get
			{
				return Resources.ResourceManager.GetString("BuildLogDeserializing", Resources.resourceCulture);
			}
		}
		internal static string BuildLogImporting
		{
			get
			{
				return Resources.ResourceManager.GetString("BuildLogImporting", Resources.resourceCulture);
			}
		}
		internal static string BuildLogProcessing
		{
			get
			{
				return Resources.ResourceManager.GetString("BuildLogProcessing", Resources.resourceCulture);
			}
		}
		internal static string BuildLogSerializing
		{
			get
			{
				return Resources.ResourceManager.GetString("BuildLogSerializing", Resources.resourceCulture);
			}
		}
		internal static string CantCreateDirectory
		{
			get
			{
				return Resources.ResourceManager.GetString("CantCreateDirectory", Resources.resourceCulture);
			}
		}
		internal static string CantDeduceImporterConflict
		{
			get
			{
				return Resources.ResourceManager.GetString("CantDeduceImporterConflict", Resources.resourceCulture);
			}
		}
		internal static string CantDeduceImporterNoneAvailable
		{
			get
			{
				return Resources.ResourceManager.GetString("CantDeduceImporterNoneAvailable", Resources.resourceCulture);
			}
		}
		internal static string CantFindImporter
		{
			get
			{
				return Resources.ResourceManager.GetString("CantFindImporter", Resources.resourceCulture);
			}
		}
		internal static string CantFindProcessor
		{
			get
			{
				return Resources.ResourceManager.GetString("CantFindProcessor", Resources.resourceCulture);
			}
		}
		internal static string CantFindType
		{
			get
			{
				return Resources.ResourceManager.GetString("CantFindType", Resources.resourceCulture);
			}
		}
		internal static string CantLoadPipelineAssembly
		{
			get
			{
				return Resources.ResourceManager.GetString("CantLoadPipelineAssembly", Resources.resourceCulture);
			}
		}
		internal static string CantLoadPipelineAssemblyOffShare
		{
			get
			{
				return Resources.ResourceManager.GetString("CantLoadPipelineAssemblyOffShare", Resources.resourceCulture);
			}
		}
		internal static string CantNormalizeWeights
		{
			get
			{
				return Resources.ResourceManager.GetString("CantNormalizeWeights", Resources.resourceCulture);
			}
		}
		internal static string CantResizeMipmapChainCollection
		{
			get
			{
				return Resources.ResourceManager.GetString("CantResizeMipmapChainCollection", Resources.resourceCulture);
			}
		}
		internal static string CantSerializeMember
		{
			get
			{
				return Resources.ResourceManager.GetString("CantSerializeMember", Resources.resourceCulture);
			}
		}
		internal static string CantSerializeMultidimensionalArrays
		{
			get
			{
				return Resources.ResourceManager.GetString("CantSerializeMultidimensionalArrays", Resources.resourceCulture);
			}
		}
		internal static string CantSerializeReadOnlyNull
		{
			get
			{
				return Resources.ResourceManager.GetString("CantSerializeReadOnlyNull", Resources.resourceCulture);
			}
		}
		internal static string CantSetValueOnProcessor
		{
			get
			{
				return Resources.ResourceManager.GetString("CantSetValueOnProcessor", Resources.resourceCulture);
			}
		}
		internal static string CantWriteDynamicTypesInFlattenContentMode
		{
			get
			{
				return Resources.ResourceManager.GetString("CantWriteDynamicTypesInFlattenContentMode", Resources.resourceCulture);
			}
		}
		internal static string CantWriteNullInFlattenContentMode
		{
			get
			{
				return Resources.ResourceManager.GetString("CantWriteNullInFlattenContentMode", Resources.resourceCulture);
			}
		}
		internal static string CharacterRegionStartGreaterThanEnd
		{
			get
			{
				return Resources.ResourceManager.GetString("CharacterRegionStartGreaterThanEnd", Resources.resourceCulture);
			}
		}
		internal static string ChildAlreadyHasParent
		{
			get
			{
				return Resources.ResourceManager.GetString("ChildAlreadyHasParent", Resources.resourceCulture);
			}
		}
		internal static string CleaningContent
		{
			get
			{
				return Resources.ResourceManager.GetString("CleaningContent", Resources.resourceCulture);
			}
		}
		internal static string CollectionArgumentContainsNull
		{
			get
			{
				return Resources.ResourceManager.GetString("CollectionArgumentContainsNull", Resources.resourceCulture);
			}
		}
		internal static string CompiledAssetFilenameConflict
		{
			get
			{
				return Resources.ResourceManager.GetString("CompiledAssetFilenameConflict", Resources.resourceCulture);
			}
		}
		internal static string CompressionError
		{
			get
			{
				return Resources.ResourceManager.GetString("CompressionError", Resources.resourceCulture);
			}
		}
		internal static string ContentPipelineException
		{
			get
			{
				return Resources.ResourceManager.GetString("ContentPipelineException", Resources.resourceCulture);
			}
		}
		internal static string ContentPipelineInnerException
		{
			get
			{
				return Resources.ResourceManager.GetString("ContentPipelineInnerException", Resources.resourceCulture);
			}
		}
		internal static string ConvertBitmapException
		{
			get
			{
				return Resources.ResourceManager.GetString("ConvertBitmapException", Resources.resourceCulture);
			}
		}
		internal static string ConvertBitmapTypeNotBitmapContent
		{
			get
			{
				return Resources.ResourceManager.GetString("ConvertBitmapTypeNotBitmapContent", Resources.resourceCulture);
			}
		}
		internal static string ConvertBitmapTypeNotConstructable
		{
			get
			{
				return Resources.ResourceManager.GetString("ConvertBitmapTypeNotConstructable", Resources.resourceCulture);
			}
		}
		internal static string ConvertWeightsOutputAlreadyExists
		{
			get
			{
				return Resources.ResourceManager.GetString("ConvertWeightsOutputAlreadyExists", Resources.resourceCulture);
			}
		}
		internal static string CreateVertexBufferBadType
		{
			get
			{
				return Resources.ResourceManager.GetString("CreateVertexBufferBadType", Resources.resourceCulture);
			}
		}
		internal static string CreateVertexBufferBadUsage
		{
			get
			{
				return Resources.ResourceManager.GetString("CreateVertexBufferBadUsage", Resources.resourceCulture);
			}
		}
		internal static string DeserializerConstructedNewInstance
		{
			get
			{
				return Resources.ResourceManager.GetString("DeserializerConstructedNewInstance", Resources.resourceCulture);
			}
		}
		internal static string DeserializerReturnedNull
		{
			get
			{
				return Resources.ResourceManager.GetString("DeserializerReturnedNull", Resources.resourceCulture);
			}
		}
		internal static string DuplicateBoneName
		{
			get
			{
				return Resources.ResourceManager.GetString("DuplicateBoneName", Resources.resourceCulture);
			}
		}
		internal static string DuplicateID
		{
			get
			{
				return Resources.ResourceManager.GetString("DuplicateID", Resources.resourceCulture);
			}
		}
		internal static string DuplicateImporter
		{
			get
			{
				return Resources.ResourceManager.GetString("DuplicateImporter", Resources.resourceCulture);
			}
		}
		internal static string DuplicateProcessor
		{
			get
			{
				return Resources.ResourceManager.GetString("DuplicateProcessor", Resources.resourceCulture);
			}
		}
		internal static string DuplicateSkeleton
		{
			get
			{
				return Resources.ResourceManager.GetString("DuplicateSkeleton", Resources.resourceCulture);
			}
		}
		internal static string DuplicateTypeHandler
		{
			get
			{
				return Resources.ResourceManager.GetString("DuplicateTypeHandler", Resources.resourceCulture);
			}
		}
		internal static string DuplicateVertexChannel
		{
			get
			{
				return Resources.ResourceManager.GetString("DuplicateVertexChannel", Resources.resourceCulture);
			}
		}
		internal static string DuplicateVertexElement
		{
			get
			{
				return Resources.ResourceManager.GetString("DuplicateVertexElement", Resources.resourceCulture);
			}
		}
		internal static string DuplicateXmlTypeName
		{
			get
			{
				return Resources.ResourceManager.GetString("DuplicateXmlTypeName", Resources.resourceCulture);
			}
		}
		internal static string EffectPostProcessorInvalidEffectFormat
		{
			get
			{
				return Resources.ResourceManager.GetString("EffectPostProcessorInvalidEffectFormat", Resources.resourceCulture);
			}
		}
		internal static string EffectPostProcessorInvalidSamplerIndex
		{
			get
			{
				return Resources.ResourceManager.GetString("EffectPostProcessorInvalidSamplerIndex", Resources.resourceCulture);
			}
		}
		internal static string EffectPostProcessorInvalidStateValue
		{
			get
			{
				return Resources.ResourceManager.GetString("EffectPostProcessorInvalidStateValue", Resources.resourceCulture);
			}
		}
		internal static string EffectPostProcessorObsoleteStateUsed
		{
			get
			{
				return Resources.ResourceManager.GetString("EffectPostProcessorObsoleteStateUsed", Resources.resourceCulture);
			}
		}
		internal static string EffectPostProcessorUnknownEffectState
		{
			get
			{
				return Resources.ResourceManager.GetString("EffectPostProcessorUnknownEffectState", Resources.resourceCulture);
			}
		}
		internal static string EffectProcessorDisplayName
		{
			get
			{
				return Resources.ResourceManager.GetString("EffectProcessorDisplayName", Resources.resourceCulture);
			}
		}
		internal static string EffectProcessorErrorCompilingEffect
		{
			get
			{
				return Resources.ResourceManager.GetString("EffectProcessorErrorCompilingEffect", Resources.resourceCulture);
			}
		}
		internal static string EffectProcessorParameterDebugMode
		{
			get
			{
				return Resources.ResourceManager.GetString("EffectProcessorParameterDebugMode", Resources.resourceCulture);
			}
		}
		internal static string EffectProcessorParameterDebugModeDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("EffectProcessorParameterDebugModeDescription", Resources.resourceCulture);
			}
		}
		internal static string EffectProcessorParameterDefineProperty
		{
			get
			{
				return Resources.ResourceManager.GetString("EffectProcessorParameterDefineProperty", Resources.resourceCulture);
			}
		}
		internal static string EffectProcessorParameterDefinePropertyDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("EffectProcessorParameterDefinePropertyDescription", Resources.resourceCulture);
			}
		}
		internal static string EffectProcessorWarningCompilingEffect
		{
			get
			{
				return Resources.ResourceManager.GetString("EffectProcessorWarningCompilingEffect", Resources.resourceCulture);
			}
		}
		internal static string ElementNotFound
		{
			get
			{
				return Resources.ResourceManager.GetString("ElementNotFound", Resources.resourceCulture);
			}
		}
		internal static string ErrorCreatingFont
		{
			get
			{
				return Resources.ResourceManager.GetString("ErrorCreatingFont", Resources.resourceCulture);
			}
		}
		internal static string FileCloseFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("FileCloseFailed", Resources.resourceCulture);
			}
		}
		internal static string FilenameNotAbsolute
		{
			get
			{
				return Resources.ResourceManager.GetString("FilenameNotAbsolute", Resources.resourceCulture);
			}
		}
		internal static string FileNotFound
		{
			get
			{
				return Resources.ResourceManager.GetString("FileNotFound", Resources.resourceCulture);
			}
		}
		internal static string FontDescriptionImporterDisplayName
		{
			get
			{
				return Resources.ResourceManager.GetString("FontDescriptionImporterDisplayName", Resources.resourceCulture);
			}
		}
		internal static string FontDescriptionNameCannotBeNullOrEmpty
		{
			get
			{
				return Resources.ResourceManager.GetString("FontDescriptionNameCannotBeNullOrEmpty", Resources.resourceCulture);
			}
		}
		internal static string FontDescriptionProcessorDisplayName
		{
			get
			{
				return Resources.ResourceManager.GetString("FontDescriptionProcessorDisplayName", Resources.resourceCulture);
			}
		}
		internal static string FontDescriptionSizeMustBeGreaterThanZero
		{
			get
			{
				return Resources.ResourceManager.GetString("FontDescriptionSizeMustBeGreaterThanZero", Resources.resourceCulture);
			}
		}
		internal static string FontNotFound
		{
			get
			{
				return Resources.ResourceManager.GetString("FontNotFound", Resources.resourceCulture);
			}
		}
		internal static string FontTextureProcessor_MultipleIndicesSameChar
		{
			get
			{
				return Resources.ResourceManager.GetString("FontTextureProcessor_MultipleIndicesSameChar", Resources.resourceCulture);
			}
		}
		internal static string FontTextureProcessorDisplayName
		{
			get
			{
				return Resources.ResourceManager.GetString("FontTextureProcessorDisplayName", Resources.resourceCulture);
			}
		}
		internal static string FontTextureProcessorParameterFirstCharacter
		{
			get
			{
				return Resources.ResourceManager.GetString("FontTextureProcessorParameterFirstCharacter", Resources.resourceCulture);
			}
		}
		internal static string FontTextureProcessorParameterFirstCharacterDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("FontTextureProcessorParameterFirstCharacterDescription", Resources.resourceCulture);
			}
		}
		internal static string FoundCyclicReference
		{
			get
			{
				return Resources.ResourceManager.GetString("FoundCyclicReference", Resources.resourceCulture);
			}
		}
		internal static string FragmentIdentifier
		{
			get
			{
				return Resources.ResourceManager.GetString("FragmentIdentifier", Resources.resourceCulture);
			}
		}
		internal static string GlyphPackerNoGlyphs
		{
			get
			{
				return Resources.ResourceManager.GetString("GlyphPackerNoGlyphs", Resources.resourceCulture);
			}
		}
		internal static string IndirectPositionCollectionHasNoParent
		{
			get
			{
				return Resources.ResourceManager.GetString("IndirectPositionCollectionHasNoParent", Resources.resourceCulture);
			}
		}
		internal static string IndirectPositionCollectionIsReadOnly
		{
			get
			{
				return Resources.ResourceManager.GetString("IndirectPositionCollectionIsReadOnly", Resources.resourceCulture);
			}
		}
		internal static string IntermediateDirectoryInvalid
		{
			get
			{
				return Resources.ResourceManager.GetString("IntermediateDirectoryInvalid", Resources.resourceCulture);
			}
		}
		internal static string IntermediateDirectoryNullOrEmpty
		{
			get
			{
				return Resources.ResourceManager.GetString("IntermediateDirectoryNullOrEmpty", Resources.resourceCulture);
			}
		}
		internal static string InteropFailedCreateD3D
		{
			get
			{
				return Resources.ResourceManager.GetString("InteropFailedCreateD3D", Resources.resourceCulture);
			}
		}
		internal static string InteropFailedCreateD3DDevice
		{
			get
			{
				return Resources.ResourceManager.GetString("InteropFailedCreateD3DDevice", Resources.resourceCulture);
			}
		}
		internal static string InteropFailedCreateDummyWindow
		{
			get
			{
				return Resources.ResourceManager.GetString("InteropFailedCreateDummyWindow", Resources.resourceCulture);
			}
		}
		internal static string InvalidAssemblyName
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidAssemblyName", Resources.resourceCulture);
			}
		}
		internal static string InvalidAssetName
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidAssetName", Resources.resourceCulture);
			}
		}
		internal static string InvalidEnumValue
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidEnumValue", Resources.resourceCulture);
			}
		}
		internal static string InvalidFilename
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidFilename", Resources.resourceCulture);
			}
		}
		internal static string InvalidTargetPlatform
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidTargetPlatform", Resources.resourceCulture);
			}
		}
		internal static string InvalidTargetProfile
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidTargetProfile", Resources.resourceCulture);
			}
		}
		internal static string InvalidVideoContent
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidVideoContent", Resources.resourceCulture);
			}
		}
		internal static string MaterialProcessorBadTypeForEffectParameter
		{
			get
			{
				return Resources.ResourceManager.GetString("MaterialProcessorBadTypeForEffectParameter", Resources.resourceCulture);
			}
		}
		internal static string MaterialProcessorEffectMaterialContentFilenameNullOrEmpty
		{
			get
			{
				return Resources.ResourceManager.GetString("MaterialProcessorEffectMaterialContentFilenameNullOrEmpty", Resources.resourceCulture);
			}
		}
		internal static string MaterialProcessorEffectMaterialDoesntHaveEffect
		{
			get
			{
				return Resources.ResourceManager.GetString("MaterialProcessorEffectMaterialDoesntHaveEffect", Resources.resourceCulture);
			}
		}
		internal static string MaterialProcessorParameterColorKeyColorDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("MaterialProcessorParameterColorKeyColorDescription", Resources.resourceCulture);
			}
		}
		internal static string MaterialProcessorParameterColorKeyEnabledDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("MaterialProcessorParameterColorKeyEnabledDescription", Resources.resourceCulture);
			}
		}
		internal static string MaterialProcessorParameterDefaultEffect
		{
			get
			{
				return Resources.ResourceManager.GetString("MaterialProcessorParameterDefaultEffect", Resources.resourceCulture);
			}
		}
		internal static string MaterialProcessorParameterDefaultEffectDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("MaterialProcessorParameterDefaultEffectDescription", Resources.resourceCulture);
			}
		}
		internal static string MaterialProcessorParameterGenerateMipmapsDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("MaterialProcessorParameterGenerateMipmapsDescription", Resources.resourceCulture);
			}
		}
		internal static string MaterialProcessorParameterPremultiplyTextureAlphaDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("MaterialProcessorParameterPremultiplyTextureAlphaDescription", Resources.resourceCulture);
			}
		}
		internal static string MaterialProcessorParameterResizeTexturesToPowerOfTwoDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("MaterialProcessorParameterResizeTexturesToPowerOfTwoDescription", Resources.resourceCulture);
			}
		}
		internal static string MeshBuilderInFinishedState
		{
			get
			{
				return Resources.ResourceManager.GetString("MeshBuilderInFinishedState", Resources.resourceCulture);
			}
		}
		internal static string MeshBuilderNotInSetupState
		{
			get
			{
				return Resources.ResourceManager.GetString("MeshBuilderNotInSetupState", Resources.resourceCulture);
			}
		}
		internal static string MeshBuilderTypesDoNotMatch
		{
			get
			{
				return Resources.ResourceManager.GetString("MeshBuilderTypesDoNotMatch", Resources.resourceCulture);
			}
		}
		internal static string MeshBuilderWrongNumberOfIndices
		{
			get
			{
				return Resources.ResourceManager.GetString("MeshBuilderWrongNumberOfIndices", Resources.resourceCulture);
			}
		}
		internal static string MissingAttribute
		{
			get
			{
				return Resources.ResourceManager.GetString("MissingAttribute", Resources.resourceCulture);
			}
		}
		internal static string MissingExternalReference
		{
			get
			{
				return Resources.ResourceManager.GetString("MissingExternalReference", Resources.resourceCulture);
			}
		}
		internal static string MissingNativeDependency
		{
			get
			{
				return Resources.ResourceManager.GetString("MissingNativeDependency", Resources.resourceCulture);
			}
		}
		internal static string MissingResource
		{
			get
			{
				return Resources.ResourceManager.GetString("MissingResource", Resources.resourceCulture);
			}
		}
		internal static string MobileNoEffects
		{
			get
			{
				return Resources.ResourceManager.GetString("MobileNoEffects", Resources.resourceCulture);
			}
		}
		internal static string MobileNoXact
		{
			get
			{
				return Resources.ResourceManager.GetString("MobileNoXact", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorDisplayName
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorDisplayName", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterColorKeyColorDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterColorKeyColorDescription", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterColorKeyEnabledDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterColorKeyEnabledDescription", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterGenerateMipmapsDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterGenerateMipmapsDescription", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterGenerateTangentFrames
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterGenerateTangentFrames", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterGenerateTangentFramesDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterGenerateTangentFramesDescription", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterPremultiplyTextureAlpha
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterPremultiplyTextureAlpha", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterPremultiplyTextureAlphaDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterPremultiplyTextureAlphaDescription", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterPremultiplyVertexColors
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterPremultiplyVertexColors", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterPremultiplyVertexColorsDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterPremultiplyVertexColorsDescription", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterResizeTexturesToPowerOfTwo
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterResizeTexturesToPowerOfTwo", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterResizeTexturesToPowerOfTwoDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterResizeTexturesToPowerOfTwoDescription", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterScale
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterScale", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterScaleDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterScaleDescription", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterSwapWindingOrder
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterSwapWindingOrder", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterSwapWindingOrderDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterSwapWindingOrderDescription", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterXAxisRotation
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterXAxisRotation", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterXAxisRotationDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterXAxisRotationDescription", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterYAxisRotation
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterYAxisRotation", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterYAxisRotationDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterYAxisRotationDescription", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterZAxisRotation
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterZAxisRotation", Resources.resourceCulture);
			}
		}
		internal static string ModelProcessorParameterZAxisRotationDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelProcessorParameterZAxisRotationDescription", Resources.resourceCulture);
			}
		}
		internal static string ModelsNotSupportedOnPlatform
		{
			get
			{
				return Resources.ResourceManager.GetString("ModelsNotSupportedOnPlatform", Resources.resourceCulture);
			}
		}
		internal static string NoDefaultConstructor
		{
			get
			{
				return Resources.ResourceManager.GetString("NoDefaultConstructor", Resources.resourceCulture);
			}
		}
		internal static string NoLoopInAudioFile
		{
			get
			{
				return Resources.ResourceManager.GetString("NoLoopInAudioFile", Resources.resourceCulture);
			}
		}
		internal static string NoRegistryPermissions
		{
			get
			{
				return Resources.ResourceManager.GetString("NoRegistryPermissions", Resources.resourceCulture);
			}
		}
		internal static string NormalWrongType
		{
			get
			{
				return Resources.ResourceManager.GetString("NormalWrongType", Resources.resourceCulture);
			}
		}
		internal static string NotACollectionType
		{
			get
			{
				return Resources.ResourceManager.GetString("NotACollectionType", Resources.resourceCulture);
			}
		}
		internal static string NotEnoughEntriesInXmlList
		{
			get
			{
				return Resources.ResourceManager.GetString("NotEnoughEntriesInXmlList", Resources.resourceCulture);
			}
		}
		internal static string NotIntermediateXml
		{
			get
			{
				return Resources.ResourceManager.GetString("NotIntermediateXml", Resources.resourceCulture);
			}
		}
		internal static string NullCompiledEffect
		{
			get
			{
				return Resources.ResourceManager.GetString("NullCompiledEffect", Resources.resourceCulture);
			}
		}
		internal static string NullElementName
		{
			get
			{
				return Resources.ResourceManager.GetString("NullElementName", Resources.resourceCulture);
			}
		}
		internal static string NullElementNotAllowed
		{
			get
			{
				return Resources.ResourceManager.GetString("NullElementNotAllowed", Resources.resourceCulture);
			}
		}
		internal static string NullVertexChannelEntry
		{
			get
			{
				return Resources.ResourceManager.GetString("NullVertexChannelEntry", Resources.resourceCulture);
			}
		}
		internal static string NullXnaFrameworkVersion
		{
			get
			{
				return Resources.ResourceManager.GetString("NullXnaFrameworkVersion", Resources.resourceCulture);
			}
		}
		internal static string PassThroughProcessorDisplayName
		{
			get
			{
				return Resources.ResourceManager.GetString("PassThroughProcessorDisplayName", Resources.resourceCulture);
			}
		}
		internal static string PathTooLong
		{
			get
			{
				return Resources.ResourceManager.GetString("PathTooLong", Resources.resourceCulture);
			}
		}
		internal static string ProcessorParametersDictionaryContainsWeirdEntry
		{
			get
			{
				return Resources.ResourceManager.GetString("ProcessorParametersDictionaryContainsWeirdEntry", Resources.resourceCulture);
			}
		}
		internal static string ProfileAspectRatio
		{
			get
			{
				return Resources.ResourceManager.GetString("ProfileAspectRatio", Resources.resourceCulture);
			}
		}
		internal static string ProfileFeatureNotSupported
		{
			get
			{
				return Resources.ResourceManager.GetString("ProfileFeatureNotSupported", Resources.resourceCulture);
			}
		}
		internal static string ProfileFormatNotSupported
		{
			get
			{
				return Resources.ResourceManager.GetString("ProfileFormatNotSupported", Resources.resourceCulture);
			}
		}
		internal static string ProfileMaxPrimitiveCount
		{
			get
			{
				return Resources.ResourceManager.GetString("ProfileMaxPrimitiveCount", Resources.resourceCulture);
			}
		}
		internal static string ProfileMaxVertexElements
		{
			get
			{
				return Resources.ResourceManager.GetString("ProfileMaxVertexElements", Resources.resourceCulture);
			}
		}
		internal static string ProfileMaxVertexStride
		{
			get
			{
				return Resources.ResourceManager.GetString("ProfileMaxVertexStride", Resources.resourceCulture);
			}
		}
		internal static string ProfileNoIndexElementSize32
		{
			get
			{
				return Resources.ResourceManager.GetString("ProfileNoIndexElementSize32", Resources.resourceCulture);
			}
		}
		internal static string ProfileNoSeparateAlphaBlend
		{
			get
			{
				return Resources.ResourceManager.GetString("ProfileNoSeparateAlphaBlend", Resources.resourceCulture);
			}
		}
		internal static string ProfileNotPowerOfTwo
		{
			get
			{
				return Resources.ResourceManager.GetString("ProfileNotPowerOfTwo", Resources.resourceCulture);
			}
		}
		internal static string ProfileNotPowerOfTwoDXT
		{
			get
			{
				return Resources.ResourceManager.GetString("ProfileNotPowerOfTwoDXT", Resources.resourceCulture);
			}
		}
		internal static string ProfileNotPowerOfTwoMipped
		{
			get
			{
				return Resources.ResourceManager.GetString("ProfileNotPowerOfTwoMipped", Resources.resourceCulture);
			}
		}
		internal static string ProfilePixelShaderModel
		{
			get
			{
				return Resources.ResourceManager.GetString("ProfilePixelShaderModel", Resources.resourceCulture);
			}
		}
		internal static string ProfileTooBig
		{
			get
			{
				return Resources.ResourceManager.GetString("ProfileTooBig", Resources.resourceCulture);
			}
		}
		internal static string ProfileVertexShaderModel
		{
			get
			{
				return Resources.ResourceManager.GetString("ProfileVertexShaderModel", Resources.resourceCulture);
			}
		}
		internal static string ReadOnlySharedResource
		{
			get
			{
				return Resources.ResourceManager.GetString("ReadOnlySharedResource", Resources.resourceCulture);
			}
		}
		internal static string RebuildAll
		{
			get
			{
				return Resources.ResourceManager.GetString("RebuildAll", Resources.resourceCulture);
			}
		}
		internal static string RebuildBecauseAssembliesChanged
		{
			get
			{
				return Resources.ResourceManager.GetString("RebuildBecauseAssembliesChanged", Resources.resourceCulture);
			}
		}
		internal static string RebuildBecauseAssemblyChanged
		{
			get
			{
				return Resources.ResourceManager.GetString("RebuildBecauseAssemblyChanged", Resources.resourceCulture);
			}
		}
		internal static string RebuildBecauseCantLoadCache
		{
			get
			{
				return Resources.ResourceManager.GetString("RebuildBecauseCantLoadCache", Resources.resourceCulture);
			}
		}
		internal static string RebuildBecauseSettingsChanged
		{
			get
			{
				return Resources.ResourceManager.GetString("RebuildBecauseSettingsChanged", Resources.resourceCulture);
			}
		}
		internal static string RebuildDirtyDependency
		{
			get
			{
				return Resources.ResourceManager.GetString("RebuildDirtyDependency", Resources.resourceCulture);
			}
		}
		internal static string RebuildMissingOutput
		{
			get
			{
				return Resources.ResourceManager.GetString("RebuildMissingOutput", Resources.resourceCulture);
			}
		}
		internal static string RebuildNewAsset
		{
			get
			{
				return Resources.ResourceManager.GetString("RebuildNewAsset", Resources.resourceCulture);
			}
		}
		internal static string RebuildNotBuilt
		{
			get
			{
				return Resources.ResourceManager.GetString("RebuildNotBuilt", Resources.resourceCulture);
			}
		}
		internal static string RebuildReferenceRenamed
		{
			get
			{
				return Resources.ResourceManager.GetString("RebuildReferenceRenamed", Resources.resourceCulture);
			}
		}
		internal static string RegistryError
		{
			get
			{
				return Resources.ResourceManager.GetString("RegistryError", Resources.resourceCulture);
			}
		}
		internal static string RequiredVertexChannelNotFound
		{
			get
			{
				return Resources.ResourceManager.GetString("RequiredVertexChannelNotFound", Resources.resourceCulture);
			}
		}
		internal static string SetPixelDataBadSourceDataLength
		{
			get
			{
				return Resources.ResourceManager.GetString("SetPixelDataBadSourceDataLength", Resources.resourceCulture);
			}
		}
		internal static string ShaderSizeAluOps
		{
			get
			{
				return Resources.ResourceManager.GetString("ShaderSizeAluOps", Resources.resourceCulture);
			}
		}
		internal static string ShaderSizeBoolConstants
		{
			get
			{
				return Resources.ResourceManager.GetString("ShaderSizeBoolConstants", Resources.resourceCulture);
			}
		}
		internal static string ShaderSizeError
		{
			get
			{
				return Resources.ResourceManager.GetString("ShaderSizeError", Resources.resourceCulture);
			}
		}
		internal static string ShaderSizeFloatConstants
		{
			get
			{
				return Resources.ResourceManager.GetString("ShaderSizeFloatConstants", Resources.resourceCulture);
			}
		}
		internal static string ShaderSizeIntConstants
		{
			get
			{
				return Resources.ResourceManager.GetString("ShaderSizeIntConstants", Resources.resourceCulture);
			}
		}
		internal static string ShaderSizeOps
		{
			get
			{
				return Resources.ResourceManager.GetString("ShaderSizeOps", Resources.resourceCulture);
			}
		}
		internal static string ShaderSizeTexOps
		{
			get
			{
				return Resources.ResourceManager.GetString("ShaderSizeTexOps", Resources.resourceCulture);
			}
		}
		internal static string SkeletonNotFound
		{
			get
			{
				return Resources.ResourceManager.GetString("SkeletonNotFound", Resources.resourceCulture);
			}
		}
		internal static string SkinningButNoWeights
		{
			get
			{
				return Resources.ResourceManager.GetString("SkinningButNoWeights", Resources.resourceCulture);
			}
		}
		internal static string SongProcessorDisplayName
		{
			get
			{
				return Resources.ResourceManager.GetString("SongProcessorDisplayName", Resources.resourceCulture);
			}
		}
		internal static string SongProcessorUnsupportedFormat
		{
			get
			{
				return Resources.ResourceManager.GetString("SongProcessorUnsupportedFormat", Resources.resourceCulture);
			}
		}
		internal static string SoundEffectProcessorDisplayName
		{
			get
			{
				return Resources.ResourceManager.GetString("SoundEffectProcessorDisplayName", Resources.resourceCulture);
			}
		}
		internal static string SoundEffectProcessorUnsupportedFormat
		{
			get
			{
				return Resources.ResourceManager.GetString("SoundEffectProcessorUnsupportedFormat", Resources.resourceCulture);
			}
		}
		internal static string SourceAssetNotFound
		{
			get
			{
				return Resources.ResourceManager.GetString("SourceAssetNotFound", Resources.resourceCulture);
			}
		}
		internal static string String1
		{
			get
			{
				return Resources.ResourceManager.GetString("String1", Resources.resourceCulture);
			}
		}
		internal static string TextureButNoTextureCoordinates
		{
			get
			{
				return Resources.ResourceManager.GetString("TextureButNoTextureCoordinates", Resources.resourceCulture);
			}
		}
		internal static string TextureCoordinateWrongType
		{
			get
			{
				return Resources.ResourceManager.GetString("TextureCoordinateWrongType", Resources.resourceCulture);
			}
		}
		internal static string TextureProcessorDisplayName
		{
			get
			{
				return Resources.ResourceManager.GetString("TextureProcessorDisplayName", Resources.resourceCulture);
			}
		}
		internal static string TextureProcessorObsolete
		{
			get
			{
				return Resources.ResourceManager.GetString("TextureProcessorObsolete", Resources.resourceCulture);
			}
		}
		internal static string TextureProcessorParameterColorKeyColor
		{
			get
			{
				return Resources.ResourceManager.GetString("TextureProcessorParameterColorKeyColor", Resources.resourceCulture);
			}
		}
		internal static string TextureProcessorParameterColorKeyColorDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("TextureProcessorParameterColorKeyColorDescription", Resources.resourceCulture);
			}
		}
		internal static string TextureProcessorParameterColorKeyEnabled
		{
			get
			{
				return Resources.ResourceManager.GetString("TextureProcessorParameterColorKeyEnabled", Resources.resourceCulture);
			}
		}
		internal static string TextureProcessorParameterColorKeyEnabledDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("TextureProcessorParameterColorKeyEnabledDescription", Resources.resourceCulture);
			}
		}
		internal static string TextureProcessorParameterGenerateMipmaps
		{
			get
			{
				return Resources.ResourceManager.GetString("TextureProcessorParameterGenerateMipmaps", Resources.resourceCulture);
			}
		}
		internal static string TextureProcessorParameterGenerateMipmapsDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("TextureProcessorParameterGenerateMipmapsDescription", Resources.resourceCulture);
			}
		}
		internal static string TextureProcessorParameterPremultiplyAlpha
		{
			get
			{
				return Resources.ResourceManager.GetString("TextureProcessorParameterPremultiplyAlpha", Resources.resourceCulture);
			}
		}
		internal static string TextureProcessorParameterPremultiplyAlphaDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("TextureProcessorParameterPremultiplyAlphaDescription", Resources.resourceCulture);
			}
		}
		internal static string TextureProcessorParameterResizeToPowerOfTwo
		{
			get
			{
				return Resources.ResourceManager.GetString("TextureProcessorParameterResizeToPowerOfTwo", Resources.resourceCulture);
			}
		}
		internal static string TextureProcessorParameterResizeToPowerOfTwoDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("TextureProcessorParameterResizeToPowerOfTwoDescription", Resources.resourceCulture);
			}
		}
		internal static string TextureProcessorParameterTextureFormat
		{
			get
			{
				return Resources.ResourceManager.GetString("TextureProcessorParameterTextureFormat", Resources.resourceCulture);
			}
		}
		internal static string TextureProcessorParameterTextureFormatDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("TextureProcessorParameterTextureFormatDescription", Resources.resourceCulture);
			}
		}
		internal static string TooManyBones
		{
			get
			{
				return Resources.ResourceManager.GetString("TooManyBones", Resources.resourceCulture);
			}
		}
		internal static string TooManyEntriesInXmlList
		{
			get
			{
				return Resources.ResourceManager.GetString("TooManyEntriesInXmlList", Resources.resourceCulture);
			}
		}
		internal static string TypeScannerError
		{
			get
			{
				return Resources.ResourceManager.GetString("TypeScannerError", Resources.resourceCulture);
			}
		}
		internal static string UnknownDeserializationType
		{
			get
			{
				return Resources.ResourceManager.GetString("UnknownDeserializationType", Resources.resourceCulture);
			}
		}
		internal static string ValidateCubemapNotSquare
		{
			get
			{
				return Resources.ResourceManager.GetString("ValidateCubemapNotSquare", Resources.resourceCulture);
			}
		}
		internal static string ValidateTextureBadDxtSize
		{
			get
			{
				return Resources.ResourceManager.GetString("ValidateTextureBadDxtSize", Resources.resourceCulture);
			}
		}
		internal static string ValidateTextureNoFaces
		{
			get
			{
				return Resources.ResourceManager.GetString("ValidateTextureNoFaces", Resources.resourceCulture);
			}
		}
		internal static string ValidateTextureNoMipmaps
		{
			get
			{
				return Resources.ResourceManager.GetString("ValidateTextureNoMipmaps", Resources.resourceCulture);
			}
		}
		internal static string ValidateTexturePartialMipChain
		{
			get
			{
				return Resources.ResourceManager.GetString("ValidateTexturePartialMipChain", Resources.resourceCulture);
			}
		}
		internal static string ValidateTextureTooManyMipmaps
		{
			get
			{
				return Resources.ResourceManager.GetString("ValidateTextureTooManyMipmaps", Resources.resourceCulture);
			}
		}
		internal static string ValidateTextureWrongMipCount
		{
			get
			{
				return Resources.ResourceManager.GetString("ValidateTextureWrongMipCount", Resources.resourceCulture);
			}
		}
		internal static string ValidateTextureWrongSize
		{
			get
			{
				return Resources.ResourceManager.GetString("ValidateTextureWrongSize", Resources.resourceCulture);
			}
		}
		internal static string ValidateTextureWrongType
		{
			get
			{
				return Resources.ResourceManager.GetString("ValidateTextureWrongType", Resources.resourceCulture);
			}
		}
		internal static string ValidateVolumeWrongMipCount
		{
			get
			{
				return Resources.ResourceManager.GetString("ValidateVolumeWrongMipCount", Resources.resourceCulture);
			}
		}
		internal static string VectorConverterInvalidType
		{
			get
			{
				return Resources.ResourceManager.GetString("VectorConverterInvalidType", Resources.resourceCulture);
			}
		}
		internal static string VertexBufferSizeNotMultipleOfStride
		{
			get
			{
				return Resources.ResourceManager.GetString("VertexBufferSizeNotMultipleOfStride", Resources.resourceCulture);
			}
		}
		internal static string VertexChannelAddWrongType
		{
			get
			{
				return Resources.ResourceManager.GetString("VertexChannelAddWrongType", Resources.resourceCulture);
			}
		}
		internal static string VertexChannelAlreadyExists
		{
			get
			{
				return Resources.ResourceManager.GetString("VertexChannelAlreadyExists", Resources.resourceCulture);
			}
		}
		internal static string VertexChannelCollectionAddViaInterface
		{
			get
			{
				return Resources.ResourceManager.GetString("VertexChannelCollectionAddViaInterface", Resources.resourceCulture);
			}
		}
		internal static string VertexChannelIsFixedSize
		{
			get
			{
				return Resources.ResourceManager.GetString("VertexChannelIsFixedSize", Resources.resourceCulture);
			}
		}
		internal static string VertexChannelNotFound
		{
			get
			{
				return Resources.ResourceManager.GetString("VertexChannelNotFound", Resources.resourceCulture);
			}
		}
		internal static string VertexChannelWrongContentType
		{
			get
			{
				return Resources.ResourceManager.GetString("VertexChannelWrongContentType", Resources.resourceCulture);
			}
		}
		internal static string VertexContentPositionIndexOutOfRange
		{
			get
			{
				return Resources.ResourceManager.GetString("VertexContentPositionIndexOutOfRange", Resources.resourceCulture);
			}
		}
		internal static string VertexElementBadUsage
		{
			get
			{
				return Resources.ResourceManager.GetString("VertexElementBadUsage", Resources.resourceCulture);
			}
		}
		internal static string VertexElementOffsetNotMultipleFour
		{
			get
			{
				return Resources.ResourceManager.GetString("VertexElementOffsetNotMultipleFour", Resources.resourceCulture);
			}
		}
		internal static string VertexElementOutsideStride
		{
			get
			{
				return Resources.ResourceManager.GetString("VertexElementOutsideStride", Resources.resourceCulture);
			}
		}
		internal static string VertexElementsOverlap
		{
			get
			{
				return Resources.ResourceManager.GetString("VertexElementsOverlap", Resources.resourceCulture);
			}
		}
		internal static string VertexHasUnknownBoneName
		{
			get
			{
				return Resources.ResourceManager.GetString("VertexHasUnknownBoneName", Resources.resourceCulture);
			}
		}
		internal static string VideoInvalidContent
		{
			get
			{
				return Resources.ResourceManager.GetString("VideoInvalidContent", Resources.resourceCulture);
			}
		}
		internal static string VideoProcessorDisplayName
		{
			get
			{
				return Resources.ResourceManager.GetString("VideoProcessorDisplayName", Resources.resourceCulture);
			}
		}
		internal static string VideoProcessorParameterSoundTrackType
		{
			get
			{
				return Resources.ResourceManager.GetString("VideoProcessorParameterSoundTrackType", Resources.resourceCulture);
			}
		}
		internal static string VideoProcessorParameterSoundTrackTypeDescription
		{
			get
			{
				return Resources.ResourceManager.GetString("VideoProcessorParameterSoundTrackTypeDescription", Resources.resourceCulture);
			}
		}
		internal static string WarnDuplicateAsset
		{
			get
			{
				return Resources.ResourceManager.GetString("WarnDuplicateAsset", Resources.resourceCulture);
			}
		}
		internal static string WarnDuplicateAssetBuiltUsing
		{
			get
			{
				return Resources.ResourceManager.GetString("WarnDuplicateAssetBuiltUsing", Resources.resourceCulture);
			}
		}
		internal static string WarnDuplicateAssetNoProcessor
		{
			get
			{
				return Resources.ResourceManager.GetString("WarnDuplicateAssetNoProcessor", Resources.resourceCulture);
			}
		}
		internal static string WarnDuplicateAssetReferencedBy
		{
			get
			{
				return Resources.ResourceManager.GetString("WarnDuplicateAssetReferencedBy", Resources.resourceCulture);
			}
		}
		internal static string WriteExternalReferenceBadPath
		{
			get
			{
				return Resources.ResourceManager.GetString("WriteExternalReferenceBadPath", Resources.resourceCulture);
			}
		}
		internal static string WriteExternalReferenceNotXnb
		{
			get
			{
				return Resources.ResourceManager.GetString("WriteExternalReferenceNotXnb", Resources.resourceCulture);
			}
		}
		internal static string WrongArgumentType
		{
			get
			{
				return Resources.ResourceManager.GetString("WrongArgumentType", Resources.resourceCulture);
			}
		}
		internal static string WrongContentProcessorInputType
		{
			get
			{
				return Resources.ResourceManager.GetString("WrongContentProcessorInputType", Resources.resourceCulture);
			}
		}
		internal static string WrongContentProcessorOutputType
		{
			get
			{
				return Resources.ResourceManager.GetString("WrongContentProcessorOutputType", Resources.resourceCulture);
			}
		}
		internal static string WrongExternalReferenceType
		{
			get
			{
				return Resources.ResourceManager.GetString("WrongExternalReferenceType", Resources.resourceCulture);
			}
		}
		internal static string WrongSharedResourceType
		{
			get
			{
				return Resources.ResourceManager.GetString("WrongSharedResourceType", Resources.resourceCulture);
			}
		}
		internal static string WrongVertexChannelSize
		{
			get
			{
				return Resources.ResourceManager.GetString("WrongVertexChannelSize", Resources.resourceCulture);
			}
		}
		internal static string WrongXmlType
		{
			get
			{
				return Resources.ResourceManager.GetString("WrongXmlType", Resources.resourceCulture);
			}
		}
		internal static string XactBuildFailed
		{
			get
			{
				return Resources.ResourceManager.GetString("XactBuildFailed", Resources.resourceCulture);
			}
		}
		internal static string XactBuilding
		{
			get
			{
				return Resources.ResourceManager.GetString("XactBuilding", Resources.resourceCulture);
			}
		}
		internal static string XactMissingWaveFile
		{
			get
			{
				return Resources.ResourceManager.GetString("XactMissingWaveFile", Resources.resourceCulture);
			}
		}
		internal static string XactProcessing
		{
			get
			{
				return Resources.ResourceManager.GetString("XactProcessing", Resources.resourceCulture);
			}
		}
		internal static string XactProjectFileNotFound
		{
			get
			{
				return Resources.ResourceManager.GetString("XactProjectFileNotFound", Resources.resourceCulture);
			}
		}
		internal static string XactSerializing
		{
			get
			{
				return Resources.ResourceManager.GetString("XactSerializing", Resources.resourceCulture);
			}
		}
		internal static string XactSkipBuild
		{
			get
			{
				return Resources.ResourceManager.GetString("XactSkipBuild", Resources.resourceCulture);
			}
		}
		internal static string XactWrongVersion
		{
			get
			{
				return Resources.ResourceManager.GetString("XactWrongVersion", Resources.resourceCulture);
			}
		}
		internal static string XmDeserializelException
		{
			get
			{
				return Resources.ResourceManager.GetString("XmDeserializelException", Resources.resourceCulture);
			}
		}
		internal static string XmlImporterDisplayName
		{
			get
			{
				return Resources.ResourceManager.GetString("XmlImporterDisplayName", Resources.resourceCulture);
			}
		}
		internal static string ZeroSizeIndexBuffer
		{
			get
			{
				return Resources.ResourceManager.GetString("ZeroSizeIndexBuffer", Resources.resourceCulture);
			}
		}
		internal static string ZeroSizeVertexBuffer
		{
			get
			{
				return Resources.ResourceManager.GetString("ZeroSizeVertexBuffer", Resources.resourceCulture);
			}
		}
		internal static string ZeroSizeVertexDeclaration
		{
			get
			{
				return Resources.ResourceManager.GetString("ZeroSizeVertexDeclaration", Resources.resourceCulture);
			}
		}
		internal Resources()
		{
		}
	}
}

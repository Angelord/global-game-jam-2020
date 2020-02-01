
using UnityEditor;

public class PixelArtImporter: AssetPostprocessor {
  
  void OnPreprocessTexture() {
    TextureImporter ti = ( TextureImporter ) assetImporter;
    ti.textureType = TextureImporterType.Sprite;
    ti.spritePixelsPerUnit = 32;
    ti.filterMode = UnityEngine.FilterMode.Point;
    ti.textureCompression = TextureImporterCompression.Uncompressed;
  }

}
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System;

namespace TexturePackerImporter
{
    public class AtlasImporter : AssetPostprocessor
    {
        private static Dictionary<string, SpriteMetaData[]> spriteMetaDataDictionary = new Dictionary<string, SpriteMetaData[]>();
        private static Dictionary<string, string> imagePathDictionary = new Dictionary<string, string>();

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            List<string> updatedAssets = new List<string>(importedAssets);
            updatedAssets.AddRange(movedAssets);
            foreach (string assetPath in updatedAssets)
            {
                if (!assetPath.EndsWith(".xml"))
                    continue;

                TextureAtlas atlas;
                try
                {
                    atlas = Utility.DeserializeXml<TextureAtlas>(assetPath);
                }
                catch (Exception e)
                {
                    continue;
                }

                SpriteMetaData[] spritesheet = new SpriteMetaData[atlas.sprites.Count];

                for (int i = 0; i < spritesheet.Length; i++)
                {
                    Sprite sprite = atlas.sprites[i];
                    spritesheet[i].name = sprite.name;
                    spritesheet[i].rect = new Rect(sprite.x, atlas.height - sprite.height - sprite.y, sprite.width, sprite.height);
                }

                string imagePath = Path.GetDirectoryName(assetPath) + "/" + atlas.imagePath;
                spriteMetaDataDictionary[imagePath] = spritesheet;
                imagePathDictionary[assetPath] = imagePath;

                AssetDatabase.ImportAsset(imagePath, ImportAssetOptions.ForceUpdate);
            }

            List<string> removedAssets = new List<string>(deletedAssets);
            removedAssets.AddRange(movedFromAssetPaths);
            foreach (string assetPath in removedAssets)
            {
                if (!assetPath.EndsWith(".xml"))
                    continue;

                string imagePath;
                if (!imagePathDictionary.TryGetValue(assetPath, out imagePath))
                    continue;

                spriteMetaDataDictionary.Remove(imagePath);
                imagePathDictionary.Remove(assetPath);
            }
        }

        void OnPostprocessTexture(Texture2D texture)
        {
            SpriteMetaData[] spritesheet;
            if (!spriteMetaDataDictionary.TryGetValue(assetPath, out spritesheet))
                return;

            TextureImporter textureImporter = assetImporter as TextureImporter;

            if (textureImporter.textureType != TextureImporterType.Advanced)
            {
                textureImporter.textureType = TextureImporterType.Sprite;
            }

            textureImporter.maxTextureSize = 4096;
            textureImporter.spriteImportMode = SpriteImportMode.Multiple;

            textureImporter.spritesheet = spritesheet;
        }
    }
}
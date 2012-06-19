using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace SpinEditor
{
    public static class STATIC_CONTBUILDER
    {
        public static string textureLoc = "Assets/Sprites/Textures/";

        public static ContentBuilder contentBuilder = new ContentBuilder();
        public static string pathToContent()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "Content/", textureLoc);      //path to the Content directory (inc the "bin" output directory)
        }

        /// <summary>
        /// Creates an XNB asset from the specified file
        /// </summary>
        public static string BuildXNBFromFile(string fileName)
        {
            // Tell the ContentBuilder what to build.
            contentBuilder.Clear();

            switch (Path.GetExtension(fileName))
            {
                case ".bmp":
                case ".dds":
                case ".png":
                    {
                        // add the file to the build project
                        // available processors are defined in ContentBuilder.cs under the "pipelineAssemblies" field
                        // you can add your own custom processors to this list in the same way
                        contentBuilder.Add(
                            fileName,
                            Path.GetFileNameWithoutExtension(fileName),
                            null,
                            "TextureProcessor");
                        break;
                    }
                default:
                    {
                        // unhandled asset type, abort
                        MessageBox.Show("Unkown asset type!", "Import Asset Failure!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return null;
                    }
            }
            // Build the asset
            string buildError = contentBuilder.Build();

            if (string.IsNullOrEmpty(buildError))
            {
                // if the build succeeded, we can do something with the XNB file we just created
                return Path.GetFileNameWithoutExtension(fileName) + ".xnb";
            }
            else
            {
                // if the build failed, display an error message
                MessageBox.Show(buildError, "Error");
            }

            return null;
        }


    }
}

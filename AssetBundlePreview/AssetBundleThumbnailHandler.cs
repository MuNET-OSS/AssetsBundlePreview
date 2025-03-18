using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using AssetStudio;
using SharpShell.Attributes;
using SharpShell.SharpThumbnailHandler;
using SixLabors.ImageSharp.Processing;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;

namespace AssetBundlePreview
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.FileExtension, ".ab")]
    public class AssetBundleThumbnailHandler : SharpThumbnailHandler
    {
        protected override Bitmap GetThumbnailImage(uint width)
        {
            try
            {
                var reader = new FileReader(@"C:\Fake", SelectedItemStream);
                var manager = new AssetsManager();
                manager.LoadFile(reader);
                manager.ReadAssets();
                manager.ProcessAssets();
                var asset = manager.assetsFileList[0].Objects.Find(it => it.type == ClassIDType.Texture2D);
                if (asset is null) return null;

                var texture = asset as Texture2D;
                var img = texture.ConvertToImage(true);
                // resize to width
                var scale = width / (float)img.Width;
                img.Mutate(x => x.Resize((int)(img.Width * scale), (int)(img.Height * scale)));
                return img.ToBitmap();
            }
            catch (Exception exception)
            {
                //  Log the exception and return null for failure
                LogError("An exception occurred", exception);
                return null;
            }
        }
    }
}
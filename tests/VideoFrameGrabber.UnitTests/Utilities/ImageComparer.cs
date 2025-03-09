using CoenM.ImageHash;
using CoenM.ImageHash.HashAlgorithms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace VideoFrameGrabber.UnitTests.Utilities;

/// <summary>
/// Provides methods for comparing the visual contents of images.
/// </summary>
public static class ImageComparer
{
    /// <summary>
    /// Gets the percent similarity between two image files.
    /// </summary>
    /// <param name="firstImagePath">File path of the first image.</param>
    /// <param name="secondImagePath">File path of the second image.</param>
    /// <returns>The percent similarity of the image from value range 0 to 100.</returns>
    public static double CompareSimilarity(string firstImagePath,  string secondImagePath)
    {
        var img1 = Image.Load<Rgba32>(firstImagePath);
        var img2 = Image.Load<Rgba32>(secondImagePath);

        return GetImageSimilarity(img1, img2);
    }

    /// <param name="firstImageBytes">Byte representation of the first image.</param>
    /// <param name="secondImageBytes">Byte representation of the second image.</param>
    /// <inheritdoc cref="CompareSimilarity(string, string)"/>
    public static double CompareSimilarity(byte[] firstImageBytes, byte[] secondImageBytes)
    {
        var img1 = Image.Load<Rgba32>(firstImageBytes);
        var img2 = Image.Load<Rgba32>(secondImageBytes);

        return GetImageSimilarity(img1, img2);
    }
    /// <param name="firstImageBytes">File path of the first image.</param>
    /// <param name="secondImageBytes">Byte representation of the second image.</param>
    /// <inheritdoc cref="CompareSimilarity(string, string)"/>
    public static double CompareSimilarity(string firstImagePath, byte[] secondImageBytes)
    {
        var img1 = Image.Load<Rgba32>(firstImagePath);
        var img2 = Image.Load<Rgba32>(secondImageBytes);

        return GetImageSimilarity(img1, img2);
    }

    /// <summary>
    /// Gets the percent similarity between two given <see cref="Image"/> instances.
    /// </summary>
    /// <param name="img1">The first <see cref="Image"/> instance.</param>
    /// <param name="img2">The second <see cref="Image"/> instance.</param>
    /// <inheritdoc cref="CompareSimilarity(string, string)" path="/returns"/>
    private static double GetImageSimilarity(Image<Rgba32> img1, Image<Rgba32> img2)
    {
        AverageHash hashAlgorithm = new();
        ulong imgHash1 = hashAlgorithm.Hash(img1);
        ulong imgHash2 = hashAlgorithm.Hash(img2);

        return CompareHash.Similarity(imgHash1, imgHash2);
    }
}

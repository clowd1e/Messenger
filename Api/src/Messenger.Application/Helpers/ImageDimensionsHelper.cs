using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace Messenger.Application.Helpers
{
    internal static class ImageDimensionsHelper
    {
        public static bool IconDimensionsAreEqual(IFormFile icon)
        {
            (int width, int height) = GetImageDimensions(icon);

            return width == height;
        }

        public static (int Width, int Height) GetImageDimensions(IFormFile file)
        {
            using var stream = file.OpenReadStream();

            using var image = Image.Load(stream);

            return (image.Width, image.Height);
        }
    }
}

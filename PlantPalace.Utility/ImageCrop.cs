using Microsoft.AspNetCore.Http;
using System.Reflection.Metadata;

namespace PlantPalace.Utility
{
    public class ImageCrop
    {
        public void Crop(string path,IFormFile img)
        {
            try
            {
                using (var image = Image.Load(img.OpenReadStream()))
                {
                    image.Mutate(x => x.Resize(300, 300));
                    image.Save(path);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

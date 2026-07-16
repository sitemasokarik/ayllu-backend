namespace DcodePe.Catering.Api.Helpers

{

    public static class UploadFileHelper

    {

        public static string ResolvePhysicalPath(IWebHostEnvironment environment, string relativeUrl)

            => AppStoragePaths.ResolvePhysicalPath(environment, relativeUrl);



        public static string GetContentType(string path)

        {

            var ext = Path.GetExtension(path).ToLowerInvariant();

            return ext switch

            {

                ".jpg" or ".jpeg" => "image/jpeg",

                ".png" => "image/png",

                ".webp" => "image/webp",

                ".pdf" => "application/pdf",

                _ => "application/octet-stream"

            };

        }

    }

}


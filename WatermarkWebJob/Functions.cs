using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.Azure.WebJobs;

namespace WatermarkWebJob
{
    public class Functions
    {
        public static void WebImageResize(
            [BlobTrigger("inputimages/{name}")] WebImage input,
            [Blob("outputimages/{name}")] out WebImage output)
        {
            output = input.Resize(500, 500).AddTextWatermark("VaHa bvba", fontColor: "Red");
        }
    }

    public class WebImageBinder : ICloudBlobStreamBinder<WebImage>
    {
        public Task<WebImage> ReadFromStreamAsync(Stream input, CancellationToken cancellationToken)
        {
            return Task.FromResult<WebImage>(new WebImage(input));
        }

        public Task WriteToStreamAsync(WebImage value, Stream output, CancellationToken cancellationToken)
        {
            var bytes = value.GetBytes();
            return output.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
        }
    }
}

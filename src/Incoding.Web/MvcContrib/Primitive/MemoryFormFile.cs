using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Incoding.Web.MvcContrib
{
    public class MemoryFormFile : IFormFile
    {
        readonly Stream inputStream;
        readonly string fileName;
        readonly string contentType;
        
        public MemoryFormFile(Stream stream, string fileName, string contentType)
        {
            inputStream = stream;
            this.fileName = fileName;
            this.contentType = contentType;
        }

        public Stream OpenReadStream()
        {
            return inputStream;
        }

        public void CopyTo(Stream target)
        {
            inputStream.CopyTo(target);
        }

        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = new CancellationToken())
        {
            return inputStream.CopyToAsync(target, 1000, cancellationToken);
        }

        public string ContentType
        {
            get { return contentType; }
        }

        public string ContentDisposition
        {
            get { return "content-disposition"; }
        }

        public IHeaderDictionary Headers { get { return new HeaderDictionary();} }
        public long Length { get { return inputStream.Length; } }
        public string Name { get { return "name"; } }
        public string FileName { get { return fileName; } }
    }
}
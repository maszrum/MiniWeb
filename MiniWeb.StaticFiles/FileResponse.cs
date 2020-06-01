using System;
using System.IO;
using System.Net;
using MiniWeb.Server.Responses;

namespace MiniWeb.StaticFiles
{
    internal class FileResponse : BaseResponse
    {
        public const int FileTimeoutSeconds = 30;

        private readonly string _filePath;

        public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public string ContentType { get; set; }

        public FileResponse(string contentType, string filePath)
        {
            if (string.IsNullOrWhiteSpace(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            ContentType = contentType;
            _filePath = filePath;
        }

        protected override void Rewrite(HttpListenerResponse response)
        {
            if (!File.Exists(_filePath))
            {
                StatusCode = HttpStatusCode.NotFound;
                return;
            }

            response.ContentType = ContentType;

            TryWriteFileToStream(response);
        }

        private void TryWriteFileToStream(HttpListenerResponse response)
        {
            var startedRewriting = DateTime.UtcNow;

            bool success;
            var forceThrow = false;
            do
            {
                success = CopyFileToStream(
                    _filePath, response.OutputStream, forceThrow);

                if (!success)
                {
                    forceThrow = IsTimeout(startedRewriting);
                }
            }
            while (!success);
        }

        public static bool IsTimeout(DateTime started)
        {
            var now = DateTime.UtcNow;
            var span = now - started;
            return span.Seconds >= FileTimeoutSeconds;
        }

        private static bool CopyFileToStream(string filePath, Stream stream, bool forceThrow = false)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    fileStream.CopyTo(stream);
                }
                return true;
            }
            catch (IOException)
            {
                if (forceThrow)
                    throw;
                return false;
            }
        }
    }
}

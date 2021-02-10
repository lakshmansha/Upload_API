using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Upload_API.Models;

namespace Upload_API.Services
{
    public class UploadService : IUploadService
    {
        private readonly ILogger<UploadService> _logger;
        private readonly UploadDBContext _context;


        public UploadService(ILogger<UploadService> logger, UploadDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<Upload> GetUploads()
        {
            List<Upload> list = new List<Upload>();

            try
            {
                list = _context.Uploads.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return list;
        }

        public async Task<bool> PostUploadsAsync(IFormFile content, string filePath)
        {
            bool IsSuccess = false;

            try
            {
                string fileuploadPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                if (!Directory.Exists(fileuploadPath))
                {
                    Directory.CreateDirectory(filePath);
                }

                Upload upload = new Upload();
                upload.Guid = Guid.NewGuid().ToString();

                var fileName = content.FileName;
                upload.LocalFileName = GetFileName(fileName) + "_" + upload.Guid + "." + GetFileExtension(fileName);

                upload.FileName = fileName;

                var fullPath = Path.Combine(fileuploadPath, upload.LocalFileName);
                upload.FilePath = Path.Combine(filePath, upload.LocalFileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    content.CopyTo(stream);
                    using (BinaryReader br = new BinaryReader(stream))
                    {
                        upload.FileBinary = br.ReadBytes((Int32)stream.Length);
                    }
                }

                upload.FileSize = content.Length;

                upload.UploadDate = DateTime.Now;

                _context.Uploads.Add(upload);
                await _context.SaveChangesAsync();

                IsSuccess = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return IsSuccess;
        }

        #region Internal Utility        

        private string GetFileName(string SourceUrl)
        {
            string rtnVal = string.Empty;
            string[] srcCnt = SourceUrl.Split('.');
            rtnVal = srcCnt[0];
            return rtnVal;
        }

        private string GetFileExtension(string UploadFileName)
        {
            string rtnVal = string.Empty;
            string[] srcCnt = UploadFileName.Split('.');
            rtnVal = srcCnt[srcCnt.Length - 1];
            return rtnVal;
        }

        #endregion

    }
}

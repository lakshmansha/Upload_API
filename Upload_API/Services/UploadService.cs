using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<bool> PostUploadsAsync(Upload upload)
        {
            bool IsSuccess = false;

            try
            {
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

    }
}

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upload_API.Services;
using Upload_API.Models;
using System.IO;
using System.Net.Http.Headers;

namespace Upload_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly ILogger<UploadController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUploadService _uploadService;

        public UploadController(ILogger<UploadController> logger, IConfiguration configuration, IUploadService uploadService)
        {
            _logger = logger;
            _configuration = configuration;
            _uploadService = uploadService;
        }

        [HttpGet]
        public IList<Upload> FetchUpload()
        {
            IList<Upload> rtnVal = new List<Upload>();
            try
            {                
                rtnVal = _uploadService.GetUploads();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return rtnVal;
        }

        public async Task<IActionResult> UploadAsync()
        {
            _logger.LogInformation("Enters: Upload");

            string rtnVal = string.Empty;
            try
            {
                var content = Request.Form.Files[0];
                var filePath = _configuration["UploadLocation"];
                string fileuploadPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                if (content.Length > 0)
                {
                    if (!Directory.Exists(fileuploadPath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    Upload upload = new Upload();
                    upload.Guid = Guid.NewGuid().ToString();

                    var fileName = ContentDispositionHeaderValue.Parse(content.ContentDisposition).FileName.Trim('"');
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

                    bool IsSuccess = await _uploadService.PostUploadsAsync(upload);

                    if (!IsSuccess)
                        rtnVal = "Error";
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            finally
            {
                _logger.LogInformation("Exits: Upload");
            }

            return string.IsNullOrEmpty(rtnVal) ?
                StatusCode(StatusCodes.Status201Created, " Uploaded Successfully") :
                StatusCode(StatusCodes.Status500InternalServerError, "Not-Upload Successfully");
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

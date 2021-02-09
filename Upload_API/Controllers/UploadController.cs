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

                if (content.Length > 0)
                {
                    bool IsSuccess = await _uploadService.PostUploadsAsync(content, filePath);

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
    }
}

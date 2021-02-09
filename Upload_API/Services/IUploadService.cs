using System.Collections.Generic;
using System.Threading.Tasks;
using Upload_API.Models;

namespace Upload_API.Services
{
    public interface IUploadService
    {
        List<Upload> GetUploads();
        Task<bool> PostUploadsAsync(Upload upload);
    }
}
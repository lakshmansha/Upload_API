using System;
using System.Collections.Generic;

#nullable disable

namespace Upload_API.Models
{
    public partial class Upload
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string FileName { get; set; }
        public decimal? FileSize { get; set; }
        public DateTime? UploadDate { get; set; }
        public byte[] FileBinary { get; set; }
        public string FilePath { get; set; }
        public string LocalFileName { get; set; }
    }
}

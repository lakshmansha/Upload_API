using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Upload_API.Models;
using Upload_API.Tests;

namespace Upload_API.Services.Tests
{
    [TestFixture()]
    public class UploadServiceTests
    {
        [SetUp]
        public void Setup()
        {
           
        }

        [Test]
        public void Get_Upload_Data()
        {
            //Arrange
            var serviceProvider = new ServiceCollection()
                       .AddLogging()
                       .BuildServiceProvider();
            var dbfactory = new ConnectionFactory();
            var context = dbfactory.CreateContextForInMemory();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<UploadService>();
            UploadService _service = new UploadService(logger, context);

            var GUID = Guid.NewGuid().ToString();
            var LocalFileName ="File Name_" + GUID + ".txt";
            var FilePath = "App_Data/File Name_" + GUID + ".txt";
            var upload = new Upload() { Guid = GUID, FileName = "File Name.txt", FileSize = 256, LocalFileName = LocalFileName, FilePath = FilePath, UploadDate = DateTime.Now };

            //Act 
            context.Uploads.Add(upload);
            context.SaveChanges();               
            List<Upload> uploads = _service.GetUploads();

            //Assert
            Assert.IsTrue(uploads.Count > 0);
        }

        [Test]
        public void Get_Upload_Zero()
        {
            //Arrange
            var serviceProvider = new ServiceCollection()
                      .AddLogging()
                      .BuildServiceProvider();
            var dbfactory = new ConnectionFactory();
            var context = dbfactory.CreateContextForInMemory();          

            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<UploadService>();
            UploadService _service = new UploadService(logger, context);

            //Act               
            List<Upload> uploads = _service.GetUploads();

            //Assert
            Assert.IsTrue(uploads.Count == 0);
        }

        [Test]
        public async Task Post_Upload_ZeroAsync()
        {
            //Arrange
            var serviceProvider = new ServiceCollection()
                      .AddLogging()
                      .BuildServiceProvider();

            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json").Build();

            var dbfactory = new ConnectionFactory();
            var context = dbfactory.CreateContextForInMemory();

            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<UploadService>();
            UploadService _service = new UploadService(logger, context);

            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Test_Image.jpg");
            var fileMock = new Mock<IFormFile>();
            var sourceImg = File.OpenRead(FilePath);
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(sourceImg);
            writer.Flush();
            stream.Position = 0;
            var fileName = "File_Name.png";
            fileMock.Setup(f => f.FileName).Returns(fileName).Verifiable();
            fileMock.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream stream, CancellationToken token) => stream.CopyToAsync(stream))
                .Verifiable();
            var inputFile = fileMock.Object;

            //Act               
            bool IsSuccess =  await _service.PostUploadsAsync(inputFile, configuration["UploadLocation"]);
            List<Upload> uploads = _service.GetUploads();

            //Assert
            Assert.IsTrue(IsSuccess);
            Assert.IsTrue(uploads.Count == 1);
        }

    }
}

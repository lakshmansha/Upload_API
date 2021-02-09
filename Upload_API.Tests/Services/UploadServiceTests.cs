using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}

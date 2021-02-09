using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Upload_API.Models;

namespace Upload_API.Tests
{
    public class ConnectionFactory : IDisposable
    {
        #region IDisposable Support  

        private bool disposedValue = false; 

        public UploadDBContext CreateContextForInMemory()
        {
            var option = new DbContextOptionsBuilder<UploadDBContext>().UseInMemoryDatabase(databaseName: "Upload_Database").Options;

            var context = new UploadDBContext(option);
            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
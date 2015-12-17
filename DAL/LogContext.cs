using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;

using ImageSharingWithCloudServices.Models;
using Microsoft.Azure;

namespace ImageSharingWithCloudServices.DAL
{
    public class LogContext
    {
        public const string LOG_TABLE_NAME = "imageviews";

        protected TableServiceContext context;

        public LogContext(TableServiceContext context)
        {
            this.context = context;
        }

        public void AddToEntry(ApplicationUser user, Image image)
        {
            LogEntry entry = new LogEntry(image.Id);
            entry.UserName = user.UserName;
            entry.Caption = image.Caption;
            entry.ImageId = image.Id;
            entry.Uri = image.Uri;
            context.AddObject(LOG_TABLE_NAME, entry);
            context.SaveChangesWithRetries();
        }

        public IEnumerable<LogEntry> select()
        {
            var results = from entity in context.CreateQuery<LogEntry>(LOG_TABLE_NAME)
                          where entity.PartitionKey == DateTime.UtcNow.ToString("MMddyyyy")
                          select entity;
            return results.ToList();
        }

        protected static CloudTableClient GetClient()
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(
                        ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
            //CloudStorageAccount account =
            //       CloudStorageAccount.Parse(
            //           CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient client = account.CreateCloudTableClient();
            return client;
        }

        protected static LogContext GetContext()
        {
            CloudTableClient client = GetClient();
            LogContext context = new LogContext(client.GetTableServiceContext());
            return context;
        }

        public static void CreateTable()
        {
            CloudTableClient client = GetClient();
            CloudTable table = client.GetTableReference(LOG_TABLE_NAME);
            table.CreateIfNotExists();
        }

        public static void AddLogEntry(ApplicationUser user, Image image)
        {
            LogContext context = GetContext();
            context.AddToEntry(user, image);
        }

        public static IEnumerable<LogEntry> Select()
        {
            LogContext context = GetContext();
            return context.select();
        }

    }
}
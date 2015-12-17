using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

using ImageSharingWithCloudServices.Models;

namespace ImageSharingWithCloudServices.DAL
{
    public class ValidationQueue
    {
        // The name of your queue
        public static string QueueName = "ValidationQueue";

        // QueueClient is thread-safe. Recommended that you cache 
        // rather than recreating it on every request
        QueueClient Client;
        //ManualResetEvent CompletedEvent = new ManualResetEvent(false);

        public void Initialize()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // Create the queue if it does not exist already
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }

            // Initialize the connection to Service Bus Queue
            Client = QueueClient.CreateFromConnectionString(connectionString, QueueName);
            //return base.OnStart();
        }

        public void send(ValidationRequest request)
        {
            Client.Send(new BrokeredMessage(request));
        }

        public void Finalize()
        {
            // Close the connection to Service Bus Queue
            Client.Close();
            //CompletedEvent.Set();
            //base.OnStop();
        }
    }
}
using System;
using Castle.Core.Logging;
using Microsoft.SupplyChain.Framework;
using Microsoft.ServiceBus.Messaging;
using Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Processors;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands
{
    public class IoTHubSubscriberCommand : BaseCommand<IoTHubSubscriberContext>
    {
       
        public IoTHubSubscriberCommand()
        {
        }

        protected override void DoExecute(IoTHubSubscriberContext context)
        {
            // fire up the event processor host.
            var eventProcessorHost = new EventProcessorHost(Guid.NewGuid().ToString(), context.IoTHubDeviceToCloudName, EventHubConsumerGroup.DefaultGroupName, context.IoTHubConnectionString, context.IoTHubStorageConnectionString, "messages-events");
            eventProcessorHost.RegisterEventProcessorFactoryAsync(new GenericEventProcessorFactory());


        }

        protected override void DoInitialize(IoTHubSubscriberContext context)
        {
            // get all iot hub config data from the service fabric config package.
            var configurationPackage = context.FabricServiceInstance.Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");

            var iotHubSection = configurationPackage.Settings.Sections["IoTHub"].Parameters;
            context.IoTHubConnectionString = iotHubSection["IoTHubConnectionString"].Value;
            context.IoTHubStorageConnectionString = iotHubSection["IoTHubAzureStorageConnectionString"].Value;
            context.IoTHubDeviceToCloudName = iotHubSection["IoTHubDeviceToCloudName"].Value;

            base.DoInitialize(context);
        }

        protected override ExceptionAction HandleError(IoTHubSubscriberContext context, Exception exception)
        {
           // _logger.ErrorFormat("Error processing event hub subscriber command: {0}", exception);
            return ExceptionAction.Rethrow;
        }
    }
}

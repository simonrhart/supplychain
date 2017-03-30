using System;
using Castle.Core.Logging;
using Microsoft.SupplyChain.Framework;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands
{
    public class IoTHubSubscriberCommand : BaseCommand<IoTHubSubscriberContext>
    {
        private readonly ILogger _logger;
     
        public IoTHubSubscriberCommand(ILogger logger)
        {
            _logger = logger;
        }

        protected override void DoExecute(IoTHubSubscriberContext context)
        {
            // fire up the event processor host.

        }

        protected override void DoInitialize(IoTHubSubscriberContext context)
        {
            // get all iot hub config data from the service fabric config package.
            var configurationPackage = context.FabricServiceInstance.Context.CodePackageActivationContext.GetConfigurationPackageObject("IoTHub");

            var iotHubUri = configurationPackage.Settings.Sections["IoTHubConnectionString"].Parameters;

            
            base.DoInitialize(context);
        }

        protected override ExceptionAction HandleError(IoTHubSubscriberContext context, Exception exception)
        {
            _logger.ErrorFormat("Error processing event hub subscriber command: {0}", exception);
            return ExceptionAction.Rethrow;
        }
    }
}

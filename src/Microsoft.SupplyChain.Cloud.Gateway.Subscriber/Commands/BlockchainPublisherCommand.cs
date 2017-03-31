using Microsoft.SupplyChain.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands
{
    public class BlockchainPublisherCommand : BaseCommand<BlockchainPublisherContext>
    {
        protected override void DoExecute(BlockchainPublisherContext context)
        {
        }

        protected override void DoInitialize(BlockchainPublisherContext context)
        {
            // get all iot hub config data from the service fabric config package.
            var configurationPackage = context.FabricServiceInstance.Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");

            var iotHubSection = configurationPackage.Settings.Sections["Blockchain"].Parameters;
           // context.IoTHubConnectionString = iotHubSection["IoTHubConnectionString"].Value;
            base.DoInitialize(context);
        }

        protected override ExceptionAction HandleError(BlockchainPublisherContext context, Exception exception)
        {
            return ExceptionAction.Rethrow;
        }
    }
}

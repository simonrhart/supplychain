using System;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Gateway.Contracts;
using Microsoft.SupplyChain.Framework.Command;

namespace Microsoft.SupplyChain.Cloud.Gateway.SubscriberService.Commands
{
    public class BlockchainPublisherCommand : BaseCommand<BlockchainPublisherContext>
    {
       
        protected override async Task DoExecuteAsync(BlockchainPublisherContext context)
        {
           
        }  

        protected override ExceptionAction HandleError(BlockchainPublisherContext context, Exception exception)
        {
            return ExceptionAction.Rethrow;
        }

        protected override TearDownAction HandleTearDown()
        {
            // executing tear down on dispose supresses this command as it will repeatedely be called
            return TearDownAction.OnDispose;
        }
    }
}

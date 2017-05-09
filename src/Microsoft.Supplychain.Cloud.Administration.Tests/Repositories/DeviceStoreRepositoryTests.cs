using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Fakes;
using Microsoft.Azure.Devices.Shared;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.SupplyChain.Cloud.Administration.DeviceStoreService.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Supplychain.Cloud.Administration.Tests.Repositories
{
    [TestClass]
    public class DeviceStoreRepositoriesTests
    {
        private RegistryManager _mockRegistryManager;

        [TestMethod]
        public void ShouldThrowOnNullRegistryManager()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new DeviceStoreRepository(null));
        }

        [TestMethod]
        public void ShouldThrowOnEmptyTags()
        {
            using (ShimsContext.Create())
            {
                   
            }

            string s = "";
            ShimRegistryManager.CreateFromConnectionStringString = x => RegistryManager.CreateFromConnectionString("");

            //RegistryManager m = new ShimRegistryManager(RegistryManager.CreateFromConnectionString(""))
            //{
            //    GetTwinAsync = ("x") =>
            //    {
            //}
            //}




            //  DeviceStoreRepository repo = new DeviceStoreRepository(mocked.Object);


            // assert
            //Assert.ThrowsException<ArgumentException>(() => repo.GetDeviceByIdAsync("1"));

        }

       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using JccPropertyHub.Domain.Core.Dto;
using JccPropertyHub.Domain.Core.Interfaces;
using JccPropertyHub.Domain.Core.Services;
using JccPropertyHub.Domain.Infraestructure.Connectors;
using Moq;
using Xunit;

namespace JccProperty.Domain.Core.Test {
    public class JccHubPropertiesInfraestructureTest {
        private readonly Mock<ISupplierConnectionsCache> supplierConnectionsCacheMock;

        public JccHubPropertiesInfraestructureTest() {
            SupplierConnectorLoaderMock = new Mock<ISupplierConnectorLoader>();
            supplierConnectionsCacheMock = new Mock<ISupplierConnectionsCache>();

            Fixture = (Fixture) new Fixture()
                .Customize(new AutoMoqCustomization());
        }

        private Fixture Fixture { get; }
        private Mock<ISupplierConnectorLoader> SupplierConnectorLoaderMock { get; }

        [Fact]
        public void It_SupplierConnectorManager_intherits_from_ISupplierConnectorManager() {
            var connectorManager = Fixture.Create<SupplierConnectorManager>();

            Assert.IsAssignableFrom<ISupplierConnectorManager>(connectorManager);
        }

        [Fact]
        public async Task Given_SupplierConnectorManager_load_ISupplierConnectorsAsync() {
            var configuration = Fixture.Create<ConnectorsConfiguration>();
            var supplierConnectionInstances = Fixture.CreateMany<ConcreteConnector>();

            var connectionInstances = supplierConnectionInstances as ConcreteConnector[] ??
                                      supplierConnectionInstances.ToArray();
            SupplierConnectorLoaderMock
                .Setup(p => p.GetInstances(It.IsAny<ConnectorsConfiguration>()))
                .Returns(connectionInstances);

            supplierConnectionsCacheMock
                .Setup(p => p.GetSupplierConnections())
                .Returns(Task.FromResult((IEnumerable<ISupplierConnector>) connectionInstances));

            var sut = new SupplierConnectorManager(SupplierConnectorLoaderMock.Object,
                supplierConnectionsCacheMock.Object);
            var actual = await sut.GetSupplierConnectors(configuration);

            Assert.IsAssignableFrom<ISupplierConnector>(actual.First());
        }


        private class ConcreteConnector : ISupplierConnector {
            public string SupplierConnectorName => "";

            public Task<SearchAvailabilityRs> SearchAvailability(SearchAvailabilityRq request) {
                throw new NotImplementedException();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using JccPropertyHub.Domain.Core.Dto;
using JccPropertyHub.Domain.Core.Interfaces;
using JccPropertyHub.Domain.Core.Services;
using JccPropertyHub.Domain.Core.Validators;
using Moq;
using Xunit;

namespace JccProperty.Domain.Core.Test {
    public class JccHubPropertiesTest {
        public JccHubPropertiesTest() {
            JccHubPropertiesMock = new Mock<IJccHubPropertiesService>();
            SearchAvalibilityRqValidatorMock = new Mock<IValidator<SearchAvailabilityRq, ResponseValidator>>();

            SupplierConnectorManagerMock = new Mock<ISupplierConnectorManager>();
            SupplierConnectorManagerMock.As<ISupplierConnector>();
            logStorageMock = new Mock<ILogStorage<Log>>();

            ConnectorsConfiguration = new ConnectorsConfiguration();
            Fixture = (Fixture) new Fixture()
                .Customize(new AutoMoqCustomization());
        }

        private Mock<IJccHubPropertiesService> JccHubPropertiesMock { get; }
        private Mock<IValidator<SearchAvailabilityRq, ResponseValidator>> SearchAvalibilityRqValidatorMock { get; }
        private Mock<ISupplierConnectorManager> SupplierConnectorManagerMock { get; }
        private Mock<ILogStorage<Log>> logStorageMock;
        private ConnectorsConfiguration ConnectorsConfiguration { get; }
        private Fixture Fixture { get; }

        [Fact]
        public void Its_JccHubPropertiesService_descending_from_IJccHubPropertiesService() {
            var jccHubProperties = new JccHubPropertiesService(
                SearchAvalibilityRqValidatorMock.Object,
                SupplierConnectorManagerMock.Object,
                ConnectorsConfiguration,
                logStorageMock.Object);

            Assert.IsAssignableFrom<IJccHubPropertiesService>(jccHubProperties);
        }

        [Fact]
        public async Task Given_SearchAvailabilityRQ_JccHubPropertiesService_response_with_SearchAvailabilityRS() {
            var request = Fixture.Create<SearchAvailabilityRq>();
            var response = Fixture.Create<SearchAvailabilityRs>();

            JccHubPropertiesMock
                .Setup(p => p.SearchAvailability(It.IsAny<SearchAvailabilityRq>()))
                .Returns(Task.FromResult(response));

            var actual = await JccHubPropertiesMock.Object.SearchAvailability(request);

            Assert.Equal(actual, response);
        }

        [Fact]
        public void It_SearchAvailabilityResponse_Intherits_from_Response_class() {
            var response = Fixture.Create<SearchAvailabilityRs>();

            Assert.IsAssignableFrom<Response>(response);
        }

        [Fact]
        public async Task Given_an_valid_SearchAvailabilityRQ_Response_without_errors() {
            var request = Fixture.Create<SearchAvailabilityRq>();
            var resonseValidator = GetValidResponseValidator();

            SearchAvalibilityRqValidatorMock
                .Setup(p => p.Validate(request))
                .Returns(resonseValidator);
            

            var sut = new JccHubPropertiesService(
                SearchAvalibilityRqValidatorMock.Object,
                SupplierConnectorManagerMock.Object,
                ConnectorsConfiguration,
                logStorageMock.Object);

            var actual = await sut.SearchAvailability(request);

            SearchAvalibilityRqValidatorMock.Verify(p => p.Validate(request));

            Assert.True(actual.Success);
            Assert.Empty(actual.Errors);
        }

        #region "SupplierConnectionManager"

        private class ConcreteSupplierConector : ISupplierConnector {
            private readonly ISupplierConnectorManager connectorManager;

            public ConcreteSupplierConector(ISupplierConnectorManager connectorManager) {
                this.connectorManager = connectorManager;
            }

            public string SupplierConnectorName => "";

            public Task<SearchAvailabilityRs> SearchAvailability(SearchAvailabilityRq request) {
                var suppliers = connectorManager.GetSupplierConnectors(new ConnectorsConfiguration());

                return Task.FromResult(new SearchAvailabilityRs());
            }
        }

        [Fact]
        public async Task Verify_is_using_ISupplierConnectorManager_to_returns_ISupplierConnectorsAsync() {
            var supplierConnectors = Fixture.CreateMany<ConcreteSupplierConector>();
            var request = Fixture.Create<SearchAvailabilityRq>();
            var resonseValidator = GetValidResponseValidator();

            SupplierConnectorManagerMock
                .Setup(p => p.GetSupplierConnectors(It.IsAny<ConnectorsConfiguration>()))
                .Returns(Task.FromResult((IEnumerable<ISupplierConnector>) supplierConnectors));

            SearchAvalibilityRqValidatorMock
                .Setup(p => p.Validate(request))
                .Returns(resonseValidator);

            var sut = new JccHubPropertiesService(
                SearchAvalibilityRqValidatorMock.Object,
                SupplierConnectorManagerMock.Object,
                ConnectorsConfiguration,
                logStorageMock.Object);

            var actual = await sut.SearchAvailability(request);

            SupplierConnectorManagerMock.Verify(p => p.GetSupplierConnectors(It.IsAny<ConnectorsConfiguration>()));
        }

        private ResponseValidator GetValidResponseValidator() {
            return Fixture
                .Build<ResponseValidator>()
                .With(p => p.IsValid, true)
                .With(p => p.Errors, Enumerable.Empty<Error>())
                .Create();
        }

        #endregion

        #region "Validators"

        [Fact]
        public async Task Given_an_invalid_SearchAvailabilityRQ_Response_with_errors() {
            var request = Fixture.Build<SearchAvailabilityRq>()
                .With(p => p.CheckIn, DateTime.UtcNow)
                .With(p => p.CheckOut, DateTime.UtcNow.AddDays(-1))
                .Create();

            var resonseValidator = Fixture
                .Build<ResponseValidator>()
                .With(p => p.IsValid, false)
                .Create();

            SearchAvalibilityRqValidatorMock
                .Setup(p => p.Validate(request))
                .Returns(resonseValidator);

            var sut = new JccHubPropertiesService(
                SearchAvalibilityRqValidatorMock.Object,
                SupplierConnectorManagerMock.Object,
                ConnectorsConfiguration,
                logStorageMock.Object);

            var actual = await sut.SearchAvailability(request);

            SearchAvalibilityRqValidatorMock.Verify(p => p.Validate(request));

            Assert.False(actual.Success);
            Assert.NotEmpty(actual.Errors);
        }

        [Fact]
        public async Task Given_an_invalid_SearchAvailabilityRQ_checkout_greeter_than_checkin_Response_with_errors() {
            var request = Fixture.Build<SearchAvailabilityRq>()
                .With(p => p.CheckIn, DateTime.UtcNow)
                .With(p => p.CheckOut, DateTime.UtcNow.AddDays(-2))
                .Create();

            var validator = new SearchAvailabilityRqValidator();

            var sut = new JccHubPropertiesService(
                validator,
                SupplierConnectorManagerMock.Object,
                ConnectorsConfiguration,
                logStorageMock.Object);

            var actual = await sut.SearchAvailability(request);

            Assert.False(actual.Success);
            Assert.NotEmpty(actual.Errors);
        }

        #endregion
    }
}
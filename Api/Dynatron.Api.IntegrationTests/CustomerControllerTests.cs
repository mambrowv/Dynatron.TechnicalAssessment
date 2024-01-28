using Dynatron.Api.Controllers.Commands;
using Dynatron.Api.Models;
using Dynatron.Shared;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Dynatron.Api.IntegrationTests
{
    public class CustomerControllerTests : IClassFixture<ApiFactory>
    {
        private readonly HttpClient _httpClient;

        public CustomerControllerTests(ApiFactory apiFactory)
        {
            _httpClient = apiFactory.CreateClient();
        }

        [Fact]
        public async Task GetList_FivePageSize_ReturnsPagedListWithFiveItems()
        {
            // Arrange
            var url = "/Customers?pageSize=5";

            // Act
            var response = await _httpClient.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.Should().NotBeNull();
            response.Content.Headers.ContentType?.ToString().Should().Be("application/json; charset=utf-8");

            var customers = JsonConvert.DeserializeObject<PagedList<CustomerModel>>(await response.Content.ReadAsStringAsync());

            customers.Page.Should().Be(1);
            customers.PageSize.Should().Be(5);
            customers.Items.Should().HaveCount(5);
        }

        [Fact]
        public async Task Get_CustomerIdExists_ReturnsCustomerModel()
        {
            // Arrange
            var url = "/Customers/1";

            // Act
            var response = await _httpClient.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.Should().NotBeNull();
            response.Content.Headers.ContentType?.ToString().Should().Be("application/json; charset=utf-8");

            var customer = JsonConvert.DeserializeObject<CustomerModel>(await response.Content.ReadAsStringAsync());
            
            customer.Should().NotBeNull();
            customer.CustomerId.Should().Be(1);
            customer.FirstName.Should().Be("Gilbert");
            customer.LastName.Should().Be("Gleason");
            customer.EmailAddress.Should().Be("GilbertGleason@yahoo.com");
            customer.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
            customer.UpdateDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
        }

        [Fact]
        public async Task Get_CustomerIdDoesNotExists_Returns404NotFound()
        {
            // Arrange
            var url = "/Customers/999999";

            // Act
            var response = await _httpClient.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            response.Content.Headers.ContentType.Should().NotBeNull();
            response.Content.Headers.ContentType?.ToString().Should().Be("application/json; charset=utf-8");
        }

        [Fact]
        public async Task Create_ValidCustomerCommand_CreatesAndReturnsCustomerModel()
        {
            // Arrange
            var url = "/Customers";
            var command = new CustomerCommand("Elliot", "Alderson", "EAlderson@EvilCorp.com");

            // Act
            var response = await _httpClient.PostAsync(url, JsonContent.Create(command));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Content.Headers.ContentType.Should().NotBeNull();
            response.Content.Headers.ContentType?.ToString().Should().Be("application/json; charset=utf-8");

            var customer = JsonConvert.DeserializeObject<CustomerModel>(await response.Content.ReadAsStringAsync());

            customer.Should().NotBeNull();
            customer.CustomerId.Should().BeGreaterThan(0);
            customer.FirstName.Should().Be("Elliot");
            customer.LastName.Should().Be("Alderson");
            customer.EmailAddress.Should().Be("EAlderson@EvilCorp.com");
            customer.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            customer.UpdateDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task Create_CustomerCommandWithInvalidEmail_Returns400WithEmailInError()
        {
            // Arrange
            var url = "/Customers";
            var command = new CustomerCommand("Ender", "Wiggin", "youarenotgettingyourmail");

            // Act
            var response = await _httpClient.PostAsync(url, JsonContent.Create(command));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Content.Headers.ContentType.Should().NotBeNull();
            response.Content.Headers.ContentType?.ToString().Should().Be("application/json; charset=utf-8");

            var validationError = JsonConvert.DeserializeObject<ValidationErrorModel>(await response.Content.ReadAsStringAsync());

            validationError.Should().NotBeNull();
            validationError.ErrorMessages.Should().ContainSingle();
            validationError.ErrorMessages.First().Should().Contain("Email Address");
        }

        [Fact]
        public async Task Create_CustomerCommandWithFirstNameOver50Characters_Returns400WithFirstNameInError()
        {
            // Arrange
            var url = "/Customers";
            var command = new CustomerCommand("Theodore-Jonathan-Lawrence-Alexander-Christopher-Montgomery-Harrison", "Smith", "theo@smith.com");

            // Act
            var response = await _httpClient.PostAsync(url, JsonContent.Create(command));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Content.Headers.ContentType.Should().NotBeNull();
            response.Content.Headers.ContentType?.ToString().Should().Be("application/json; charset=utf-8");

            var validationError = JsonConvert.DeserializeObject<ValidationErrorModel>(await response.Content.ReadAsStringAsync());

            validationError.Should().NotBeNull();
            validationError.ErrorMessages.Should().ContainSingle();
            validationError.ErrorMessages.First().Should().Contain("First Name");
        }

        [Fact]
        public async Task Update_CustomerId10AndValidCustomerCommand_UpdatesAndReturnsCustomerModel()
        {
            // Arrange
            var url = "/Customers/10";
            var command = new CustomerCommand("Peter", "Petrelli", "Peter@SaveTheCheerleaderSaveTheWorld.com");

            // Act
            var response = await _httpClient.PutAsync(url, JsonContent.Create(command));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.Should().NotBeNull();
            response.Content.Headers.ContentType?.ToString().Should().Be("application/json; charset=utf-8");

            var customer = JsonConvert.DeserializeObject<CustomerModel>(await response.Content.ReadAsStringAsync());

            customer.Should().NotBeNull();
            customer.CustomerId.Should().Be(10);
            customer.FirstName.Should().Be("Peter");
            customer.LastName.Should().Be("Petrelli");
            customer.EmailAddress.Should().Be("Peter@SaveTheCheerleaderSaveTheWorld.com");
            customer.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            customer.UpdateDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }
    }
}
using Dynatron.Api.Controllers.Commands;
using Dynatron.Api.Filters;
using Dynatron.Api.Models;
using Dynatron.Domain;
using Dynatron.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Dynatron.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public sealed class CustomersController : ControllerBase
    {
        private readonly IRepository _repository;

        public CustomersController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType<CustomerModel>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] int page = 1, [FromQuery] int pageSize = 25)
        {
            if(page <= 0 || pageSize <= 0)
                return Ok(new PagedList<CustomerModel>(new List<CustomerModel>(), default, default, default));

            var customers = await _repository.List<Customer>(new PaginationCriteria(page, pageSize));

            var model = new PagedList<CustomerModel>(
                customers.Items.Select(CustomerModel.FromEntity),
                customers.Page,
                customers.PageSize,
                customers.TotalRows);

            return Ok(model);
        }

        [HttpGet("{id}")]
        [ProducesResponseType<CustomerModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var customer = await _repository.GetById<Customer>(id);
            if (customer == null)
                return NotFound();

            return Ok(CustomerModel.FromEntity(customer));
        }

        [HttpPost]
        [Validate<CustomerCommand>]
        [ProducesResponseType<CustomerModel>(StatusCodes.Status201Created)]
        [ProducesResponseType<ValidationErrorModel>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CustomerCommand command)
        {
            var customer = new Customer(command.FirstName, command.LastName, command.EmailAddress);
            await _repository.Add(customer);

            return CreatedAtAction(nameof(Create), CustomerModel.FromEntity(customer));
        }

        [HttpPut("{id}")]
        [Validate<CustomerCommand>]
        [ProducesResponseType<CustomerModel>(StatusCodes.Status200OK)]
        [ProducesResponseType<ValidationErrorModel>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CustomerCommand command)
        {
            var customer = await _repository.GetById<Customer>(id);
            if (customer == null)
                return NotFound();

            customer.FirstName = command.FirstName;
            customer.LastName = command.LastName;
            customer.EmailAddress = command.EmailAddress;

            await _repository.Update(customer);

            return Ok(CustomerModel.FromEntity(customer));
        }
    }
}

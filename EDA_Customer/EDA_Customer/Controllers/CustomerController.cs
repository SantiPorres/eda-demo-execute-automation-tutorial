using EDA_Customer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDA_Customer.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController: ControllerBase
{
    private readonly CustomerDbContext _customerDbContext;
    public CustomerController(CustomerDbContext customerDbContext)
    {
        _customerDbContext = customerDbContext;        
    }

    [HttpGet]
    [Route("/customers")]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        return await _customerDbContext.Customers.ToListAsync();
    }
    
    [HttpGet]
    [Route("/products")]
    public ActionResult<IEnumerable<Product>> GetProducts()
    {
        return _customerDbContext.Products.ToList();
    }

    [HttpPost]
    public async Task Create(Customer customer)
    {
        _customerDbContext.Customers.Add(customer);
        await _customerDbContext.SaveChangesAsync();
    }
}
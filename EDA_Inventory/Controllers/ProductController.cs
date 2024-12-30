using System.Text.Json;
using System.Text.Json.Serialization;
using EDA_Inventory.Data;
using EDA_Inventory.RabbitMq;
using Microsoft.AspNetCore.Mvc;

namespace EDA_Inventory.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController: ControllerBase
{
    private readonly ProductDbContext _productDbContext;
    private readonly IRabbitMqUtil _rabbitMqUtil;

    public ProductController(ProductDbContext productDbContext, IRabbitMqUtil rabbitMqUtil)
    {
        _productDbContext = productDbContext;
        _rabbitMqUtil = rabbitMqUtil;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Products>> GetProducts()
    {
        return _productDbContext.Products.ToList(); 
    }

    [HttpPut]
    public async Task<ActionResult<Products>> UpdateProduct(Products products)
    {
        _productDbContext.Products.Update(products);
        
        await _productDbContext.SaveChangesAsync();

        var product = JsonSerializer.Serialize(new
        {
            products.Id,
            NewName = products.Name,
            products.Quantity,
        });
        return CreatedAtAction("GetProducts", new { products.Id }, product);
    }

    [HttpPost]
    public async Task<ActionResult<Products>> PostProduct(Products products)
    {
        _productDbContext.Products.Add(products);
        
        await _productDbContext.SaveChangesAsync();

        var product = JsonSerializer.Serialize(new
        {
            products.Id,
            products.ProductId,
            products.Name,
            products.Quantity,
        });
        
        Console.WriteLine("Product Created");
        await _rabbitMqUtil.PublishMessageQueue("inventory.product", product);
        
        return CreatedAtAction("GetProducts", new { products.Id }, product);
    }
}
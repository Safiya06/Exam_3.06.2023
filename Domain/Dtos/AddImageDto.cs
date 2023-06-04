using Domain.Dtos;
using Microsoft.AspNetCore.Http;

namespace Domain.Dtos;

public class AddImageDto :QuoteDto
{
    public IFormFile? File { get; set; }

}
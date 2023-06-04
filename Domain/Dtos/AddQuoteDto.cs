using Domain.Dtos;
using Microsoft.AspNetCore.Http;

namespace WebApi.Dtos;

public class AddQuoteDto : QuoteDto
{
    public IFormFile?  File { get; set; }
}


using Dapper;
using Domain.Context;
using Domain.Dtos;
using Infrastructure.Services;
using WebApi.Dtos;


namespace Wen.Services;
public class QuoteService
{
    private readonly IImageService _imageService;
    private readonly DapperContext _context;
    public QuoteService(DapperContext context, IImageService imageService)
    {
        _context = context;
        _imageService = imageService;
    }
    public List<GetQuoteDto> GetQuotes()
    {
        using (var conn = _context.CreateConnection())
        {
            var sql = "SELECT id,quotetext,categoryid,filename FROM quotes;";
            var result = conn.Query<GetQuoteDto>(sql).ToList();
            return result;
        }
    }
    public GetQuoteDto AddQuote(AddQuoteDto quote)
    {
        using (var conn = _context.CreateConnection())
        {
            //upload file
            var filename = _imageService.CreateFile("images", quote.File);
            var sql = "insert into quotes (quotetext,categoryid,file_name)values(@quotetext,@categoryid,@FileName)returning id";
            var result = conn.ExecuteScalar<int>(sql, new
            {
                quote.QuoteText,
                quote.CategoryId,
                filename

            });

            return new GetQuoteDto()
            {
                QuoteText = quote.QuoteText,
                CategoryId = quote.CategoryId,
                FileName = filename
            };
        }
    }

    public GetQuoteDto UpdateQuote(AddQuoteDto quote)
    {
        using (var conn = _context.CreateConnection())
        {
            var existing = conn.QuerySingleOrDefault<GetQuoteDto>("SELECT id,quotetext,categoryid,file_name as filename FROM quotes where id =@id; ",
            new {quote.Id});
            if (existing== null)
            {
                return new GetQuoteDto();
            }
            string? filename = null ;
            if (quote.File!=null && existing.FileName!=null   )
            {
              _imageService.DeleteFile("images",existing.FileName);
              filename = _imageService.CreateFile("images", quote.File);
            }
            else if (quote.File!=null && existing.FileName==null   )
            {
                filename = _imageService.CreateFile("images", quote.File);
            }
           
             var sql = "update quotes set quotetext=@quotetext,categoryid=@categoryid where id =@id ";
            if (quote.File !=null)
            {
                sql="update quotes set quotetext=@quotetext,categoryid=@categoryid,filename=@FileName where id =@id ";
            }
            var result = conn.ExecuteScalar<int>(sql, new
            {
                quote.QuoteText,
                quote.CategoryId,
                filename,
                quote.Id
            });

            return new GetQuoteDto()
            {
                QuoteText = quote.QuoteText,
                CategoryId = quote.CategoryId,
                FileName = filename,
                Id = result
            };
        }

    }
}
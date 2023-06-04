using Dapper;
using Domain.Context;
using Domain.Dtos;
using Npgsql;

public class CategoryService
{
   private readonly DapperContext _context;
    public CategoryService (DapperContext context)
    {
        _context = context;
    }

 public List<CategoryDto>GetCategories()
    {
    using(var conn=_context.CreateConnection() )
    {
        var sql="select id as Id,categoryname as CategoryName from categories";
            var result = conn.Query<CategoryDto>(sql);
       return result.ToList();
    }
    }
     public CategoryDto GetCategoryById(int id)
    {
    using(var conn=_context.CreateConnection() ){
        var sql=$"select id as Id,categoryname as CategoryName from categories where id=@ID";
       var result = conn.QuerySingle<CategoryDto>(sql,new {Id=id});
       return result;
    }
    }



   public CategoryDto AddCategory(CategoryDto category)
    {
    using(var conn= _context.CreateConnection()){
        var sql=$"insert into categories(id,categoryname )values ( @Id,@CategoryName) returning id";
       var result = conn.Execute(sql,new {category.Id,category.CategoryName});
       category.Id=result;
       return category;
    }
    }

        public CategoryDto UpdateCategory(CategoryDto category)
    {
    using(var conn= _context.CreateConnection()){
        var sql=$"update categories set id=@Id,categoryname=@CategoryName where id=@Id returning id";
       var result = conn.Execute(sql,new {category.Id,category.CategoryName});
       category.Id=result;
       return category;
    }
    }
       public  int DeleteCategory(int id)
    {
        using (var conn = _context.CreateConnection())
        {
            var sql = $"Delete from categories where id = @Id";
            var result=  conn.Execute(sql,new { Id = id});
            return result;
        }
    }


}
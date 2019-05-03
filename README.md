# Generic.Repository.EFCore
This is a Generic Repository Async make using EFCore 2.2.4

Code Quality - Master
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/b2b523e13d4b490187071837e8574570)](https://www.codacy.com/app/guilhermecaixeta/Generic.Service.DotNetCore2.2?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=guilhermecaixeta/Generic.Service.DotNetCore2.2&amp;utm_campaign=Badge_Grade)
Travis CI - Master 
[![Build Status](https://travis-ci.org/guilhermecaixeta/Generic.Repository.EFCore.svg?branch=master)](https://travis-ci.org/guilhermecaixeta/Generic.Repository.EFCore)

## This version is final.

This project has objective to made a CRUD more easily.
Adding an extra layer of abstraction in application.
This project has building using the best programmation pratices.

Principles used:
   * Reactive progammation;
   * *DRY* principle;
   * *SOLID* principles.

Add an generic layer of abstraction in application. 

This project is builded in *asp.net standard* and has the dependencies below:
* Microsoft.EntityFrameworkCore (>= 2.2.4)

 ### V.1.0.0
 An new project has born..

 ## DOCS

 To use this package is necessary make this steps:

 ### Step 1
 On startup project yor will add this

 ```
  public void ConfigureServices(IServiceCollection services)
        {
           //You can pass empty parameter on here!!! EX: Commom.SetSizeByLengthProperties("", "");
         Commom.SetSizeByLengthProperties("AssemblyName", "Namespace of Entity; Namespace of Filters");
         Commom.SaveOnCacheIfNonExists<TypeEntity>();
         ... rest your code...
 ```
 ### Step 2
 
```
   //Create your own repository, like this sample:
   //Interface
    public interface ICustomerRepository : IBaseRepositoryAsync<Customer, CustomerFilter>
    {
        IPage<Customer> CustomerPage(IPageConfig config);
    }

   //Implementation   
   public class CustomerRepository : BaseRepositoryAsync<Customer, CustomerFilter>, ICustomerRepository
   {

      public CustomerRepository(LocalDbContext context) : base(context)
      {
         //is this necessary
         includesString = new List<string>();
         //and this... 
         includesExp = new List<Expression<Func<Customer, object>>>();
      }
      // If you will use Pagination
      public IPage<Customer> CustomerPage(IPageConfig config)
      {
         return base._context.Set<Customer>().AsNoTracking().ToPage(config);
      }
   }

   //On startup you will add this:
   services.AddScoped<ICustomerRepository, CustomerRepository>();

   //If you will use auto generation filter you will need implements interface IFilter, like this...
       public class CustomerFilter : IFilter...
```

#### Step 2.1 If you will use IFilter implementation
Whith this implementation you don't need create a lambda in your filter.
This attribute says how your filter will be aplied on every request.
```
namespace Models.Filter
{
    public class CustomerFilter : IFilter
    {
        [Lambda(MethodOption = LambdaMethod.Contains, MergeOption = LambdaMerge.Or)]
        [FromQuery(Name = "Email")]
        public string Email { get; set; }

        [Lambda(MethodOption = LambdaMethod.Contains, MergeOption = LambdaMerge.Or)]
        [FromQuery(Name = "Name")]
        public string Name { get; set; }
        
        ///Between date
        [Lambda(NameProperty = "Birthday", MethodOption = LambdaMethod.GreaterThanOrEqual, MergeOption = LambdaMerge.Or)]
        public DateTime BirthdayMax { get; set; }
        [Lambda(NameProperty = "Birthday", MethodOption = LambdaMethod.GreaterThanOrEqual)]
        public DateTime BirthdayMin { get; set; }

    }
}
```
Fast attributes explanation:
* MethodOption, this say how method you will use to generate a lambda.-- Sample using lambda, x => name.Contais(x.name);
* MergeOption, says how you will merge each lambda attribute. -- Example: x => email.Contains(x.email) || nome.Contains(x.name);
* NameProperty, name property on entity which refers column in database;

### Step 3
Now you need add your repository on your Controller....
P.S.: This is a sample, if you want you can add repository on your Service layer and add your Service in your Controller, this is a simple sample.

```
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
      ///....code...
      ///Filter example
        [HttpGet("filter")]
        public async Task<ActionResult<List<Customer>>> GetAllFilterAsync([FromQuery]CustomerFilter filter)
        {
            try
            {
                var list = await _repo.FilterAllAsync(filter, true);
                if (list.Count() < 1)
                    return NoContent();
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest($"Message: {e.Message} - StackTrace: {e.StackTrace} {(e.InnerException != null ? "InnerException" + e.InnerException : "")}");
            }
        }

        /// Get All example
        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync(true);
            if (list.Count() < 1)
                return NoContent();
            return Ok(list);
        }

        /// <summary>
        /// Get All paginated example
        /// </summary>
        /// <returns></returns>
        [HttpGet("page")]
        public ActionResult<Page<Customer>> GetAllPaginated([FromQuery]PageConfig config)
        {
            try
            {
                return Ok(_repo.CustomerPage(config));
            }
            catch (Exception e)
            {
                return BadRequest($"Message: {e.Message} - StackTrace: {e.StackTrace} {(e.InnerException != null ? "InnerException" + e.InnerException : "")}");
            }
        }

    }

```
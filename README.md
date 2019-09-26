Code Quality - Master/Developer
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/b2b523e13d4b490187071837e8574570)](https://www.codacy.com/app/guilhermecaixeta/Generic.Service.DotNetCore2.2?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=guilhermecaixeta/Generic.Service.DotNetCore2.2&amp;utm_campaign=Badge_Grade)

Travis CI - Master/Developer
[![Build Status](https://travis-ci.org/guilhermecaixeta/Generic.Repository.EFCore.svg?branch=master)](https://travis-ci.org/guilhermecaixeta/Generic.Repository.EFCore)

Appveyor
[![Build status](https://ci.appveyor.com/api/projects/status/bv400l6e1wpd1de9?svg=true)](https://ci.appveyor.com/project/guilhermecaixeta/generic-repository-efcore)

Nuget
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Generic.RepositoryAsync.EFCore?label=Nuget%20Version)
![Nuget](https://img.shields.io/nuget/dt/Generic.RepositoryAsync.EFCore?label=Nuget%20Download)

## Summary
 1. [Overview](https://github.com/guilhermecaixeta/Generic.Repository.EFCore#Overview)
 2. [Version](https://github.com/guilhermecaixeta/Generic.Repository.EFCore#Version)
 3. [Documentation](https://github.com/guilhermecaixeta/Generic.Repository.EFCore#Doc)
    - [Step 1](https://github.com/guilhermecaixeta/Generic.Repository.EFCore#Step-1)
    - [Step 2](https://github.com/guilhermecaixeta/Generic.Repository.EFCore#Step-2)
      - [Step 2.1](https://github.com/guilhermecaixeta/Generic.Repository.EFCore#Step-21)
    - [Step 3](https://github.com/guilhermecaixeta/Generic.Repository.EFCore#Step-3)
 4. [About](https://github.com/guilhermecaixeta/Generic.Repository.EFCore#about-this-project)
 
# Overview
A Generic Repository Async developed using EFCore 2.2.4.
This project has objective to made a CRUD more easily.
Adding an extra layer of abstraction in application.
This project has building using the best programmation pratices.

Principles used:
   * Reactive progammation;
   * *D.R.Y* principle;
   * *S.O.L.I.D* principles.

This project is builded in *.net core* and has the dependencies below:
   * Microsoft.EntityFrameworkCore (>= 2.2.4) 

# Version
* V.1.0.0 - (DEPRECATED)
* V.1.0.1 - Release
  Features and fixs:
    * Fix include validation;
    * Add GetPageAsync on Repository;
    * Add unity test; 
    * Add possibility to return data mapped, adding the method responsible for mapping on the constructor*.
 * (Coming soon) V.1.0.2
  Features and fixs;
  (To be writed)

# DOC
 Here is describe how you need to use this library on your code. 

### Step 1
On startup project yor will add cache and the repo.

 ```
  public void ConfigureServices(IServiceCollection services)
        {
          services.AddSingleton<ICacheRepository,CacheRepository>();
         /*...configurations...*/
        }
 ```
 

### Step 2

```
   //Create your own repository, like this sample:
   //Interface
    public interface ICustomerRepository : IBaseRepositoryAsync<Customer, CustomerFilter>
    {   
        /*code of specific behavior*/
    }

   //Implementation   
   public class CustomerRepository : BaseRepositoryAsync<Customer, CustomerFilter>, ICustomerRepository
   {
      public CustomerRepository(
          LocalDbContext context, 
          ICacheRepository cacheRepository) : 
      base(
        context, 
        cacheRepository)
      {      }
   }
```

#### Step 2.1
Using this package you don't need create a functions to filter data in DB.
You just need implement the Interface IFilter and applying the attribute Lambda in every object what you need filter.
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

#### Attribue description:
| Name Attribute | Description | Attribute | Example |
|----------------|-------------|-----------|---------|
| MethodOption | This say how method you will use to generate a lambda. |  LambdaMethod.Contains | x => name.Contais(x.name) |
| MergeOption  | Tell how you will join each lambda. | MergeOption = LambdaMerge.Or | x => email.Contains(x.email) || nome.Contains(x.name) |
| NameProperty | Name of property from the entity wich be referenced | |

### Step 3
Now you need add your repository on your Controller....
```
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        /*....code...*/
        /*Filter example*/
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
        /// Get All paginated
        /// </summary>
        /// <returns></returns>
        [HttpGet("page")]
        public async Task<ActionResult<Page<Customer>>> GetAllPaginated([FromQuery]PageConfig config)
        {
            try
            {
                return Ok(await _repo.GetPageAsync(config, true));
            }
            catch (Exception e)
            {
                return BadRequest($"Message: {e.Message} - StackTrace: {e.StackTrace} {(e.InnerException != null ? "InnerException" + e.InnerException : "")}");
            }
        }
    }

```

# About this project
This project born as a study project, but I have been transform in something more "professional". Because this, that's project still in improvement constantly. I saw this wich an way to learn new things and apliyng then more profissionaly.

If you want see some Issue please open a issue, this is a way to improve my skills and help a little the comunity around the .Net.

# GitHub.Io
[GitHub.Io](https://guilhermecaixeta.github.io/Generic.Repository.EFCore/)

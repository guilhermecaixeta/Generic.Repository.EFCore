# Generic.Repository.EFCore

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
    - [Step 1](https://github.com/guilhermecaixeta/Generic.Repository.EFCore#1)
    - [Step 2](https://github.com/guilhermecaixeta/Generic.Repository.EFCore#2-Optional)
    - [Step 3](https://github.com/guilhermecaixeta/Generic.Repository.EFCore#3)
 4. [About](https://github.com/guilhermecaixeta/Generic.Repository.EFCore#about-this-project)

## Overview
This project has objective to made a CRUD more easily.
Adding an repository layer in application.

Principles used:
   * Reactive progammation;
   * *D.R.Y* principle;
   * *S.O.L.I.D* principles.

This project was build in *asp.net standard 2.1*.

Dependencies:
   * Microsoft.EntityFrameworkCore (>= 3.0.0)

## Versions 
* V.1.0.0 - (DEPRECATED)
 * Pilot
* V.1.0.1 - (DEPRECATED)
 New features and fixs:
  * Fix include validation;
  * Add GetPageAsync on Repository;
  * Add unity test; 
  * Add possibility to return data mapped, adding the method responsible for mapping on the constructor.
* V.1.0.2 - Lastest
 Improvements in this version:
  * Update package to .Net Framework 2.1 and EFCore Dependency to 3.0.0
  * Specialization of DBContext in implementation of IBaseRepository
  * Fix cache issue
  * Improve pagination
  * Default values in PageConfig (Sort: "Id", Order: "ASC", Page: 0, Size: 10)
  * Rename LambdaAttribute to FilterAttribute
  * Name property now can be used to define the name property used in pagination order
  * Add NoCacheableAttribute can be applied to attribute or object
  * Add restrictions to attribute to be cached
      * Need be a primitive types or a String or a derived types as StringBuilder...
      * Not be a IEnumerable, Array or any type of list
      * Can be a nullable primitive type
  * Add the methods with specific return
  * Validation method - "ThrowError" throw error if the condition is true
  * Specifics exceptions:
      * CacheNotInitializedException, throwed if the cache is not initialized
      * InvalidTypeException, if the type of object is forbiden for this action
      * LessThanOrEqualsZeroException, as the name says is throwed if the value is less or than zero
      * LessThanZero, throwed if value is less than zero
      * ListNullOrEmptyException, if the list is null or empty the exception is thrown
      * NotEqualsFieldException, if the value of the fields is not equals
  * Implemantation of BaseRepositoryAsync with filter has been separeted providing more abstraction

## DOC
This documentation has been updated with the version 1.0.2

 ### 1
 On startup project yor will add this

 ```
  public void ConfigureServices(IServiceCollection services)
        {
          services.AddSingleton<ICacheRepository,CacheRepository>();
          services.AddScoped(typeof(IBaseRepositoryAsync<>), typeof(BaseRepositoryAsync<,>));
         /*...configurations...*/
        }
 ```
 
#### 2 - Optional
It's mandatory if you will use the implementation of BaseRepositoryAsync which use IFilter.
With the IFilter, the query in generated automatically as the example below:

```
namespace Models.Filter
{*
    public c*lass CustomerFilter : IFilter
    {*
        [Fil*ter(MethodOption = LambdaMethod.Contains, MergeOption = LambdaMerge.Or)]
        [Fro*mQuery(Name = "Email")]
        publ*ic string Email { get; set; }

        [Filter(MethodOption = LambdaMethod.Contains, MergeOption = LambdaMerge.Or)]
        [FromQuery(Name = "Name")]
        public string Name { get; set; }
        
        /// Make a query using between dates
        [Filter(NameProperty = "Birthday", MethodOption = LambdaMethod.GreaterThanOrEqual, MergeOption = LambdaMerge.Or)]
        public DateTime BirthdayMax { get; set; }
        
        [Filter(NameProperty = "Birthday", MethodOption = LambdaMethod.GreaterThanOrEqual)]
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

 ### 3
Using the repository

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

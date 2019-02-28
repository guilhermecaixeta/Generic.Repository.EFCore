# Generic Repository .Net core 2.2 
This is a Generic Repository

VERSION 1.0.3 - Notes:
* All Namespace of project was changed to make more easily and intuitive.

    - BaseRepository
        * Before: IRepository
        * After: Generic.Repository.Base
    - Pagination
        * Before: Models.BaseEntities.Pagination
        * After: Generic.Repository.Extension.Pagination
    - BaseFilter
        * Before: Models.BaseEntities.BaseFilter
        * After: Generic.Repository.Entity.IFilter

* BaseFilter are changed to interface new is IBaseFilter.
* Filter was changed to attend more methods in lambda.

This project has objective to made a CRUD more easily.
Adding an extra layer of abstraction in application.
This project has building using the best programmation pratices.

Principles used:
* Reactive progammation;
* SOLID principles.

Add an generic layer of abstraction in application. 

This project is builded in *asp.net core 2.2* and has the dependencies below:
* Microsoft.EntityFrameworkCore (>= 2.2.1)

## *All methods is Async. This project is focused in repository pattern.*

Like or dislike, tell me and togheter make this project better.
*Come and be part of this project!*

Link to [this](https://www.nuget.org/packages/GenericModelRepository/1.0.3) package on nuget.org.
Link to [repository](https://github.com/guilhermecaixeta/GenericModelLayerRepository) 

## *DOCs*

For implements this package, follow the steps:

- Install package:
  * *Package Manager* > Install-Package GenericModelRepository -Version 1.0.3
  * *.Net CLI* > dotnet add package GenericModelRepository --version 1.0.3
  * *Paket CLI* > paket add GenericModelRepository --version 1.0.3
  
  
- In your code you make this to use the package:
  
```
//IBaseRepo
public interface IMyRepo : IBaseRepository<Entity, Filter> {}

//Implementation
public class MyRepo: BaseRepository<Entity, Filter>, IMyRepo{}

//On the startup
services.AddScoped<IMyRepo, MyRepo>();

///On the Controller
...Controller code
private readonly MyRepo _repo;

...ctor

pulic async MyEntity GetById(long id)
{
  return await _repo.GetByIdAsync(id);
}
....Controller code...
```

## From version 1.0.3
Atention on this

### The filter property names need to be the same as the entity
### Now this method can be generate lambda by names complements

If you add one this word below the generated lambda will attend this.

List words reserved to lambda methods:
* Equal
* Contains (only used in string types)
* GreaterThan
* LessThan
* GreaterThanOrEqual
* LessThanOrEqual

To use this words: IdEqual

*Generated lambda: x => x.Id == value;*

List words reserved to merge expressions:
* Or
* And

To use this words: IdEqualAnd

*Generated lambda: x => x.Id == value && .....;*
### If none word reserved is informed on properties the method assumes the follow default values:
* word reserved to merge expressions : And
* word reserved to lambda methods: Equal



To make a Pagination
```
JSON of BaseConfigurePagination
{
  (int)page : 0,
  (int) size : 0
  (string) sort : "ASC"
  (string) order : "Id"
}

JSON Page format
{
  "content": [
    {
      Entity Array
    }
  ],
  "totalElements": 0,
  "sort": "string",
  "order": "string",
  "size": 0,
  "page": 0
}
...Controller Code
        [HttpGet("Paginate")]
        public Pagination<Category> GetPage([FromQuery]BaseConfigurePagination config)
        {
            return _repo.GetAll().PaginateTo(config);
        }
...more code...
```

Saving data on database:
```
//The entity is the same of first example.

//In Controller
... Controller code

private readonly MyRepo _repo;

//...ctor and more code.....

public async Task<ActionResult<MyEntity>> PostAsync(MyEntity entity)
{
  entity = await _repo.CreateAsync(entity);
 return CreatedAtAction(nameof(GetByIdAsync), new { id = entity.Id }, entity);
}
```

Updating data:
```
//.....more code....

public async Task<ActionResult> PutAsync(long id, MyEntity entity)
{
  if(id != entity.Id)
    return BadRequest();
  await _repo.UpdateAsync(entity);
  return NoContent();
}
```

Delete data:
```
//...more code and finally...

public async Task<ActionResult> DeleteAsync(long id)
        {
            if (id < 1)
                return BadRequest();
            await _repo.DeleteAsync(id);
            return NoContent();
        }
```

[Here](https://github.com/guilhermecaixeta/TodoApi) are the implemented package on project using web api.

Doubts or recommendations? 
Send me an e-mail: guilherme.lpcaixeta@gmail.com



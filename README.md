# GenericModelLayerRepository
This is a Generic Repository


This a pilot project with objective to made a CRUD more easily. Adding an generic layer of abstraction in application. 

This project is builded in *asp.net core 2.2* and has the dependencies below:
 * Microsoft.EntityFrameworkCore (>= 2.2.1)

## *All methods is Async. This project is focused in repository pattern.*

Like or dislike, tell me and togheter make this project better.
*Come and be part of this project!*

Link to [this](https://www.nuget.org/packages/GenericModelRepository/1.0.0) package on nuget.org.
Link to [repository](https://github.com/guilhermecaixeta/GenericModelLayerRepository) 

## *DOCs*

For implements this package, follow the steps:

- Install package:
  * *Package Manager* > Install-Package GenericModelRepository -Version 1.0.0
  * *.Net CLI* > dotnet add package GenericModelRepository --version 1.0.0 
  * *Paket CLI* > paket add GenericModelRepository --version 1.0.0 
  
  
- In your repository make this:
  
```
public class MyRepo<E, F>: BaseRepository<E, F, MyContext>, IBaseRepository<E, F>
where E : class
where F : BaseFilter
{
//if has any code you implements here!!!
}

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



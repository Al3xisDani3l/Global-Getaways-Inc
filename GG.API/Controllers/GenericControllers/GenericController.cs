using GG.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.JsonPatch;

namespace GG.Api.Controllers
{
    /// <summary> 
    /// Clase generica base que contiene toda la logica de interaccion de las entidades del negocio de la API GG.
    /// Su implementacion con un tipo concreto debe ser establecidad para su correcto funcionamiento.
    /// </summary>
    /// <typeparam Nombre="TEntity">Entidad de negocio, esta debe ser una clase y poder ser instanciada, ademas implementar la interfaz <see cref="IEntity"/>.</typeparam>
    [Authorize(Roles = "Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController<TEntity, TPrivateEntity,TKey> : ControllerBase where TEntity : class, IEntity<TKey>, new() where TPrivateEntity : class, IEntity<TKey>, new() where TKey : IEquatable<TKey>
    {

        internal readonly IRepository<TPrivateEntity,TKey> _repository;

        internal readonly IMapper _mapper;

        public GenericController(IRepository<TPrivateEntity,TKey> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public virtual async Task<ActionResult<List<TEntity>>> Get()
        {

            var result = await _repository.GetAllAsync();
            if (result is not null)
            {
                var resultMapper = _mapper.Map<List<TEntity>>(result);
                return new OkObjectResult(resultMapper);
            }
            return BadRequest(new { message = "Ha ocurrido un error al intentar obtener los registros" });

        }
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TEntity>> Get(TKey id)
        {
            var result = await _repository.FindAsync(e => e.Id.Equals(id));
            if (result is not null)
            {
                var resultMapper = _mapper.Map<TEntity>(result);
                return new OkObjectResult(resultMapper);
            }
            return NotFound();
        }
        [HttpGet("uuid/{guid}")]
        public virtual async Task<ActionResult<TEntity>> Get(Guid guid)
        {
            var result = await _repository.FindAsync(e => e.Guid == guid);
            if (result is not null)
            {
                var resultMapper = _mapper.Map<TEntity>(result);
                return new OkObjectResult(resultMapper);
            }
            return NotFound();
        }
        [HttpPut("{id}")]
        public virtual async Task<ActionResult<TEntity>> Put(TKey id, TEntity element)
        {
            //Obtenemos los datos desde el repositorio
            var result = await _repository.FindAsync(e => e.Equals(id));

            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            if (result is not null)
            {

                var updateElement = _mapper.Map<TPrivateEntity>(element);

                updateElement.Id = result.Id;
                updateElement.Guid = result.Guid;

                var resultUpdate = await _repository.UpdateAsync(updateElement);

                var mapElementResult = _mapper.Map<TEntity>(resultUpdate);

                return Ok(mapElementResult);
            }

            return NotFound(new { message = $"No se encuentra ningun registro con el Id {id}" });

        }

        [HttpPut("uuid/{guid}")]
        public virtual async Task<ActionResult<TEntity>> Put(Guid guid, TEntity element)
        {
            var result = await _repository.FindAsync(e => e.Guid == guid);

            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }


            if (result is not null)
            {
                var updateElement = _mapper.Map<TPrivateEntity>(element);

                updateElement.Id = result.Id;
                updateElement.Guid = result.Guid;

                var resultUpdate = await _repository.UpdateAsync(updateElement);

                var mapElementResult = _mapper.Map<TEntity>(resultUpdate);

                return new OkObjectResult(mapElementResult);
            }

            return NotFound(new { message = $"No se encuentra ningun registro con el Guid {guid.ToString()}" });
        }

        [HttpPatch("{id}")]
        public virtual async Task<ActionResult<TEntity>> Patch(int id, JsonPatchDocument<TEntity> element)
        {

            var result = await _repository.FindAsync(e => e.Equals(id));

            if (element is not null)
            {
                if (result is not null)
                {
                    var resultMap = _mapper.Map<TEntity>(result);
                    element.ApplyTo(resultMap, ModelState);
                    resultMap.Id = result.Id;
                    resultMap.Guid = result.Guid;

                    if (!ModelState.IsValid)
                    {
                        return new BadRequestObjectResult(ModelState);
                    }

                    var reconvertMap = _mapper.Map<TPrivateEntity>(resultMap);

                    await _repository.UpdateAsync(reconvertMap);

                    return new OkObjectResult(resultMap);


                }
                return BadRequest(new { message = $"No se encuentra ningun registro con el id {id}" });
            }
            else
            {
                return new BadRequestObjectResult(ModelState);
            }
        }

        [HttpPatch("uuid/{guid}")]
        public virtual async Task<ActionResult<TEntity>> Patch(Guid guid, JsonPatchDocument<TEntity> element)
        {
            var result = await _repository.FindAsync(e => e.Guid == guid);

            if (element is not null)
            {
                if (result is not null)
                {
                    var resultMap = _mapper.Map<TEntity>(result);
                    element.ApplyTo(resultMap, ModelState);
                    resultMap.Id = result.Id;
                    resultMap.Guid = result.Guid;

                    if (!ModelState.IsValid)
                    {
                        return new BadRequestObjectResult(ModelState);
                    }

                    var reconvertMap = _mapper.Map<TPrivateEntity>(resultMap);

                    await _repository.UpdateAsync(reconvertMap);

                    return new OkObjectResult(resultMap);


                }
                return BadRequest(new { message = $"No se encuentra ningun registro con el Guid {guid}" });
            }
            else
            {
                return new BadRequestObjectResult(ModelState);
            }
        }

        [HttpPost]
        public virtual async Task<ActionResult<TEntity>> Post(TEntity element)
        {

            if (ModelState.IsValid)
            {
                element.Guid = Guid.NewGuid();

                var resultMap = _mapper.Map<TPrivateEntity>(element);
                var resultPost = await _repository.CreateAsync(resultMap);
                return new OkObjectResult(_mapper.Map<TEntity>(resultPost));
            }
            return new BadRequestObjectResult(ModelState);



        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<bool>> Delete(TKey id)
        {
            var result = await _repository.FindAsync(e => e.Equals(id));

            if (result is not null)
            {
                var resultDelete = await _repository.DeleteAsync(result);
                return new OkObjectResult(resultDelete);
            }

            return NotFound(new { message = $"No se encuentra ningun registro con el id {id}" });
        }

        [HttpDelete("uuid/{guid}")]
        public virtual async Task<ActionResult<bool>> Delete(Guid guid)
        {
            var result = await _repository.FindAsync(e => e.Guid == guid);

            if (result is not null)
            {
                var resultDelete = await _repository.DeleteAsync(result);
                return new OkObjectResult(resultDelete);
            }

            return NotFound(new { message = $"No se encuentra ningun registro con el Guid {guid}" });
        }



    }
}

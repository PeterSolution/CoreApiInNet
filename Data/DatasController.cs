using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreApiInNet.Model;
using AutoMapper;
using CoreApiInNet.Contracts;

namespace CoreApiInNet.Data
{
    [ApiController]
    [Route("api/datas")]
    public class DatasController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly InterfaceDataRepository dataRepository;

        public DatasController(IMapper mapper, InterfaceDataRepository dataRepository)
        {

            this.mapper = mapper;
            this.dataRepository = dataRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return dataRepository.GetAllAsync != null ?
                Ok(await dataRepository.GetAllAsync()) :
                Problem("Database is empty.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (dataRepository.GetAsync(id) == null)
            {
                return NotFound();
            }

            var dbModelData = await dataRepository.GetAsync(id);
            if (dbModelData == null)
            {
                return NotFound();
            }

            return Ok(dbModelData);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HelpingModelData model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dbModelData = mapper.Map<DbModelData>(model);
            using (var transaction = await dataRepository.StartTransaction())
            {
                /*DbModelData dbModelDataold = new DbModelData();
                dbModelDataold.data = model.data;
                dbModelDataold.IdUser = model.IdUser;*/



                await dataRepository.AddAsync(dbModelData);
                await transaction.CommitAsync();
                return CreatedAtAction(nameof(GetById), new { id = dbModelData.IdData }, dbModelData);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update(FullDataModel dbModelData)
        {

            using (var transaction = await dataRepository.StartTransaction())
            {
                try
                {

                    DbModelData dbmodel = await dataRepository.GetAsync(dbModelData.id);
                    if(dbmodel == null)
                    {
                        return NotFound();
                    }
                    mapper.Map(dbModelData, dbmodel);
                    //_context.Update(dbModelData);
                    try
                    {
                        await dataRepository.UpdateAsync(dbmodel);
                    }
                    catch (Exception ex)
                    {
                        return NotFound(ex);
                    }
                    await transaction.CommitAsync();
                    return Ok(dbmodel);
                }
                catch (DbUpdateConcurrencyException)
                {

                    return NotFound();
                }
            }



        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var dbModelData = await dataRepository.GetAsync(id);
            if (dbModelData == null)
            {
                return NotFound();
            }
            using (var transaction = await dataRepository.StartTransaction())
            {
                await dataRepository.DeleteAsync(id);
                await transaction.CommitAsync();
                return Ok();
            }
        }

        /*private bool DbModelDataExists(int id)
        {
            return (_context.DataModel?.Any(e => e.IdData == id)).GetValueOrDefault();
        }*/
    }
}

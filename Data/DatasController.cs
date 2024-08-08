using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreApiInNet.Model;
using AutoMapper;

namespace CoreApiInNet.Data
{
    [ApiController]
    [Route("api/datas")]
    public class DatasController : ControllerBase
    {
        private readonly ModelDbContext _context;
        private readonly IMapper mapper;

        public DatasController(ModelDbContext context,IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return _context.DataModel != null ?
                Ok(await _context.DataModel.ToListAsync()) :
                Problem("Entity set 'ModelDbContext.DataModel' is null.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (_context.DataModel == null)
            {
                return NotFound();
            }

            var dbModelData = await _context.DataModel.FindAsync(id);
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
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                /*DbModelData dbModelDataold = new DbModelData();
                dbModelDataold.data = model.data;
                dbModelDataold.IdUser = model.IdUser;*/

                var dbModelData = mapper.Map<DbModelData>(model);

                _context.Add(dbModelData);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return CreatedAtAction(nameof(GetById), new { id = dbModelData.IdData }, dbModelData);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update(FullDataModel dbModelData)
        {
            if(DbModelDataExists(dbModelData.id))
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        
                        DbModelData dbmodel=_context.DataModel.Find(dbModelData.id);
                        mapper.Map(dbModelData, dbmodel); 
                        //_context.Update(dbModelData);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return Ok(dbmodel);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DbModelDataExists(dbModelData.id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }

            

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            
            var dbModelData = await _context.DataModel.FindAsync(id);
            if (dbModelData == null)
            {
                return NotFound();
            }
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                _context.DataModel.Remove(dbModelData);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok();
            }
        }

        private bool DbModelDataExists(int id)
        {
            return (_context.DataModel?.Any(e => e.IdData == id)).GetValueOrDefault();
        }
    }
}

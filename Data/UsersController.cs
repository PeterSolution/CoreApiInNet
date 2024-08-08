using CoreApiInNet.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreApiInNet.Data
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly ModelDbContext _context;

        public UsersController(ModelDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return _context.UserModel != null ?
                Ok(await _context.UserModel.ToListAsync()) :
                Problem("Entity set 'ModelDbContext.UserModel' is null.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (_context.UserModel == null)
            {
                return NotFound();
            }
            var dbModelUser = await _context.UserModel.FindAsync(id);
            if (dbModelUser == null)
            {
                return NotFound();
            }
            return Ok(dbModelUser);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HelpingModelUser model)
        {
            
            DbModelUser dbModelUser = new DbModelUser();
            dbModelUser.Name = model.Name;
            dbModelUser.password = model.password;
            _context.Add(dbModelUser);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = dbModelUser.ID }, dbModelUser);
        }

        [HttpPut]
        public async Task<IActionResult> Update(DbModelUser dbModelUser)
        {
            if (DbModelUserExists(dbModelUser.ID))
            {
                _context.Update(dbModelUser);
                await _context.SaveChangesAsync();
                
            }

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var dbModelUser = await _context.UserModel.FindAsync(id);
            if (dbModelUser == null)
            {
                return NotFound();
            }
            _context.UserModel.Remove(dbModelUser);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool DbModelUserExists(int id)
        {
            return (_context.UserModel?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}

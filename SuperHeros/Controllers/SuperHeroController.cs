using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SuperHeros.Controllers
{
    [ApiController]
    [Route("SuperHero")]
    public class SuperHeroController : Controller
    {

        private static List<SuperHero> heros = new List<SuperHero>
    {
        new SuperHero
        {
            Id= 01,
            ChacterName= "Spiderman",
            FristName= "Peter",
            LastName= "ksjf",
            Address="london"
        },
        new SuperHero {
            Id= 02,
            ChacterName= "Ironman",
            FristName= "Tony",
            LastName= "Strack",
            Address="london"
        },

    };
        private readonly DataContext _context;

        public SuperHeroController(DataContext context)
        {
            _context = context;
        }


    [HttpGet("GetAllSuperHeros")]


    public async Task<ActionResult<List<SuperHero>>> Get()
        {
            try
            {
                return Ok(await _context.SuperHeroes.ToListAsync());
            }
            catch(Exception e)
            {

            return BadRequest(e);
            }
        }

        [HttpGet("GetSuperHeroById")]


    public async Task<ActionResult<List<SuperHero>>> Get(int id)
     {
            try
            {
                var hero = await _context.SuperHeroes.FindAsync(id);
                if(hero == null)
                {
                    return BadRequest("Hero Not Found");
                }else
                {
                    return Ok(hero);
                }
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }
     }


        [HttpPost("AddSuperHero")]

        public async Task<ActionResult<SuperHero>> Post(SuperHero hero)
        {
            try
            {
                 _context.SuperHeroes.Add(hero);
                await _context.SaveChangesAsync(); 

                return Ok(await _context.SuperHeroes.ToListAsync());

            }catch(Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut("ModifieSuperHeroById")]

        public async Task<ActionResult<SuperHero>> Put(SuperHero Request)
        {
            try
            {
                var dbhero = await _context.SuperHeroes.FindAsync(Request.Id);
                if (Request.Id != null && dbhero != null)
                {
                    dbhero.ChacterName = Request.ChacterName;
                    dbhero.FristName= Request.FristName;
                    dbhero.LastName = Request.LastName;
                    dbhero.Address = Request.Address;

                    await _context.SaveChangesAsync();

                    return Ok(await _context.SuperHeroes.ToListAsync());
                }
                else
                {
                    return BadRequest("Hero Not FOund");
                }
            }catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [HttpDelete("DeleteSuperHerobyId")]

        public async Task<ActionResult<SuperHero>> Delete(int Id)
        {
            try
            {
                var dbhero = await _context.SuperHeroes.FindAsync(Id);

                if (Id != null && dbhero != null)
                {
                     _context.SuperHeroes.Remove(dbhero);

                    await _context.SaveChangesAsync();
                    //heros.Remove(hero);

                    return Ok("Suceessfully delete the Super Hero!!");
                }
                else
                {
                    return BadRequest("Hero Not FOund");
                }
            }
            catch (IOException e)
            {
                return BadRequest(e);
            }
        }

    }
    
}

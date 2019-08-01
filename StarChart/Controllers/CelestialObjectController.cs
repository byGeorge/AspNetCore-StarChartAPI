using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
	[Route("")]
	[ApiController]
    public class CelestialObjectController : ControllerBase
    {
		private readonly ApplicationDbContext _context;
		public CelestialObjectController(ApplicationDbContext adc)
        {
            _context = adc;
        }

        public int OrbitedObjectId { get; private set; }

        [HttpGet("{id:int}", Name = "GetById")]
		public IActionResult GetById(int id)
        {
            CelestialObject co = _context.CelestialObjects.Find(id);
			if (co == null)
	            return NotFound();
            co.Satellites = _context.CelestialObjects.Where(e => OrbitedObjectId == id).ToList();
            return Ok(co);
        }

		[HttpGet("{name}")]
		public IActionResult GetByName(string name)
        {
            List<CelestialObject> cos = _context.CelestialObjects.Where(e => e.Name == name).ToList();
            if (!cos.Any())
                return NotFound();
            foreach( CelestialObject co in cos)
            {
                co.Satellites = _context.CelestialObjects.Where(e => e.Id == co.Id).ToList();
            }
            return Ok(cos);
        }

		[HttpGet]
		public IActionResult GetAll()
        {
            List<CelestialObject> cos = _context.CelestialObjects.ToList();
            foreach (CelestialObject co in cos)
            {
                co.Satellites = _context.CelestialObjects.Where(e => e.Id == co.Id).ToList();
            }
            return Ok(cos);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecordVoice.Domain;
using RecordVoice.Persistence;

namespace RecordVoice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly DataContext _context;
        public DeviceController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("add/{ip}")]
        public async Task<IActionResult> AddNewDevice (string ip) 
        {
            Record rec = new Record 
            {
                IP = ip,
                Connected=false,
                Recording = false,
                ApplicationOpened = false
            };
            await _context.Records.AddAsync(rec);
            await _context.SaveChangesAsync();

            return Ok(rec);
        }

        [HttpDelete("delete/{ip}")]
        public async Task<IActionResult> DeleteDevice (string ip) 
        {
            Record rec = await _context.Records.FirstAsync(r=>r.IP == ip);
            _context.Records.Remove(rec);
            await _context.SaveChangesAsync();

            return Ok(rec);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDevices () 
        {
            return Ok(await _context.Records.ToListAsync());
        }
    }
}
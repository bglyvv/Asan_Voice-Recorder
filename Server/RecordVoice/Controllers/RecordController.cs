using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecordVoice.Persistence;

namespace RecordVoice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecordController : ControllerBase
    {
        private readonly DataContext _context;
        public RecordController(DataContext context)
        {
            _context = context;
        }


    }
}
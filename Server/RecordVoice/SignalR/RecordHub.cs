using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RecordVoice.Domain;
using RecordVoice.Persistence;
using System.Linq;

namespace RecordVoice.SignalR
{
    public class RecordHub : Hub
    {
        private readonly DataContext _context;
        public RecordHub(DataContext context)
        {
            _context = context;
        }

        public async Task StartConnection(string ip) 
        {
            Record record = await _context.Records.FirstAsync(r=>r.IP == ip);
            if(!record.ApplicationOpened)
            {
                await Clients.Caller.SendAsync("appError", $"Application must be opened in the computer with ip: {ip}");
                return;
            }
            record.Connected = true;
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("getChanges", _context.Records.ToListAsync());

            // await Clients.
        }

        public async Task StartApplication(string ip) 
        {
            Record record = await _context.Records.FirstAsync(r=>r.IP == ip);
            record.ApplicationOpened = true;
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("getChanges", _context.Records.ToListAsync());

            // await Clients.
        }

        public async Task StopApplication(string ip) 
        {
            Record record = await _context.Records.FirstAsync(r => r.IP == ip);
            record.Connected = false;
            record.Recording = false;
            record.ApplicationOpened = false;
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("getChanges", _context.Records.ToListAsync());
            // await Clients.
        }

        public async Task StartRecord(string ip) 
        {
            Record record = await _context.Records.FirstAsync(r => r.IP == ip);
            if(record.Connected == false) 
            {
                await Clients.Caller.SendAsync("getError", "First you must be connected to device");
                return;
            }
            record.Recording = true;
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("getChanges", _context.Records.ToListAsync());
            await Clients.All.SendAsync("startRecord", ip);

            // await Clients.
        }

        public async Task StopConnection(string ip) 
        {
            Record record = await _context.Records.FirstAsync(r=>r.IP == ip);
            if(record.Recording == true) 
            {
                record.Recording = false;
                await Clients.All.SendAsync("stopRecord", ip);
            }
            record.Connected = false;
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("getChanges", _context.Records.ToListAsync());

            // await Clients.
        }

        public async Task StopRecord(string ip) 
        {
            Record record = await _context.Records.FirstAsync(r=>r.IP == ip);

            record.Recording = false;
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("getChanges", _context.Records.ToListAsync());
            await Clients.All.SendAsync("stopRecord", ip);
        }
    }
}
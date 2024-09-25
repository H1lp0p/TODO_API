using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TODO_API.Models;
using Microsoft.AspNetCore.JsonPatch;


namespace TODO_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly TicketContext _context;

        public TicketsController(TicketContext context)
        {
            _context = context;
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            return await _context.Tickets.ToListAsync();
        }

        [HttpGet("dashboard/{dashboard}")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetDashBoard(string dashboard)
        {
            var curDashboard = (await _context.Tickets.ToListAsync()).Where(el => el.Dashboard == dashboard);
            if (curDashboard.Any())
            {
                return Ok(curDashboard);
            }

            return NotFound();
        }

        [HttpPut("loadList")]
        public async Task<List<Ticket>> loadList(List<Ticket> newList)
        {
            _context.Tickets.RemoveRange(_context.Tickets);
            _context.Tickets.AddRange(newList);

            await _context.SaveChangesAsync();
            return newList;
        }


        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(long id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }

        [HttpPost("{id}-{newComplete}")]
        public async Task<ActionResult<Ticket>> SetComplete(bool newComplete, long id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.IsComplete = newComplete;
            _context.Entry(ticket).State = EntityState.Modified;

            return Ok();
        }


        // PUT: api/Tickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTicket([FromRoute]long id, [FromBody] JsonPatchDocument ticket)
        {

            var item = await _context.Tickets.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            ticket.ApplyTo(item);
			await _context.SaveChangesAsync();
			return Ok();
        }

        // POST: api/Tickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(long id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketExists(long id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}

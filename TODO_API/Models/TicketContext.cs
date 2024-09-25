using Microsoft.EntityFrameworkCore;

namespace TODO_API.Models
{
	public class TicketContext : DbContext
	{

		public TicketContext(DbContextOptions<TicketContext> options) : base(options) { 
		}

		public DbSet<Ticket> Tickets { get; set;}
	}
}

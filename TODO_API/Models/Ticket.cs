namespace TODO_API.Models
{
	public class Ticket
	{

		public long Id { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public bool IsComplete { get; set; } = false;
		public string Dashboard { get; set; } = "shared";

	}
}

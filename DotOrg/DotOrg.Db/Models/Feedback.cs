using System;
using DotOrg.Db.Entities;

namespace DotOrg.Db.Models
{
	public class Feedback : IEntity
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Text { get; set; }
		public DateTime Date { get; set; }
		public string AdminComment { get; set; }
	}
}

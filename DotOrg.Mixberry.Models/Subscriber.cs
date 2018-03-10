using System;
using DotOrg.Db.Entities;

namespace DotOrg.Mixberry.Models
{
	public class Subscriber: IEntity
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string Key { get; set; }
		public DateTime Date { get; set; }
		public bool IsApprowed { get; set; }
		public string Url { get; set; }
		public string Lang { get; set; }
	}
}
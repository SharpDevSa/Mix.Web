using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotOrg.Db.Entities.Mocks
{
	public class CustomMetadata : IMetadataEntity
	{
		public int Id { get; set; }
		public string MetaDescription { get; set; }
		public string MetaKeywords { get; set; }
		public string MetaTitle { get; set; }
		public string MetaData { get; set; }
	}
}

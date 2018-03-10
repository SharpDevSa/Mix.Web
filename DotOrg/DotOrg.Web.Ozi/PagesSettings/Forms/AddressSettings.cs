using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotOrg.Web.Ozi.PagesSettings.Forms
{
	public class AddressSettings : FieldSettings
	{
		public override string Control
		{
			get
			{
				return "ozi_Address";
			}
		}

		public string Longitude { get; set; }
		public string Latitude { get; set; }
	}
}

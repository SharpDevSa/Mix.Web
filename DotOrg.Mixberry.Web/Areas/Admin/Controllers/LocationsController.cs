using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DotOrg.Mixberry.Db;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Models.Entities;
using DotOrg.Mixberry.Models.Localization;

namespace DotOrg.Mixberry.Web.Areas.Admin.Controllers
{
	public partial class LocationsController : BaseAdminController<Location>
    {
		protected override IEnumerable<Location> GetIndexEntities()
		{
			var result = base.GetIndexEntities();
			var lang = DependencyResolver.Current.GetService<ILocalizationContext>();
			result = result.Where(x => x.Lang == lang.Current.Lang);
			return result;
		}

		protected override void OnEntityEdited(Location entity, FormCollection collection)
		{
			var lang = DependencyResolver.Current.GetService<ILocalizationContext>();
			entity.Lang = lang.Current.Lang;
			base.OnEntityEdited(entity, collection);
		}

		protected override void AppendBlock(Location entity, IBlockEntity block)
		{
			entity.Blocks.Add((LocationBlock)block);
			base.AppendBlock(entity, block);
		}

		protected override void RemoveBlock(Location entity, IBlockEntity block)
		{
			entity.Blocks.Remove((LocationBlock)block);
			DataModelContext.Set<LocationBlock>().Remove((LocationBlock)block);
			base.RemoveBlock(entity, block);
		}
    }
}
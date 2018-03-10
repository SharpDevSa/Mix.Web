using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotOrg.Db.Entities
{
    public interface ISortableByDateEntity:IEntity
    {
        DateTime PublishDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

namespace DotOrg.Mixberry.Web.Areas.Admin.Engine
{
    public class DbConnection
    {
        public static SqlConnection GetConnection
        {
            get { return new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString); }
        }

    }
}
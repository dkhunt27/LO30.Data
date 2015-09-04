using LO30.Data.Services;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Transactions;

namespace LO30.Data.Contexts
{
  //public class LO30ContextSeedInitializer : DropCreateDatabaseIfModelChanges<LO30Context>
  public class LO30ContextSeedInitializer : DropCreateDatabaseAlways<LO30Context>
  {

    protected override void Seed(LO30Context context)
    {
      base.Seed(context);
      LO30ContextSeed seeder = new LO30ContextSeed(context);
      seeder.Seed();
    }
  }
}

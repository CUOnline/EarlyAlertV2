using System;
using System.Collections.Generic;
using System.Text;

namespace EarlyAlertV2.Repository
{
    public abstract class RepositoryBase
    {
        protected readonly EarlyAlertV2Context Context;

        protected RepositoryBase(EarlyAlertV2Context context)
        {
            Context = context;
        }
    }
}

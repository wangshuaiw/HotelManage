using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace HotelManage.Common
{
    public class EFHelper
    {
        public static EntityEntry<T> GetEntry<T>(DbContext db, T t,params string[] properties) where T:class
        {
            var entry = db.Set<T>().Attach(t);
            entry.State = EntityState.Unchanged;
            foreach(var p in properties)
            {
                entry.Property(p).IsModified = true;
            }
            return entry;
        }
    }
}

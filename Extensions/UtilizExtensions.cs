using AEDFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AEDFirst.Extensions
{
    public static class UtilizExtensions
    {
        public static bool HasRight(this UTILIZ User, string CodeDrt)
        {
            using (ModelAED db = new ModelAED()){
                DROITS Dt = db.DROITS.Where(d => d.CodeDrt == CodeDrt).FirstOrDefault();
                var query = from d in db.DROITS
                            join ud in db.UTILIZDROITS on d.IdDrt equals ud.IdDrt
                            join u in db.UTILIZ on ud.IdUtiliz equals u.IdUtiliz
                            where u.IdUtiliz == User.IdUtiliz & u.Active == true
                            select d;
                List<DROITS> rights = query.ToList();
                return rights.Contains(Dt);
            }
            
        }
        public static void AddRights(this UTILIZ user, params string[] codeDrts)
        {
            using (ModelAED db = new ModelAED())
            {
                foreach (string codeDrt in codeDrts)
                {
                    DROITS dt = db.DROITS.FirstOrDefault(d => d.CodeDrt == codeDrt);
                    if (dt != null)
                    {
                        UTILIZDROITS udToAdd = new UTILIZDROITS
                        {
                            IdUtiliz = user.IdUtiliz,
                            IdDrt = dt.IdDrt,
                            DateUD = DateTime.UtcNow
                        };
                        db.UTILIZDROITS.Add(udToAdd);
                    }
                }
                db.SaveChanges();
            }
        }

    }
}
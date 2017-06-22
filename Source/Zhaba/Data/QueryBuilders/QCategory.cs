using NFX.DataAccess.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaba.Data.Filters;

namespace Zhaba.Data.QueryBuilders
{
    /// <summary>
    /// Shortcuts for category data query
    /// </summary>
    public class QCategory
    {
        public static Query<TRow> findCategoryByFilter<TRow>(CategoryListFilter filter) where TRow : Row
        {
            return new Query<TRow>("SQL.Filters.CategoryList")
            {
            new Query.Param("pFilter", filter)
            };
        }

        public static Query<TRow> findCategoryByID<TRow>(ulong id) where TRow: Row
        {
            return new Query<TRow>("SQL.CRUD.Category.ByID")
            {
            new Query.Param("pID", id)
            };
        }
    }
}

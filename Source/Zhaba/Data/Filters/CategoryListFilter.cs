using NFX.DataAccess.CRUD;
using System;
using Zhaba.Data.Forms;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Filters
{
    public class CategoryListFilter : ZhabaFilterForm
    {
        #region Nested
        private class CategoryFilterListRow : TypedRow
        {
            [Field] public ulong Counter { get; set; }
            [Field] public string Name { get; set; }
            [Field] public string Description { get; set; }
        }
        #endregion

        [Field(valueList: "Name:Name Ascending,-Name:Name Descending",
               metadata: "Description='Sort By' Hint='Sort category list by'")]
        public string OrderBy { get; set; }

        [Field(metadata: "Description='Name' Placeholder='Name' Hint='Category Name'")]
        public string Name { get; set; }

        [Field(metadata: "Description='Description' Placeholder='Description' Hint='Category Description'")]
        public string Description { get; set; }

        protected override Exception DoSave(out object saveResult)
        {
            var qry = QCategory.findCategoryByFilter<CategoryFilterListRow>(this);
            saveResult = ZApp.Data.CRUD.LoadOneRowset(qry);
            return null;
        }

    }
}

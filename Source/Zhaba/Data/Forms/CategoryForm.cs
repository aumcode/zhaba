using System;

using NFX;
using NFX.DataAccess;
using NFX.DataAccess.CRUD;
using NFX.Wave;

using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;

namespace Zhaba.Data.Forms
{
    public class CategoryForm : ZhabaForm
    {
        public CategoryForm() { }
        public CategoryForm(ulong? id)
        {
            if (id.HasValue)
            {
                FormMode = FormMode.Edit;

                var qry = QCategory.findCategoryByID<CategoryRow>(id.Value);
                var row = ZApp.Data.CRUD.LoadRow(qry);
                if (row != null)
                    row.CopyFields(this);
                else
                    throw HTTPStatusException.NotFound_404("Project");

                this.RoundtripBag[ITEM_ID_BAG_PARAM] = id.Value;
            }
            else
                FormMode = FormMode.Insert;
        }

        [Field(typeof(CategoryRow))]
        public string Name { get; set; }

        [Field(typeof(CategoryRow))]
        public string Description { get; set; }

        protected override Exception DoSave(out object saveResult)
        {
            saveResult = null;
            try
            {

                var id = RoundtripBag[ITEM_ID_BAG_PARAM].AsNullableULong();
                CategoryRow row = FormMode == FormMode.Edit && id.HasValue
                  ? ZApp.Data.CRUD.LoadRow(QCategory.findCategoryByID<CategoryRow>(id.Value))
                  : new CategoryRow(RowPKAction.Default);

                CopyFields(row);

                var verror = row.ValidateAndPrepareForStore();

                if (verror != null) return verror;

                ZApp.Data.CRUD.Upsert(row);
                saveResult = row;

            }
            catch (DataAccessException error)
            {
                if (error != null && error.KeyViolation != null)
                    return new CRUDFieldValidationException(this, "Name", "This value is already used");
            }
            catch (Exception ex)
            {
                return ex;
            }
            return null;
        }

        protected override Exception DoDelete()
        {
            Exception error = null;
            try
            {
                var id = RoundtripBag[ITEM_ID_BAG_PARAM].AsNullableULong();
                CategoryRow row = id.HasValue ? ZApp.Data.CRUD.LoadRow(QCategory.findCategoryByID<CategoryRow>(id.Value)) : null;
                if (row != null)
                {
                    row.In_Use = false;
                    ZApp.Data.CRUD.Upsert(row);
                }
                else
                {
                    error = new CRUDFieldValidationException(row, "ID", "This value is not found");
                }
            }
            catch (Exception ex)
            {
                error = ex;
            }
            return error;
        }
    }
}

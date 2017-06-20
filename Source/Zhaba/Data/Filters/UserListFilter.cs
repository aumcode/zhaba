using NFX.DataAccess.CRUD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Zhaba.Data.Forms;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Filters
{
    public class UserListFilter : ZhabaFilterForm
    {
        #region Nested
        private class UserFilterListRow : TypedRow
        {
            [Field] public ulong  Counter      { get; set; }
            [Field] public string Login        { get; set; }
            [Field] public string First_Name   { get; set; }
            [Field] public string Last_Name    { get; set; }
            [Field] public string EMail        { get; set; }
        }
        #endregion

        [Field(valueList: "Name:Name Ascending,-Name:Name Descending",
                metadata: "Description='Sort By' Hint='Sort Project list by'")]
        public string OrderBy { get; set; }

        [Field(metadata: "Description='Login' Placeholder='Login' Hint='User Login'")]
        public string Login { get; set; }

        [Field(metadata: "Description='First Name' Placeholder='First Name' Hint='User First Name'")]
        public string First_Name { get; set; }

        [Field(metadata: "Description='Last Name' Placeholder='Last Name' Hint='User Last Name'")]
        public string Last_Name { get; set; }

        [Field(metadata: "Description='e-mail' Placeholder='e-mail' Hint='User e-mail'")]
        public string EMail { get; set; }

        protected override Exception DoSave(out object saveResult)
        {
            var qry = QCommon.UsersByFilter<UserFilterListRow>(this);
            saveResult = ZApp.Data.CRUD.LoadOneRowset(qry);
            return null;
        }

    }
}
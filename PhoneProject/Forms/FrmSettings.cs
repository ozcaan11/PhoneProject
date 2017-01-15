using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors;
using PhoneProject.Models;
using Type = PhoneProject.Models.Type;

namespace PhoneProject.Forms
{
    public partial class FrmSettings : XtraForm
    {
        private readonly mydb _db = new mydb();
        private readonly List<Type> _typeList = new List<Type>();

        public FrmSettings()
        {
            InitializeComponent();

            btnCancel.Click += (sender, args) => Close();

            Load += (sender, args) =>
            {
                var types = _db.Types.ToList();
                foreach (var type in types)
                    listBoxType.Items.Add(type.Name);

                _typeList.AddRange(types);
            };

            TypeClickEvents();
        }

        private void TypeClickEvents()
        {
            btnAdd.Click += (sender, args) =>
            {
                var name = txtName.Text;
                if(name == "" || name == "Enter name ..")return;

                _db.Types.Add(new Type {Name = name});
                listBoxType.Items.Add(name);
                txtName.Text = "";
            };

            btnRemove.Click += (sender, args) =>
            {
                var name = listBoxType.SelectedItem.ToString();

                for (var i = 0; i < _typeList.Count; i++)
                {
                    if (_typeList[i].Name != name) continue;
                    _db.Types.Remove(_typeList[i]);
                    _typeList.RemoveAt(i);
                }
                listBoxType.Items.Remove(listBoxType.SelectedItem);
            };
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _db.SaveChanges();
                Close();

            }
            catch (Exception)
            {
                XtraMessageBox.Show("You can't delete the old types!", "An Error occured!");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors;
using PhoneProject.Helper;
using PhoneProject.Models;
using PhoneProject.Properties;

namespace PhoneProject.Forms
{
    public partial class FrmNew : XtraForm
    {
        private Mydb _db;

        private readonly List<Phone> _phoneList = new List<Phone>();
        private readonly List<Email> _mailList = new List<Email>();

        public FrmNew()
        {
            InitializeComponent();

            btnCancel.Click += (sender, args) => Close();

            GetTypes();

            ControlThe.Mask(txtPhone);

            PhoneClickEvent();

            MailClickEvent();
        }

        private void MailClickEvent()
        {
            btnAddEmail.Click += (sender, args) =>
            {
                var mail = txtEmail.Text;
                if (mail == "") return;
                _mailList.Add(
                    new Email
                    {
                        Name = mail
                    });

                listEmail.Items.Add(mail);
                txtEmail.Text = "";
            };

            btnRemoveEmail.Click += (sender, args) =>
            {
                if (listEmail.Items.Count < 1) return;

                var mail = listEmail.SelectedItem.ToString();
                for (var i = 0; i < _mailList.Count; i++)
                    if (_mailList[i].Name == mail)
                        _mailList.RemoveAt(i);
                listEmail.Items.Remove(listEmail.SelectedItem);
            };
        }

        private void PhoneClickEvent()
        {
            btnAddPhone.Click += (sender, args) =>
            {
                var type = Convert.ToInt32(cmbType.EditValue);
                var number = txtPhone.Text;
                if (number == "") return;

                _phoneList.Add(
                    new Phone
                    {
                        Number = number,
                        TypeId = type
                    });

                listPhone.Items.Add(number);
                txtPhone.Text = "";
            };

            btnRemovePhone.Click += (sender, args) =>
            {
                if (listPhone.Items.Count < 1) return;

                var phone = listPhone.SelectedItem.ToString();

                for (var i = 0; i < _phoneList.Count; i++)
                    if (_phoneList[i].Number == phone)
                        _phoneList.RemoveAt(i);

                listPhone.Items.Remove(listPhone.SelectedItem);
            };
        }

        private void GetTypes()
        {
            using (_db = new Mydb())
            {
                var types = _db.Types.Select(x => new
                {
                    x.Id,
                    x.Name
                }).ToList();

                cmbType.Properties.DataSource = types;
                cmbType.Properties.DisplayMember = "Name";
                cmbType.Properties.ValueMember = "Id";
                cmbType.EditValue = 1;
            }
        }

        private void btnSave_Click(object sender, EventArgs e){
            if (FirstnameTextEdit.Text == "" ||
                NicknameTextEdit.Text == "" ||
                listPhone.Items.Count < 1 ||
                listEmail.Items.Count < 1)
                return;

            using (_db = new Mydb())
            {

                var user = new User
                {
                    Firstname = FirstnameTextEdit.Text,
                    Lastname = LastnameTextEdit.Text,
                    Adress = AdressMemoEdit.Text,
                    Nickname = NicknameTextEdit.Text
                };

                _db.Users.Add(user);

                foreach (var phone in _phoneList)
                {
                    _db.Phones.Add(new Phone { Number = phone.Number, TypeId = phone.TypeId, User = user });
                }

                foreach (var mail in _mailList)
                {
                    _db.Emails.Add(new Email { Name = mail.Name, User = user });
                }

                try
                {
                    _db.SaveChanges();
                    alertControl1.Show(this, "Success", "User successfully added to contact List", Resources.success);
                    Close();
                }
                catch (Exception)
                {
                    XtraMessageBox.Show("An Error occured");
                }
            }
        }
    }
}
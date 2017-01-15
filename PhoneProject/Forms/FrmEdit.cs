using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DevExpress.XtraEditors;
using PhoneProject.Models;
using PhoneProject.Properties;

namespace PhoneProject.Forms
{
    public partial class FrmEdit : XtraForm
    {
        private readonly mydb _db = new mydb();
        private User _user;

        private List<Phone> _phoneList = new List<Phone>();
        private List<Email> _mailList = new List<Email>();

        public FrmEdit(int id)
        {
            InitializeComponent();

            btnCancel.Click += (sender, args) => Close();

            PrepareForm(id);

            GetTypes();

            PhoneClickEvent();

            MailClickEvent();
        }

        private void PrepareForm(int id)
        {
            _user = _db.Users.Include(x => x.Phones).Include(x => x.Emails).FirstOrDefault(x => x.Id == id);

            if (_user == null) return;

            userBindingSource.DataSource = _user;

            _phoneList = _user.Phones.ToList();
            _mailList = _user.Emails.ToList();

            foreach (var phone in _phoneList)
            {
                listPhone.Items.Add(phone.Number);
            }

            foreach (var mail in _mailList)
            {
                listEmail.Items.Add(mail.Name);
            }
        }

        private void MailClickEvent()
        {
            btnAddEmail.Click += (sender, args) =>
            {
                var mail = txtEmail.Text;
                if (mail == "") return;
                _user.Emails.Add(new Email { Name = mail });


                listEmail.Items.Add(mail);
                txtEmail.Text = "";
            };

            btnRemoveEmail.Click += (sender, args) =>
            {
                if (listEmail.Items.Count < 1) return;

                var mail = listEmail.SelectedItem.ToString();
                for (var i = 0; i < _mailList.Count; i++)
                {
                    if (_mailList[i].Name != mail) continue;

                    _user.Emails.Remove(_mailList[i]);
                    _db.Emails.Remove(_mailList[i]);

                    _mailList.RemoveAt(i);
                }
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

                _user.Phones.Add(new Phone { Number = number, TypeId = type });

                listPhone.Items.Add(number);
                txtPhone.Text = "";
            };

            btnRemovePhone.Click += (sender, args) =>
            {
                if (listPhone.Items.Count < 1) return;

                var phone = listPhone.SelectedItem.ToString();

                for (var i = 0; i < _phoneList.Count; i++)
                {
                    if (_phoneList[i].Number != phone) continue;

                    _user.Phones.Remove(_phoneList[i]);
                    _db.Phones.Remove(_phoneList[i]);

                    _phoneList.RemoveAt(i);
                }
                listPhone.Items.Remove(listPhone.SelectedItem);
            };
        }

        private void GetTypes()
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (FirstnameTextEdit.Text == "" ||
                NicknameTextEdit.Text == "" ||
                listPhone.Items.Count < 1 ||
                listEmail.Items.Count < 1)
                return;


            _user.Firstname = FirstnameTextEdit.Text;
            _user.Lastname = LastnameTextEdit.Text;
            _user.Adress = AdressMemoEdit.Text;
            _user.Nickname = NicknameTextEdit.Text;

            _db.Entry(_user).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
                alertControl1.Show(this, "Success", "User successfully editted!", Resources.success);
                Close();
            }
            catch (Exception)
            {
                XtraMessageBox.Show("An Error occured");
            }
        }
    }
}
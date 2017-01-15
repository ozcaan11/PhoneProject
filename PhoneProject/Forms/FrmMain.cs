using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using PhoneProject.Models;
using PhoneProject.Properties;

namespace PhoneProject.Forms
{
    public partial class FrmMain : XtraForm
    {
        private Mydb _db;
        public FrmMain()
        {
            InitializeComponent();

            Load += (sender, args) => GetContactList();

            btnNew.Click += (sender, args) =>
            {
                var frm = new FrmNew();
                frm.FormClosed += (o, eventArgs) => GetContactList();
                frm.ShowDialog();
                ClearDetails();
            };

            btnEdit.Click += (sender, args) =>
            {
                var id = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Id"));
                var frm = new FrmEdit(id);
                frm.FormClosed += (o, eventArgs) => GetContactList();
                frm.ShowDialog();
                ClearDetails();
            };

            btnSettings.Click += (sender, args) =>
            {
                new FrmSettings().ShowDialog();
            };
        }

        private void GetContactList()
        {
            using (_db = new Mydb())
            {
                var datas = _db.Users
                    .Include(c => c.Phones)
                    .Include(c => c.Emails).Select(x => new
                    {
                        x.Id,
                        x.Nickname,
                        Fullname = x.Firstname + " " + x.Lastname,
                        x.Phones.FirstOrDefault().Number,
                        x.Emails.FirstOrDefault().Name,
                        x.Adress
                    }).ToList();

                gridControl1.DataSource = datas;
            }
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            ClearDetails();

            var id = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Id"));

            using (_db = new Mydb())
            {
                var data = _db.Users.Include(x => x.Phones).Include(x => x.Emails).FirstOrDefault(x => x.Id == id);

                if (data == null) return;

                lblFullname.Text = data.Firstname + @" " + data.Lastname;
                lblNickname.Text = data.Nickname;
                lblAdress.Text = data.Adress;
                foreach (var phone in data.Phones)
                    lblPhones.Text += phone.Type.Name.PadRight(9)+@": "+phone.Number + Resources.new_line;
                foreach (var mail in data.Emails)
                    lblMails.Text += mail.Name + Resources.new_line;
            }
        }
        private void ClearDetails()
        {
            lblPhones.Text = "";
            lblMails.Text = "";
            lblFullname.Text = "";
            lblAdress.Text = "";
            lblNickname.Text = "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Id"));
            var fullname = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Fullname").ToString();

            var delete = XtraMessageBox.Show("Do you really want to delete user ?", "Delete " + fullname + "?", MessageBoxButtons.YesNo);
            if (delete != DialogResult.Yes) return;

            using (_db = new Mydb())
            {
                var user = _db.Users.Include(x => x.Phones).Include(x => x.Emails).FirstOrDefault(x => x.Id == id);
                if (user == null) return;
                _db.Phones.RemoveRange(user.Phones);
                _db.Emails.RemoveRange(user.Emails);
                _db.Users.Remove(user);
                _db.SaveChanges();
            }
            GetContactList();
            return;
        }
    }
}
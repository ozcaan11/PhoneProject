using DevExpress.XtraEditors;

namespace PhoneProject.Helper
{
    public class ControlThe
    {
        public static void Mask(TextEdit txtPhone)
        {
            var kontrol = false;
            txtPhone.Click += (sender, args) =>
            {
                // Tek seferlik kontrol et
                // _kontrol'e ilk sefer için false değer atanır
                if (kontrol) return;
                if (txtPhone.SelectionStart > 0)
                    txtPhone.Select(0, 0);
                kontrol = true;
            };
            txtPhone.LostFocus += (sender, args) => kontrol = false;
        }
    }
}
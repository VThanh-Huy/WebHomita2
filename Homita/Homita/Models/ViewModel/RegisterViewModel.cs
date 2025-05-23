using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Homita.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Tên người dùng")]
        public string TenDangNhap { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; }

        [Required]
        [Display(Name = "Vai trò")]
        public string VaiTro { get; set; } // "KhachHang" hoặc "NhanVien"

        // Thông tin bổ sung cho Khách hàng
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }

        [Display(Name = "Số điện thoại")]
        public string SDT { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }
    }

}
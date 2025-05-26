using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homita.Models.Helper
{
    public class Create_UserID
    {
        private readonly TRA_SUAEntities1 _context;

        public Create_UserID(TRA_SUAEntities1 context)
        {
            _context = context;
        }

        public string GenerateNewMaTK()
        {
            var lastMa = _context.TaiKhoan
                .OrderByDescending(t => t.MaTK)
                .Select(t => t.MaTK)
                .FirstOrDefault();

            return GenerateNewCode("TK", lastMa);
        }

        public string GenerateNewMaKH()
        {
            var lastMa = _context.KhachHang
                .OrderByDescending(k => k.MaKH)
                .Select(k => k.MaKH)
                .FirstOrDefault();

            return GenerateNewCode("TK", lastMa);
        }

        public string GenerateNewMaNV()
        {
            var lastMa = _context.NhanVien
                .OrderByDescending(n => n.MaNV)
                .Select(n => n.MaNV)
                .FirstOrDefault();

            return GenerateNewCode("nd", lastMa);
        }

        private string GenerateNewCode(string prefix, string lastCode)
        {
            int nextNumber = 1;

            if (!string.IsNullOrEmpty(lastCode) && lastCode.Length >= 3)
            {
                string numberPart = lastCode.Substring(2);
                if (int.TryParse(numberPart, out int number))
                {
                    nextNumber = number + 1;
                }
            }

            return $"{prefix}{nextNumber.ToString("D3")}"; 
        }
    }
}
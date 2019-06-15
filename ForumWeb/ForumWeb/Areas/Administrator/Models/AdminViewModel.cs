using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ForumWeb.Areas.Administrator.Models
{
    public class AdminViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string TenDangNhap { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }
    }
}
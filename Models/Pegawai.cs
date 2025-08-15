using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMgmtMvc.Models
{
    public class Pegawai
    {
        public int Id { get; set; }
        [Required, StringLength(100)] public string Nama { get; set; } = string.Empty;
        [Required, Display(Name="NIK")]
        [RegularExpression(@"^\d{16}$", ErrorMessage="NIK harus 16 digit angka.")]
        public string NIK { get; set; } = string.Empty;
        [StringLength(200)] public string? Alamat { get; set; }
        [DataType(DataType.Date), Display(Name="Tanggal Lahir"), MinAge(17)] public DateTime TanggalLahir { get; set; }
        [Required, Display(Name="Jenis Kelamin")] public Gender JenisKelamin { get; set; }
        [Display(Name="Nomor Rekening")]
        [RegularExpression(@"^\d{10,20}$", ErrorMessage="Nomor rekening harus 10–20 digit angka.")]
        public string? NomorRekening { get; set; }
        [Required, EmailAddress, StringLength(254)] public string Email { get; set; } = string.Empty;
        [Phone, StringLength(20)] public string? Phone { get; set; }
        [Required, StringLength(50)] public string Department { get; set; } = string.Empty;
        [Required, DataType(DataType.Date), Display(Name="Tanggal Masuk")] public DateTime HireDate { get; set; }
        [Required, DataType(DataType.Currency), Precision(18,2)] public decimal Salary { get; set; }
    }
}
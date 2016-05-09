using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace lab.ngdemo.Models
{
    [Table("user", Schema = "public")]
    public class User
    {
        public User()
        {
            DateCreated = DateTime.Now;
        }

        [Key]
        [Required(ErrorMessage = "User Name is required")]
        [Display(Name = "User Name")]
        [MaxLength(100)]
        [Column("username")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(64)]
        [Column("passwordhash")]
        public byte[] PasswordHash { get; set; }

        [Required]
        [MaxLength(128)]
        [Column("passwordsalt")]
        public byte[] PasswordSalt { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid email address.")]
        [MaxLength(200)]
        [Column("email")]
        public string Email { get; set; }

        [MaxLength(200)]
        [Column("comment")]
        public string Comment { get; set; }

        [Display(Name = "Approved?")]
        [Column("isapproved")]
        public bool IsApproved { get; set; }

        [Display(Name = "Create Date")]
        [Column("datecreated")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Last Login Date")]
        [Column("datelastlogin")]
        public DateTime? DateLastLogin { get; set; }

        [Display(Name = "Last Activity Date")]
        [Column("datelastactivity")]
        public DateTime? DateLastActivity { get; set; }

        [Display(Name = "Last Password Change Date")]
        [Column("datelastpasswordchange")]
        public DateTime DateLastPasswordChange { get; set; }

        //public bool IsLoggedIn { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }

    [Table("role", Schema = "public")]
    public class Role
    {
        [Key]
        [Display(Name = "Role Name")]
        [Column("rolename")]
        public virtual string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }

    public class Employee
    {
        public int emp_id { get; set; }

        public string emp_name { get; set; }

        public string emp_emailaddress { get; set; }
    }
}
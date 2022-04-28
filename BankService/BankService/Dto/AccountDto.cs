using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BankService.Dto
{
    [DataContract]
    public class AccountDto
    {
       
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string FullName { get; set; }
        [DataMember]
        [Required(ErrorMessage = "Vui lòng nhâp tên")]
        public string Password { get; set; }
        [DataMember]
        [Required(ErrorMessage = "Vui lòng nhâp số dư tài khoản ")]
        public double Balancer { get; set; }

        [DataMember]
        [Key]
        [Required(ErrorMessage = "Vui lòng nhâp số tài khoản")]
        public string AccountNumber { get; set; }

            [DataMember]
            [DefaultValue(1)]
            public int Status { get; set; }

    }
}
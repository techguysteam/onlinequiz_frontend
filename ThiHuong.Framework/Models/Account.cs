﻿using System;
using System.Collections.Generic;

namespace ThiHuong.Framework.Models
{
    public partial class Account
    {
        public Account()
        {
            AccountInStage = new HashSet<AccountInStage>();
        }

        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Address { get; set; }
        public DateTime? Birthdate { get; set; }
        public int? RoleId { get; set; }
        public bool? Deleted { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<AccountInStage> AccountInStage { get; set; }
    }
}

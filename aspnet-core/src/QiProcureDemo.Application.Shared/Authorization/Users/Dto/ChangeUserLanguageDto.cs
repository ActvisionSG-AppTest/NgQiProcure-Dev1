﻿using System.ComponentModel.DataAnnotations;

namespace QiProcureDemo.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
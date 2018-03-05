﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPagesMovie.Models
{
    public class FileUpload
    {
        [Required]
        [Display(Name ="Title")]
        [StringLength(60, MinimumLength = 3)]

        public string Title { get; set; }
        [Required]
        [Display(Name ="Public Schedule")]
        //IFormFile表示以HttpRequest方式提交的文件
        public IFormFile UploadPublicSchedule { get; set; }

        [Required]
        [Display(Name ="Private Schedule")]
        public IFormFile UploadPrivateSchedule { get; set; }
    }
}

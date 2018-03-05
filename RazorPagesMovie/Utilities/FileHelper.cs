using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RazorPagesMovie.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RazorPagesMovie.Utilities
{
    public class FileHelper
    {
        public static async Task<string> ProcessFormFile(IFormFile formFile, ModelStateDictionary modelState)
        {
            var fieldDisplayName = string.Empty;
            //使用反射来包含model的DisplayName属性，关联IFormFile
            //如果display name没有找到，错误消息中就不会有这个name
            MemberInfo property = typeof(FileUpload).GetProperty(formFile.Name.Substring(formFile.Name.IndexOf(".") + 1));
            if (property != null)
            {
                var displayAttribute = property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                if (displayAttribute != null)
                {
                    fieldDisplayName = $"{displayAttribute.Name}";
                }
            }
            //使用 Path.GetFileName 来包含文件名，其将会剥离路径信息，作为文件名属性的一部分传送。
            //用HtmlEncode处理结构，以防其信息必须作为一个错误信息来返回
            var fileName = WebUtility.HtmlEncode(Path.GetFileName(formFile.FileName));

            if (formFile.ContentType.ToLower() != "text/plain")
            {
                modelState.AddModelError(formFile.Name, $"The{fieldDisplayName} file ({fileName}) must be a text file");
            }

            //检查文件长度，并且如果文件没有内容就不尝试去读取。
            //这个检查不会捕捉只有一个BOM作为内容的文件
            //所以文件长度的检查实在读取文件内容捕捉只包含一个BOM后执行的
            if (formFile.Length == 0)
            {
                modelState.AddModelError(formFile.Name, $"The {fieldDisplayName}file ({fileName}) is empty.");
            }
            else if (formFile.Length > 1048576)
            {
                modelState.AddModelError(formFile.Name, $"The {fieldDisplayName}file ({fileName}) exceeds 1 MB.");
            }
            else
            {
                try
                {
                    string fileContents;
                    //创建StreamReader去读取UTF-8的文件
                    //如果上传需要其他编码，在使用状态处提供编码。
                    //改为32位编码，将UTF8Encoding(...)改为UTF32Encoding().
                    using (var reader = new StreamReader(formFile.OpenReadStream(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true), detectEncodingFromByteOrderMarks: true))
                    {
                        fileContents = await reader.ReadToEndAsync();
                        //检查文档长度，以防止在移除BOM后变为空
                        if (fileContents.Length > 0)
                        {
                            return fileContents;
                        }
                        else
                        {
                            modelState.AddModelError(formFile.Name, $"The {fieldDisplayName}file ({fileName}) is empty.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    modelState.AddModelError(formFile.Name, $"The {fieldDisplayName}file ({fileName}) upload failed. " + $"Please contact the Help Desk for support.Error:{ex.Message}");
                }
            }
            return string.Empty;
        }
    }
}

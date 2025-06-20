using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlassCuttingOptimization.Utils
{
    public static class GlobalExceptionHandler
    {

        public static void Initialize()
        {
        }
        public static void HandleException(Exception ex, string source = null)
        {
            var errorMessage = BuildErrorMessage(ex, source);
            LogException(ex, errorMessage);
            ShowErrorMessage(errorMessage);
        }
        private static string BuildErrorMessage(Exception ex, string source)
        {
            var sb = new StringBuilder();
            sb.AppendLine("发生错误:");
            sb.AppendLine($"时间: {DateTime.Now}");

            if (!string.IsNullOrEmpty(source))
                sb.AppendLine($"来源: {source}");

            if (ex is BusinessException businessEx)
            {
                sb.AppendLine($"业务错误: {businessEx.Message}");
                sb.AppendLine($"错误代码: {businessEx.ErrorCode}");
            }
            else if (ex is ValidationException validationEx)
            {
                sb.AppendLine($"验证错误: {validationEx.Message}");
                if (validationEx.Errors != null)
                {
                    sb.AppendLine("验证详情:");
                    foreach (var error in validationEx.Errors)
                    {
                        sb.AppendLine($"- {error.Key}: {string.Join(", ", error.Value)}");
                    }
                }
            }
            else
            {
                sb.AppendLine($"系统错误: {ex.Message}");
                sb.AppendLine($"堆栈跟踪: {ex.StackTrace}");
            }

            return sb.ToString();
        }
        private static void LogException(Exception ex, string errorMessage)
        {
            //if (_logger == null) return;

            if (ex is BusinessException)
            {
                Serilog.Log.Warning(ex, errorMessage);
                //_logger.LogWarning(ex, errorMessage);
            }
            else
            {
                Serilog.Log.Error(ex, errorMessage);
                //_logger.LogError(ex, errorMessage);
            }
        }

        private static void ShowErrorMessage(string errorMessage)
        {
            if (System.Windows.Forms.Application.OpenForms.Count > 0)
            {
                MessageBox.Show(errorMessage, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public class BusinessException : BaseException
    {
        public BusinessException(string message, string errorCode = "BUSINESS_ERROR")
        : base(message, errorCode, 400)
        {
        }


    }


    public class BaseException : Exception
    {
        public string ErrorCode { get; }
        public int StatusCode { get; }


        public BaseException(string message, string errorCode = null, int statusCode = 500)
       : base(message)
        {
            ErrorCode = errorCode ?? "UNKNOWN_ERROR";
            StatusCode = statusCode;
        }

        public BaseException(string message, Exception innerException, string errorCode = null)
            : base(message, innerException)
        {
            ErrorCode = errorCode ?? "UNKNOWN_ERROR";
        }
    }


    public class ValidationException : BaseException
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(string message, IDictionary<string, string[]> errors = null)
            : base(message, "VALIDATION_ERROR", 400)
        {
            Errors = errors;
        }
    }
}

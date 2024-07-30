using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Exceptions;
using System.Net;

namespace SyriaTrustPlanning.Api.MiddleWares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly ILogger<ExceptionMiddleware> _Logger;

        public ExceptionMiddleware(RequestDelegate Next,
            ILogger<ExceptionMiddleware> Logger)
        {
            _Next = Next;
            _Logger = Logger;
        }

        public async Task InvokeAsync(HttpContext Context)
        {
            string? Language = Context.Request.Headers["lang"];

            try
            {
                await _Next(Context);
            }
            catch (Exception Ex)
            {
                if (string.IsNullOrEmpty(Language))
                    Language = "en";

                if (Language.ToLower() == "en")
                {
                    _Logger.LogError(Ex, "An error occurred");

                    Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    Context.Response.ContentType = "application/json";

                    //string ResponseMessage = Ex.InnerException != null
                    //    ? $"Exception = {Ex.Message}, Inner Exeption = {Ex.InnerException}"
                    //    : $"Exception = {Ex.Message}";

                    if (Ex is BadRequestException)
                    {
                        Context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                        BaseResponse<string> Response = new BaseResponse<string>
                            ("Bad request. Please check your input. " /*+*/
                            /*$"{ResponseMessage}"*/, false, (int)HttpStatusCode.BadRequest,
                            $"Exception = {Ex.Message}" +
                                (Ex.InnerException != null ? $"Inner Exeption = {Ex.InnerException.Message}" : null));

                        await Context.Response.WriteAsync(JsonConvert.SerializeObject(Response));
                    }
                    else if (Ex is SqlException)
                    {
                        Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        BaseResponse<string> Response = new BaseResponse<string>
                            ("Database connection error. " /*+*/
                            /*$"{ResponseMessage}"*/, false, (int)HttpStatusCode.InternalServerError,
                            $"Exception = {Ex.Message}" +
                                (Ex.InnerException != null ? $"Inner Exeption = {Ex.InnerException.Message}" : null));

                        await Context.Response.WriteAsync(JsonConvert.SerializeObject(Response));
                    }
                    else if (Ex is DbUpdateException)
                    {
                        if (Ex.InnerException != null)
                        {
                            if (Ex.InnerException is SqlException)
                            {
                                SqlException SqlEx = Ex.InnerException as SqlException;

                                foreach (SqlError error in SqlEx.Errors)
                                {
                                    if (error.Number == 2601 || error.Number == 2627)
                                    {
                                        if (error.Message.Contains("IX_GroupInvitees_Email", StringComparison.OrdinalIgnoreCase) ||
                                            error.Message.Contains("IX_PersonaLnvitees_Email", StringComparison.OrdinalIgnoreCase))
                                        {
                                            Context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                                            BaseResponse<string> Response = new BaseResponse<string>
                                                ("This email is already in use.", false, (int)HttpStatusCode.BadRequest,
                                                $"Exception = {Ex.Message}" +
                                                    (Ex.InnerException != null ? $"Inner Exeption = {Ex.InnerException.Message}" : null));

                                            await Context.Response.WriteAsync(JsonConvert.SerializeObject(Response));
                                        }
                                        else if (error.Message.Contains("Cannot insert duplicate key row in object"))
                                        {
                                            Context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                                            BaseResponse<string> Response = new BaseResponse<string>
                                                ("An error occurred. Cannot insert duplicate key row in object. " /*+*/
                                                /*$"{ResponseMessage}"*/, false, (int)HttpStatusCode.BadRequest,
                                                $"Exception = {Ex.Message}" +
                                                    (Ex.InnerException != null ? $"Inner Exeption = {Ex.InnerException.Message}" : null));

                                            await Context.Response.WriteAsync(JsonConvert.SerializeObject(Response));
                                        }
                                        else
                                        {
                                            Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                                            BaseResponse<string> Response = new BaseResponse<string>
                                                ("An error occurred. Please try again later. " /*+*/
                                                /*$"{ResponseMessage}"*/, false, (int)HttpStatusCode.InternalServerError,
                                                $"Exception = {Ex.Message}" +
                                                    (Ex.InnerException != null ? $"Inner Exeption = {Ex.InnerException.Message}" : null));

                                            await Context.Response.WriteAsync(JsonConvert.SerializeObject(Response));
                                        }
                                    }
                                    else
                                    {
                                        Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                                        BaseResponse<string> Response = new BaseResponse<string>
                                            ("An error occurred. Please try again later. " /*+*/
                                            /*$"{ResponseMessage}"*/, false, (int)HttpStatusCode.InternalServerError,
                                            $"Exception = {Ex.Message}" +
                                                (Ex.InnerException != null ? $"Inner Exeption = {Ex.InnerException.Message}" : null));

                                        await Context.Response.WriteAsync(JsonConvert.SerializeObject(Response));
                                    }
                                }
                            }
                            else
                            {
                                Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                                BaseResponse<string> Response = new BaseResponse<string>
                                    ("An error occurred. Please try again later. " /*+*/
                                    /*$"{ResponseMessage}"*/, false, (int)HttpStatusCode.InternalServerError,
                                    $"Exception = {Ex.Message}" +
                                        (Ex.InnerException != null ? $"Inner Exeption = {Ex.InnerException.Message}" : null));

                                await Context.Response.WriteAsync(JsonConvert.SerializeObject(Response));
                            }
                        }
                        else
                        {
                            Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                            BaseResponse<string> Response = new BaseResponse<string>
                                ("An error occurred. Please try again later. " /*+*/
                                /*$"{ResponseMessage}"*/, false, (int)HttpStatusCode.InternalServerError,
                                $"Exception = {Ex.Message}" +
                                    (Ex.InnerException != null ? $"Inner Exeption = {Ex.InnerException.Message}" : null));

                            await Context.Response.WriteAsync(JsonConvert.SerializeObject(Response));
                        }
                    }
                    else if (Ex is WebException ||
                        Ex is System.Net.Mail.SmtpException ||
                        Ex is AggregateException)
                    {
                        Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        BaseResponse<string> Response = new BaseResponse<string>
                            ("Internet connection error, please check your internet connection and try again later. " /*+*/
                            /*$"{ResponseMessage}"*/, false, (int)HttpStatusCode.InternalServerError,
                            $"Exception = {Ex.Message}" +
                                (Ex.InnerException != null ? $"Inner Exeption = {Ex.InnerException.Message}" : null));

                        await Context.Response.WriteAsync(JsonConvert.SerializeObject(Response));
                    }
                    else
                    {
                        Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        BaseResponse<string> Response = new BaseResponse<string>
                            ("An error occurred. Please try again later. " /*+*/
                            /*$"{ResponseMessage}"*/, false, (int)HttpStatusCode.InternalServerError,
                            $"Exception = {Ex.Message}" +
                                (Ex.InnerException != null ? $"Inner Exeption = {Ex.InnerException.Message}" : null));

                        await Context.Response.WriteAsync(JsonConvert.SerializeObject(Response));
                    }
                }
                else if (Language.ToLower() == "ar")
                {
                    _Logger.LogError(Ex, "حدث خطأ");

                    Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    Context.Response.ContentType = "application/json";

                    //string ResponseMessage = Ex.InnerException != null
                    //    ? $"استثناء = {Ex.Message}، الاستثناء الداخلي = {Ex.InnerException}"
                     //    : $"استثناء = {Ex.Message}";

                    if (Ex is BadRequestException)
                    {
                        Context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                        BaseResponse<string> Response = new BaseResponse<string>
                            ("إدخال خاطئ. الرجاء التحقق من المعلومات المدخلة. " /*+ */
                            /*$"{ResponseMessage}"*/, false, (int)HttpStatusCode.BadRequest,
                            $"Exception = {Ex.Message}" +
                                (Ex.InnerException != null ? $"Inner Exeption = {Ex.InnerException.Message}" : null));

                        await Context.Response.WriteAsync(JsonConvert.SerializeObject(Response));
                    }
                    else if (Ex is SqlException)
                    {
                        Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        BaseResponse<string> Response = new BaseResponse<string>
                            ("حدث خطأ بالاتصال بقاعدة المعطيات. " /*+*/
                            /*$"{ResponseMessage}"*/, false, (int)HttpStatusCode.InternalServerError,
                            $"Exception = {Ex.Message}" +
                                (Ex.InnerException != null ? $"Inner Exeption = {Ex.InnerException.Message}" : null));

                        await Context.Response.WriteAsync(JsonConvert.SerializeObject(Response));
                    }
                    else if (Ex is DbUpdateException)
                    {
                        if (Ex.InnerException != null)
                        {
                            SqlException SqlEx = Ex.InnerException as SqlException;

                            foreach (SqlError error in SqlEx.Errors)
                            {
                                if (error.Number == 2601 || error.Number == 2627)
                                {
                                    if (error.Message.Contains("IX_GroupInvitees_Email", StringComparison.OrdinalIgnoreCase) ||
                                        error.Message.Contains("IX_PersonaLnvitees_Email", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                                        BaseResponse<string> Response = new BaseResponse<string>
                                            ("هذا الإيميل مستخدم مسبقاً.", false, (int)HttpStatusCode.BadRequest,
                                            $"Exception = {Ex.Message}" +
                                                (Ex.InnerException != null ? $"Inner Exeption = {Ex.InnerException.Message}" : null));

                                        await Context.Response.WriteAsync(JsonConvert.SerializeObject(Response));
                                    }
                                    else if (error.Message.Contains("Cannot insert duplicate key row in object"))
                                    {
                                        Context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                                        BaseResponse<string> Response = new BaseResponse<string>
                                            ("حدث خطأ. لا يمكن إضافة قيمة متشابهة . " /*+*/
                                           /* $"{ResponseMessage}"*/, false, (int)HttpStatusCode.BadRequest,
                                            $"Exception = {Ex.Message}" +
                                                (Ex.InnerException != null ? $"Inner Exeption = {Ex.InnerException.Message}" : null));

                                        await Context.Response.WriteAsync(JsonConvert.SerializeObject(Response));
                                    }
                                    else
                                    {
                                        Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                                        BaseResponse<string> Response = new BaseResponse<string>
                                            ("حدث خطأ. الرجاء المحاولة لاحقأ. " /*+*/
                                           /* $"{ResponseMessage}"*/, false, (int)HttpStatusCode.InternalServerError,
                                            $"Exception = {Ex.Message}" +
                                                (Ex.InnerException != null ? $"Inner Exeption = {Ex.InnerException.Message}" : null));

                                        await Context.Response.WriteAsync(JsonConvert.SerializeObject(Response));
                                    }
                                }
                                else
                                {
                                    Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                                    BaseResponse<string> Response = new BaseResponse<string>
                                        ("حدث خطأ. الرجاء المحاولة لاحقأ. " /*+*/
                                       /* $"{ResponseMessage}"*/, false, (int)HttpStatusCode.InternalServerError,
                                        $"Exception = {Ex.Message}" +
                                            (Ex.InnerException != null ? $"Inner Exeption = {Ex.InnerException.Message}" : null));

                                    await Context.Response.WriteAsync(JsonConvert.SerializeObject(Response));
                                }
                            }
                        }
                        else
                        {
                            Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                            BaseResponse<string> Response = new BaseResponse<string>
                                ("حدث خطأ. الرجاء المحاولة لاحقأ. " /*+*/
                               /* $"{ResponseMessage}"*/, false, (int)HttpStatusCode.InternalServerError,
                                $"Exception = {Ex.Message}" +
                                    (Ex.InnerException != null ? $"Inner Exeption = {Ex.InnerException.Message}" : null));

                            await Context.Response.WriteAsync(JsonConvert.SerializeObject(Response));
                        }
                    }
                    else if (Ex is WebException ||
                        Ex is System.Net.Mail.SmtpException)
                    {
                        Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        BaseResponse<string> Response = new BaseResponse<string>
                            ("خطأ في الاتصال بالإنترنت، يرجى التحقق من اتصالك بالإنترنت والمحاولة مرة أخرى لاحقًا. "/* +*/
                            /*$"{ResponseMessage}"*/, false, (int)HttpStatusCode.InternalServerError,
                            $"Exception = {Ex.Message}" +
                                (Ex.InnerException != null ? $"Inner Exeption = {Ex.InnerException.Message}" : null));

                        await Context.Response.WriteAsync(JsonConvert.SerializeObject(Response));
                    }
                    else
                    {
                        Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        BaseResponse<string> Response = new BaseResponse<string>
                            ("حدث خطأ. الرجاء المحاولة لاحقاً. " /*+ */
                            /*$"{ResponseMessage}"*/, false, (int)HttpStatusCode.InternalServerError,
                            $"Exception = {Ex.Message}" +
                                (Ex.InnerException != null ? $"Inner Exeption = {Ex.InnerException.Message}" : null));

                        await Context.Response.WriteAsync(JsonConvert.SerializeObject(Response));
                    }
                }
            }
        }
    }
}

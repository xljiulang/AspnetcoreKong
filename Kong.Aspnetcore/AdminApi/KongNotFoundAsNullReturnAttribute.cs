using System.Net;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace Kong.Aspnetcore.AdminApi
{
    /// <summary>
    /// 表示kong的404响应处理为null值特性
    /// </summary>
    class KongNotFoundAsNullReturnAttribute : JsonReturnAttribute
    {
        /// <summary>
        /// 是否在成功的状态码
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        protected override bool IsSuccessStatusCode(HttpStatusCode statusCode)
        {
            return statusCode == HttpStatusCode.NotFound || base.IsSuccessStatusCode(statusCode);
        }

        public override async Task SetResultAsync(ApiResponseContext context)
        {
            if (context.HttpContext.ResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                context.Result = default;
            }
            else
            {
                await base.SetResultAsync(context);
            }
        }
    }
}

using System.Net;
using System.Threading.Tasks;
using WebApiClient.Attributes;
using WebApiClient.Contexts;

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

        /// <summary>
        /// 获取结果值
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override async Task<object> GetTaskResult(ApiActionContext context)
        {
            if (context.ResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                return default;
            }
            return await base.GetTaskResult(context);
        }
    }
}

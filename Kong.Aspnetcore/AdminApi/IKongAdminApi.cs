using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using WebApiClient;
using WebApiClient.Attributes;

namespace Kong.Aspnetcore.AdminApi
{
    /// <summary>
    /// 定义kong的管理接口
    /// </summary>
    public interface IKongAdminApi : IHttpApi
    { 
        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="kongService"></param>
        /// <returns></returns>
        [HttpPost("/services")]
        ITask<KongService> AddServiceAsync([JsonContent] KongServiceEdit kongService);

        /// <summary>
        /// 获取单个服务
        /// </summary>
        /// <param name="nameOrId"></param>
        /// <returns></returns>

        [HttpGet("/services/{nameOrId}")]
        ITask<KongService> GetServiceAsync([Required]string nameOrId);

        /// <summary>
        /// 获取所有服务
        /// </summary>
        /// <returns></returns>
        [HttpGet("/services")]
        ITask<KongArrayData<KongService>> GetServicesAsync();

        /// <summary>
        /// 更新服务
        /// </summary>
        /// <param name="nameOrId"></param>
        /// <param name="kongService"></param>
        /// <returns></returns>
        [HttpPatch("/services/{nameOrId}")]
        ITask<KongService> UpdateServiceAsync([Required]string nameOrId, [JsonContent]KongServiceEdit kongService);


        /// <summary>
        /// 删除服务
        /// </summary>
        /// <param name="nameOrId"></param>
        /// <returns></returns>
        [HttpDelete("/services/{nameOrId}")]
        ITask<HttpResponseMessage> DeleteServiceAsync([Required]string nameOrId);


        /// <summary>
        /// 添加路由
        /// </summary>
        /// <param name="serviceNameOrId"></param>
        /// <param name="kongRoute"></param>
        /// <returns></returns>
        [HttpPost("/services/{serviceNameOrId}/routes")]
        ITask<KongRoute> AddRouteAsync([Required]string serviceNameOrId, [JsonContent] KongRouteEdit kongRoute);


        /// <summary>
        /// 获取单个路由
        /// </summary>
        /// <param name="nameOrId"></param>
        /// <returns></returns>
        [HttpGet("/routes/{nameOrId}")]
        ITask<KongRoute> GetRouteAsync([Required]string nameOrId);

        /// <summary>
        /// 获取所有路由
        /// </summary>
        /// <returns></returns>
        [HttpGet("/routes")]
        ITask<KongArrayData<KongRoute>> GetRoutesAsync();

        /// <summary>
        /// 获取服务下的路由
        /// </summary>
        /// <param name="serviceNameOrId"></param>
        /// <returns></returns>
        [HttpGet("/services/{serviceNameOrId}/routes")]
        ITask<KongArrayData<KongRoute>> GetRoutesAsync([Required]string serviceNameOrId);

        /// <summary>
        /// 更新路由
        /// </summary>
        /// <param name="nameOrId"></param>
        /// <param name="kongRoute"></param>
        /// <returns></returns>
        [HttpPatch("/routes/{nameOrId}")]
        ITask<KongRoute> UpdateRouteAsync([Required]string nameOrId, [JsonContent]KongRouteEdit kongRoute);


        /// <summary>
        /// 删除路由
        /// </summary>
        /// <param name="nameOrId"></param>
        /// <returns></returns>
        [HttpDelete("/routes/{nameOrId}")]
        ITask<HttpResponseMessage> DeleteRouteAsync([Required]string nameOrId);

        /// <summary>
        /// 添加上游
        /// </summary>
        /// <param name="kongUpstream"></param>
        /// <returns></returns>
        [HttpPost("/upstreams")]
        ITask<KongUpstream> AddUpstreamAsync([JsonContent] KongUpstreamEdit kongUpstream);

        /// <summary>
        /// 获取上游
        /// </summary>
        /// <param name="nameOrId"></param>
        /// <returns></returns>

        [HttpGet("/upstreams/{nameOrId}")]
        ITask<KongUpstream> GetUpstreamAsync([Required]string nameOrId);

        /// <summary>
        /// 获取所有上游
        /// </summary>
        /// <returns></returns>
        [HttpGet("/upstreams")]
        ITask<KongArrayData<KongUpstream>> GetUpstreamsAsync();

        /// <summary>
        /// 更新上游
        /// </summary>
        /// <param name="nameOrId"></param>
        /// <param name="kongUpstream"></param>
        /// <returns></returns>
        [HttpPatch("/upstreams/{nameOrId}")]
        ITask<KongUpstream> UpdateUpstreamAsync([Required]string nameOrId, [JsonContent]KongUpstreamEdit kongUpstream);

        /// <summary>
        /// 删除上游
        /// </summary>
        /// <param name="nameOrId"></param>
        /// <returns></returns>
        [HttpDelete("/Upstreams/{nameOrId}")]
        ITask<HttpResponseMessage> DeleteUpstreamAsync([Required]string nameOrId);

        /// <summary>
        /// 为上游添加目标
        /// </summary>
        /// <param name="upstreamId"></param>
        /// <param name="kongTarget"></param>
        /// <returns></returns>

        [HttpPost("/upstreams/{upstreamId}/targets")]
        ITask<KongTarget> AddTargetAsync([Required] string upstreamId, [JsonContent] KongTargetEdit kongTarget);


        /// <summary>
        /// 获取上游的所有目标
        /// </summary>
        /// <param name="upstreamId"></param>
        /// <returns></returns>
        [HttpGet("/upstreams/{upstreamId}/targets")]
        ITask<KongArrayData<KongTarget>> GetTargetsAsync([Required] string upstreamId);


        /// <summary>
        /// 删除上游的某个目标
        /// </summary>
        /// <param name="upstreamId"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        [HttpDelete("/Upstreams/{upstreamId}/targets{targetId}")]
        ITask<HttpResponseMessage> DeleteTargetAsync([Required]string upstreamId, [Required]string targetId);
    }
}

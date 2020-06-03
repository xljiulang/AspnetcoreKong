using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace Kong.Aspnetcore.AdminApi
{
    /// <summary>
    /// 定义kong的管理接口
    /// </summary>
    [Timeout(1000)]
    public interface IKongAdminApi : IHttpApi
    {
        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="kongService"></param>
        /// <returns></returns>
        [HttpPost("/services")]      
        ITask<KongServiceObject> AddServiceAsync([JsonContent] KongService kongService);

        /// <summary>
        /// 获取单个服务
        /// </summary>
        /// <param name="nameOrId"></param>
        /// <returns></returns>

        [HttpGet("/services/{nameOrId}")]
        [KongNotFoundAsNullReturn]
        ITask<KongServiceObject> GetServiceAsync([Required]string nameOrId);

        /// <summary>
        /// 获取所有服务
        /// </summary>
        /// <returns></returns>
        [HttpGet("/services")]
        ITask<KongArray<KongServiceObject>> GetServicesAsync();

        /// <summary>
        /// 更新服务
        /// </summary>
        /// <param name="nameOrId"></param>
        /// <param name="kongService"></param>
        /// <returns></returns>
        [HttpPatch("/services/{nameOrId}")]
        ITask<KongServiceObject> UpdateServiceAsync([Required]string nameOrId, [JsonContent]KongService kongService);


        /// <summary>
        /// 删除服务
        /// </summary>
        /// <param name="nameOrId"></param>
        /// <returns></returns>
        [HttpDelete("/services/{nameOrId}")]
        Task DeleteServiceAsync([Required]string nameOrId);


        /// <summary>
        /// 添加路由
        /// </summary>
        /// <param name="serviceNameOrId"></param>
        /// <param name="kongRoute"></param>
        /// <returns></returns>
        [HttpPost("/services/{serviceNameOrId}/routes")]
        ITask<KongRouteObject> AddRouteAsync([Required]string serviceNameOrId, [JsonContent] KongRoute kongRoute);


        /// <summary>
        /// 获取单个路由
        /// </summary>
        /// <param name="nameOrId"></param>
        /// <returns></returns>
        [HttpGet("/routes/{nameOrId}")]
        [KongNotFoundAsNullReturn]
        ITask<KongRouteObject> GetRouteAsync([Required]string nameOrId);

        /// <summary>
        /// 获取所有路由
        /// </summary>
        /// <returns></returns>
        [HttpGet("/routes")]
        ITask<KongArray<KongRouteObject>> GetRoutesAsync();

        /// <summary>
        /// 获取服务下的路由
        /// </summary>
        /// <param name="serviceNameOrId"></param>
        /// <returns></returns>
        [HttpGet("/services/{serviceNameOrId}/routes")]
        ITask<KongArray<KongRouteObject>> GetRoutesAsync([Required]string serviceNameOrId);

        /// <summary>
        /// 更新路由
        /// </summary>
        /// <param name="nameOrId"></param>
        /// <param name="kongRoute"></param>
        /// <returns></returns>
        [HttpPatch("/routes/{nameOrId}")]
        ITask<KongRouteObject> UpdateRouteAsync([Required]string nameOrId, [JsonContent]KongRoute kongRoute);


        /// <summary>
        /// 删除路由
        /// </summary>
        /// <param name="nameOrId"></param>
        /// <returns></returns>
        [HttpDelete("/routes/{nameOrId}")]
        Task DeleteRouteAsync([Required]string nameOrId);

        /// <summary>
        /// 添加上游
        /// </summary>
        /// <param name="kongUpstream"></param>
        /// <returns></returns>
        [HttpPost("/upstreams")]
        ITask<KongUpstreamObject> AddUpstreamAsync([JsonContent] KongUpstream kongUpstream);

        /// <summary>
        /// 获取上游
        /// </summary>
        /// <param name="nameOrId"></param>
        /// <returns></returns>

        [HttpGet("/upstreams/{nameOrId}")]
        [KongNotFoundAsNullReturn]
        ITask<KongUpstreamObject> GetUpstreamAsync([Required]string nameOrId);

        /// <summary>
        /// 获取所有上游
        /// </summary>
        /// <returns></returns>
        [HttpGet("/upstreams")]
        ITask<KongArray<KongUpstreamObject>> GetUpstreamsAsync();

        /// <summary>
        /// 更新上游
        /// </summary>
        /// <param name="nameOrId"></param>
        /// <param name="kongUpstream"></param>
        /// <returns></returns>
        [HttpPatch("/upstreams/{nameOrId}")]
        ITask<KongUpstreamObject> UpdateUpstreamAsync([Required]string nameOrId, [JsonContent]KongUpstream kongUpstream);

        /// <summary>
        /// 删除上游
        /// </summary>
        /// <param name="nameOrId"></param>
        /// <returns></returns>
        [HttpDelete("/Upstreams/{nameOrId}")]
        Task DeleteUpstreamAsync([Required]string nameOrId);

        /// <summary>
        /// 为上游添加目标
        /// </summary>
        /// <param name="upstreamId"></param>
        /// <param name="kongTarget"></param>
        /// <returns></returns>

        [HttpPost("/upstreams/{upstreamId}/targets")]
        ITask<KongTargetObject> AddTargetAsync([Required] string upstreamId, [JsonContent] KongTarget kongTarget);


        /// <summary>
        /// 获取上游的所有目标
        /// </summary>
        /// <param name="upstreamId"></param>
        /// <returns></returns>
        [HttpGet("/upstreams/{upstreamId}/targets")]
        ITask<KongArray<KongTargetObject>> GetTargetsAsync([Required] string upstreamId);


        /// <summary>
        /// 删除上游的某个目标
        /// </summary>
        /// <param name="upstreamId"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        [HttpDelete("/Upstreams/{upstreamId}/targets{targetId}")]
        Task DeleteTargetAsync([Required]string upstreamId, [Required]string targetId);
    }
}

using System.Web.Http;
using CRM.Extend.HttpResponseMessages;

namespace CRM.Extend
{
    public class BaseApiController:ApiController
    {
        /// <summary>
        /// 没有权限
        /// </summary>
        internal void ReturnNoAuthority()
        {
            throw new HttpResponseException(new NoAuthorityMessage());
        }

        /// <summary>
        /// 数据没有找到
        /// </summary>
        internal void ReturnDataNotFound()
        {
            throw new HttpResponseException(new DataNotFoundMessage());
        }

        /// <summary>
        /// 系统异常
        /// </summary>
        internal void ReturnSystemException()
        {
            throw new HttpResponseException(new SystemExceptionMessage());
        }

        /// <summary>
        /// 登录失效
        /// </summary>
        internal void ReturnSiginFailure()
        {
            throw new HttpResponseException(new SiginFailureMessage());
        }


    }
}
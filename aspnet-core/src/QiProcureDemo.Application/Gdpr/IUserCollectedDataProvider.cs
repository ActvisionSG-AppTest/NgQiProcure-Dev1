using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using QiProcureDemo.Dto;

namespace QiProcureDemo.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}

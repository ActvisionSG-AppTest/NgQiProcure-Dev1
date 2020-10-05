using Abp.Dependency;
using GraphQL;
using GraphQL.Types;
using QiProcureDemo.Queries.Container;

namespace QiProcureDemo.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IDependencyResolver resolver) :
            base(resolver)
        {
            Query = resolver.Resolve<QueryContainer>();
        }
    }
}
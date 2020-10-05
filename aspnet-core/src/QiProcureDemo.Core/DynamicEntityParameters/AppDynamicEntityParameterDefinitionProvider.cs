using Abp.DynamicEntityParameters;
using Abp.UI.Inputs;
using QiProcureDemo.Authorization.Users;
using QiProcureDemo.CustomInputTypes;

namespace QiProcureDemo.DynamicEntityParameters
{
    public class AppDynamicEntityParameterDefinitionProvider : DynamicEntityParameterDefinitionProvider
    {
        public override void SetDynamicEntityParameters(IDynamicEntityParameterDefinitionContext context)
        {
            context.Manager.AddAllowedInputType<SingleLineStringInputType>();
            context.Manager.AddAllowedInputType<ComboboxInputType>();
            context.Manager.AddAllowedInputType<CheckboxInputType>();
            context.Manager.AddAllowedInputType<MultiSelectComboboxInputType>();

            //Add entities here 
            context.Manager.AddEntity<User, long>();
        }
    }
}

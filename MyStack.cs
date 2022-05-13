using System.Threading.Tasks;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;

class MyStack : Stack
{
    public MyStack()
    {
        // Create an Azure Resource Group
        var resourceGroup = new ResourceGroup("resourceGroup");

        var appService = new AppServicePlan("functionApp-appService", new AppServicePlanArgs
        {
            ResourceGroupName = resourceGroup.Name,
            Sku = new SkuDescriptionArgs
            {
                Name = "Y1",
                Tier = "Dynamic",
            },
        });

        var app = new WebApp("mywebapp", new WebAppArgs
        {
            ResourceGroupName = resourceGroup.Name,
            Kind = "functionapp",
            ServerFarmId = appService.Id
        });

        var credentials = ListWebAppPublishingCredentials.Invoke(new ListWebAppPublishingCredentialsInvokeArgs
        {
            Name = app.Name,
            ResourceGroupName = resourceGroup.Name
        });
    }
}

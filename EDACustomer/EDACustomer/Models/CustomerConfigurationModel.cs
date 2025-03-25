#nullable disable
namespace EDACustomer.Models
{
    public class CustomerConfigurationModel
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public List<ScreenConfigModel> ScreenConfig { get; set; }

    }

    public class ScreenConfigModel
    {
        public string ScreenName { get; set; }
        public string ScreenLabel { get; set; }
        public string PostURL { get; set; }
        public string FetchURL { get; set; }
        public string PutURL { get; set; }
        public string DeleteURL { get; set; }
        public List<FormConfigModel> FormConfigModel { get; set; }
    }

    public class FormConfigModel
    {
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string url { get; set; }
        public string defaultValue { get; set; }
        public string style { get; set; }
    }
}

using EDACustomer.Repository.Interface;
using EDADBContext;
using Helper;

namespace EDACustomer.Repository
{
    public class ConfigRepository : IConfigRepository
    {
        private readonly DBContext _dbContext;

        public ConfigRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public T? GetConfigValue<T>(string Key)
        {
            dynamic? configData = TypeConversionHelper.GetDefaultValue(typeof(T));
            try
            {
                var config = _dbContext.Config.FirstOrDefault(x => x.Key == Key);

                if (config != null)
                {
                    configData = (T)Convert.ChangeType(config.Value, typeof(T)); ;
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return configData;
        }
    }
}

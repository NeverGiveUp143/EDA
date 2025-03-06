using Helper.Models;

namespace Helper
{
    public static class ModelMapper
    {
        public static List<TTarget> SourceModelToTargetModel<TSource, TTarget>(List<TSource> sourceObjectList, List<ModelMapping> modelMappings)
     where TTarget : class, new()
        {
            List<TTarget> targetObjectList = new();

            if (sourceObjectList == null || sourceObjectList.Count == 0 ) return targetObjectList;

            Type sourceType = typeof(TSource);
            Type targetType = typeof(TTarget);

            foreach (var sourceObject in sourceObjectList)
            {
                var targetObj = SourceModelToTargetModel<TSource, TTarget>(sourceObject,modelMappings); // Create new Target Object             
                targetObjectList.Add(targetObj);
            }
            return targetObjectList;
        }

        public static TTarget SourceModelToTargetModel<TSource, TTarget>(TSource sourceObject, List<ModelMapping> modelMappings)
        where TTarget : class, new()
        {
            var targetObj = new TTarget();

            if (sourceObject == null) return targetObj;

            foreach (var modelMapping in modelMappings)
            {
                string sourcePropName = modelMapping.Source;
                string targetPropName = modelMapping.Target;
                object defaultValue = modelMapping.DefaultValue;

                // Get property info from source (Source Model) and target (Target Model)
                var sourceProp = typeof(TSource).GetProperty(sourcePropName);
                var targetProp = typeof(TTarget).GetProperty(targetPropName);

                if (sourceProp != null && targetProp != null)
                {
                    object? rawValue = sourceProp.GetValue(sourceObject) ?? TypeConversionHelper.GetDefaultValue(targetProp.PropertyType);
                    object value = rawValue ?? defaultValue;

                    targetProp.SetValue(targetObj, value);
                }
            }

            return targetObj;

        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using TestsUniversal.Model.Examination.Data;
using TestsUniversal.Model.Examination.Data.Yaml;

namespace TestsUniversal.Model.Examination
{
    public static class ParameterParser
    {
        public static IOrderedEnumerable<TaskParameterInfo> ParseVariantData(IEnumerable<TaskParameterDescription> descriprion,
                                                                             IEnumerable<TaskParameter<String>> strParameters)
        {
            if (!descriprion.Select(desc => desc.Name).
                SequenceEqual(strParameters.Select(data => data.Name)))
                throw new KeyNotFoundException();

            return descriprion.Join(strParameters,
                desc => desc.Name,
                data => data.Name,
                (desc, data) => new TaskParameterInfo(desc.Name, desc.Label, Parse(data.Value, desc.DataType), desc.Position)).
                    OrderBy(param => param.Position);
        }

        public static Object Parse(String value, ExamTaskDataType type)
        {
            if (value is null || value == String.Empty)
                return null;

            switch (type)
            {
                case ExamTaskDataType.Boolean: return Boolean.Parse(value);
                case ExamTaskDataType.Float: return Double.Parse(value.Replace('.', ','));
                case ExamTaskDataType.Integer: return Int64.Parse(value);
                case ExamTaskDataType.String: return value;
                default: return null;
            }
        }

        public static Boolean TryParse(String value, out Object result, ExamTaskDataType type)
        {
            result = null;
            if (value is null || value == String.Empty)
                return true;

            switch (type)
            {
                case ExamTaskDataType.Boolean:
                    {
                        var isValid = Boolean.TryParse(value, out Boolean parsedResult);
                        result = parsedResult;
                        return isValid;
                    }

                case ExamTaskDataType.Float:
                    {
                        var isValid = Double.TryParse(value.Replace('.', ','), out Double parsedResult);
                        result = parsedResult;
                        return isValid;
                    }

                case ExamTaskDataType.Integer:
                    {
                        var isValid = Int64.TryParse(value, out Int64 parsedResult);
                        result = parsedResult;
                        return isValid;
                    }

                case ExamTaskDataType.String:
                    result = value;
                    return true;

                default: return false;
            }
        }
    }
}

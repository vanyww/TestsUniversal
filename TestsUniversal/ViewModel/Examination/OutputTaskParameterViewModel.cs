using System;
using System.ComponentModel;
using TestsUniversal.Model.Examination;
using TestsUniversal.Model.Examination.Data;

namespace TestsUniversal.ViewModel.Examination
{
    public sealed class OutputTaskParameterViewModel : IDataErrorInfo
    {
        private const String ErrorMessage = "Ошибка!";

        public OutputTaskParameterViewModel() => m_strValue = String.Empty;

        public String this[String columnName]
        {
            get
            {
                var error = String.Empty;

                if (columnName == nameof(StrValue) && !m_isValueValid && StrValue != String.Empty)
                    error = ErrorMessage;

                return error;
            }
        }

        public ExamTaskDataType Type { get; set; }
        public Object Value { get; set; }
        public Object ValidValue { get; set; }
        public String StrValue
        {
            get => m_strValue;
            set
            {
                m_strValue = value;
                m_isValueValid = ParameterParser.TryParse(StrValue, out Object result, Type);
                if (m_isValueValid)
                    Value = (Type == ExamTaskDataType.Float) ?
                        Math.Round((Double)result, ExamSettings.Default.SignesNumber) :
                        result;
            }
        }
        public String Label { get; set; }

        public String Error => throw new NotImplementedException();

        private String m_strValue;
        private Boolean m_isValueValid;
    }
}

using DevExpress.Data.Filtering;
using System;
using tmLib;

namespace MyPhongTro.Module.Functions
{
    public class BangChuFunction : ICustomFunctionOperator
    {
        public const string FunctionName = "BangChu";

        public string Name => FunctionName;

        public object Evaluate(params object[] operands)
        {
            if (operands == null || operands.Length == 0 || operands[0] == null)
                return string.Empty;

            // Gọi hàm static từ ViCom
            return ViCom.ChuyenSo(Convert.ToString(operands[0]));
        }

        public Type ResultType(params Type[] operands)
        {
            return typeof(string);
        }
    

     public static void Register()
        {
            var instance = new BangChuFunction();
            if (CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }
    }
}

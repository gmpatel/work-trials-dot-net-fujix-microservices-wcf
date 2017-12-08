using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FXA.DPSE.Service.DTO.Core.Validation
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class RequiredWithGuidFormatAttribute : ValidationAttribute
    {
        private readonly bool _nullValidation;

        public RequiredWithGuidFormatAttribute(bool nullValidation = true)
        {
            _nullValidation = nullValidation;
        }

        public override bool IsValid(object value)
        {
            if (_nullValidation && value == null) return true;

            var guidString = Convert.ToString(value);
            
            //Optional field but the value is not empty.
            if (!string.IsNullOrEmpty(guidString))
            {
                Guid guid;
                return Guid.TryParseExact(guidString, "D", out guid);    
            }
            
            return false;
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class RequiredWithJsonUtcDateTimeFormatAttribute : ValidationAttribute
    {
        private readonly bool _nullOrWhiteSpaceValidation;

        public RequiredWithJsonUtcDateTimeFormatAttribute(bool nullOrWhiteSpaceValidation = true)
        {
            _nullOrWhiteSpaceValidation = nullOrWhiteSpaceValidation;
        }

        public override bool IsValid(object value)
        {
            // Validate the data format of value is "yyyy-MM-ddTHH:mm:ss.fffZ"
            
            var parts = value.ToString().Split(new char[] {'T'});

            if (parts.Count() != 2) return false;
            if (!parts[1].EndsWith("Z")) return false;

            var dateParts = parts[0].Split(new char[] {'-'});

            if (dateParts.Count() != 3) return false;
            if (dateParts[0].Length != 4 || !dateParts[0].IsNumeric()) return false;
            if (!(dateParts[1].Length >= 1 && dateParts[1].Length <= 2) || !dateParts[1].IsNumeric()) return false;
            if (!(dateParts[2].Length >= 1 && dateParts[2].Length <= 2) || !dateParts[2].IsNumeric()) return false;


            //if (dateaParts[0].Length != 4 || dateaParts[1].Length != 2 || dateaParts[2].Length != 2) return false;

            var timeParts = parts[1].Substring(0, parts[1].Length - 1).Split(new char[] { ':' });
            if (timeParts.Count() != 3) return false;

            if (!(timeParts[0].Length >= 1 && timeParts[0].Length <= 2) || !timeParts[0].IsNumeric()) return false;
            if (!(timeParts[1].Length >= 1 && timeParts[1].Length <= 2) || !timeParts[1].IsNumeric()) return false;
            var secondParts = timeParts[2].Split(new char[] { '.' });

            if (secondParts.Count() == 1)
            {
                if (!(secondParts[0].Length >= 1 && secondParts[0].Length <= 2) || !secondParts[0].IsNumeric()) return false;    
            }
            else if (secondParts.Count() == 2)
            {
                if (!(secondParts[0].Length >= 1 && secondParts[0].Length <= 2) || !secondParts[0].IsNumeric()) return false;
                if (!secondParts[1].IsNumeric()) return false;                    
            }
            else
            {
                return false;
            }

            return true;
        }
    }

    public static class DataHelper
    {
        public static bool IsNumeric(this string data)
        {
            long value;
            return long.TryParse(data, out value);
        }
    }
}
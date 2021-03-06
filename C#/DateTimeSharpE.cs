﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation;
using System.Text.RegularExpressions;

namespace ParameterClasses
{
    [ParameterClass]
    public class DateTimeSharpE : IComparable
    {
        [ParameterContract(ParameterContractType.Field, ParameterContractBehavior.Mandatory)]
        public DateTime Value;
        [ParameterContract(ParameterContractType.Field, ParameterContractBehavior.Mandatory)]
        public object InputObject;

        [ParameterContract(ParameterContractType.Field, ParameterContractBehavior.Mandatory)]
        public bool InFuture
        {
            get { return Value > DateTime.Now; }
        }

        public int CompareTo(object Item)
        {
            if (Item == null)
                throw new ArgumentNullException("Cannot compare with null");

            DateTimeSharpE tempInput = Item as DateTimeSharpE;

            if (tempInput == null)
                throw new ArgumentException(String.Format("Cannot compare DateTime with <{0}>", Item.GetType().FullName));

            return Value.CompareTo(tempInput.Value);
        }

        [ParameterContract(ParameterContractType.Operator, ParameterContractBehavior.Conversion)]
        public static implicit operator DateTime(DateTimeSharpE Parameter)
        {
            return Parameter.Value;
        }

        [ParameterContract(ParameterContractType.Operator, ParameterContractBehavior.Conversion)]
        public static implicit operator DateTimeSharpE(DateTime Value)
        {
            return new DateTimeSharpE(Value);
        }

        public static Dictionary<string, List<string>> PropertyMapping = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        public DateTimeSharpE(DateTime Value)
        {
            this.Value = Value;
            InputObject = Value;
        }

        public DateTimeSharpE(string Value)
        {
            this.Value = ParseDateTime(Value);
            InputObject = Value;
        }

        public DateTimeSharpE(object InputObject)
        {
            if (InputObject == null)
                throw new ArgumentException("Hey Gringo, you try converting $null to DateTime!");

            Value = ProcessObject(InputObject);
            this.InputObject = InputObject;
        }

        private static DateTime ParseDateTime(string Value)
        {
            if (String.IsNullOrWhiteSpace(Value))
                throw new ArgumentNullException("Cannot parse empty string!");

            try { return DateTime.Parse(Value, CultureInfo.CurrentCulture); }
            catch { }
            try { return DateTime.Parse(Value, CultureInfo.InvariantCulture); }
            catch { }

            bool positive = !(Value.Contains("-"));
            string tempValue = Value.Replace("-", "").Trim();
            bool date = Util.IsLike(tempValue, "D *");
            if (date)
                tempValue = tempValue.Substring(2);
            TimeSpan timeResult = new TimeSpan();

            foreach (string element in tempValue.Split(' '))
            {
                if (Regex.IsMatch(element, @"^\d+$"))
                    timeResult = timeResult.Add(new TimeSpan(0, 0, Int32.Parse(element)));
                else if (Util.IsLike(element, "*ms") && Regex.IsMatch(element, @"^\d+ms$", RegexOptions.IgnoreCase))
                    timeResult = timeResult.Add(new TimeSpan(0, 0, 0, 0, Int32.Parse(Regex.Match(element, @"^(\d+)ms$", RegexOptions.IgnoreCase).Groups[1].Value)));
                else if (Util.IsLike(element, "*s") && Regex.IsMatch(element, @"^\d+s$", RegexOptions.IgnoreCase))
                    timeResult = timeResult.Add(new TimeSpan(0, 0, Int32.Parse(Regex.Match(element, @"^(\d+)s$", RegexOptions.IgnoreCase).Groups[1].Value)));
                else if (Util.IsLike(element, "*m") && Regex.IsMatch(element, @"^\d+m$", RegexOptions.IgnoreCase))
                    timeResult = timeResult.Add(new TimeSpan(0, Int32.Parse(Regex.Match(element, @"^(\d+)m$", RegexOptions.IgnoreCase).Groups[1].Value), 0));
                else if (Util.IsLike(element, "*h") && Regex.IsMatch(element, @"^\d+h$", RegexOptions.IgnoreCase))
                    timeResult = timeResult.Add(new TimeSpan(Int32.Parse(Regex.Match(element, @"^(\d+)h$", RegexOptions.IgnoreCase).Groups[1].Value), 0, 0));
                else if (Util.IsLike(element, "*d") && Regex.IsMatch(element, @"^\d+d$", RegexOptions.IgnoreCase))
                    timeResult = timeResult.Add(new TimeSpan(Int32.Parse(Regex.Match(element, @"^(\d+)d$", RegexOptions.IgnoreCase).Groups[1].Value), 0, 0, 0));
                else
                    throw new ArgumentException(String.Format("Failed to parse as timespan: {0} at {1}", Value, element));
            }

            DateTime result;
            if (!positive)
                result = DateTime.Now.Add(timeResult.Negate());
            else
                result = DateTime.Now.Add(timeResult);

            if (date)
                return result.Date;
            return result;
        }

        public static DateTime ProcessObject(object Value)
        {
            PSObject input = new PSObject(Value);

            foreach (string name in input.TypeNames)
            {
                if (PropertyMapping.ContainsKey(name))
                {
                    foreach (string property in PropertyMapping[name])
                    {
                        if (input.Properties[property] != null && input.Properties[property].Value != null)
                        {
                            try { return (DateTime)input.Properties[property].Value; }
                            catch { }

                            try { return (DateTime)((PSObject)input.Properties[property].Value).BaseObject; }
                            catch { }

                            try { return new DateTimeSharpE(input.Properties[property].Value).Value; }
                            catch { }
                        }
                    }
                }
            }

            throw new ArgumentException(String.Format("Could not interpret {0}", Value.GetType().FullName));
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}

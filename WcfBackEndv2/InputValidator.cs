using System.Text.RegularExpressions;

namespace WcfBackEndv2
{
    public static class InputValidator
    {
        public static string RegexEmail { get => @"[\w\.-_]+@([\w\.-_]+\.)+[a-z]+"; }

        public static bool ValidateMinMaxLength(this string input, int min, int max)
        {
            return (input.Length >= min) && (input.Length <= max);
        }

        public static bool ValidateMinMax(this int input, int min, int max)
        {
            return (input >= min) && (input <= max);
        }

        public static bool ValidateRegex(this string input, string regex)
        {
            var rgx = new Regex(regex);
            return rgx.IsMatch(input);
        }
        public static bool ValidateEmail(this string input)
        {
            return input?.ValidateRegex(InputValidator.RegexEmail) ?? false;
        }
    }
}
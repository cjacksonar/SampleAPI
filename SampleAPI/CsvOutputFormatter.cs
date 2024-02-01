using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace SampleAPI
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }
        protected override bool CanWriteType(Type? type)
        {
            if (typeof(Data.Entities.Fund).IsAssignableFrom(type)
                || typeof(IEnumerable<Data.Entities.Fund>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }

            return false;
        }
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context,
            Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();

            if (context.Object is IEnumerable<Data.Entities.Fund>)
            {
                foreach (var fund in (IEnumerable<Data.Entities.Fund>)context.Object)
                {
                    FormatCsv(buffer, fund);
                }
            }
            else
            {
                FormatCsv(buffer, (Data.Entities.Fund)context.Object);
            }

            await response.WriteAsync(buffer.ToString());
        }
        private static void FormatCsv(StringBuilder buffer, Data.Entities.Fund fund)
        {
            buffer.AppendLine($"{fund.Id},\"{fund.FundName},\"{fund.Comments}\"");
        }
    }
}
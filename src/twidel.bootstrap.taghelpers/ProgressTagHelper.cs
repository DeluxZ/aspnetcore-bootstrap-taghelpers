using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace twidel.bootstrap.taghelpers
{
    [HtmlTargetElement("div", Attributes = ProgressValueAttributeName)]
    public class ProgressTagHelper : TagHelper
    {
        private const string ProgressValueAttributeName = "bs-progress-value";
        private const string ProgressMinAttributeName = "bs-progress-min";
        private const string ProgressMaxAttributeName = "bs-progress-max";

        [HtmlAttributeName(ProgressValueAttributeName)]
        public int ProgressValue { get; set; }

        [HtmlAttributeName(ProgressMinAttributeName)]
        public int ProgressMin { get; set; } = 0;

        [HtmlAttributeName(ProgressMaxAttributeName)]
        public int ProgressMax { get; set; } = 100;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ProgressMin >= ProgressMax)
            {
                throw new ArgumentException($"{ProgressMinAttributeName} must be less than {ProgressMaxAttributeName}");
            }

            if (ProgressValue > ProgressMax || ProgressValue < ProgressMin)
            {
                throw new ArgumentOutOfRangeException($"{ProgressValueAttributeName} must be within the range of {ProgressMinAttributeName} and {ProgressMaxAttributeName}");
            }

            var progressTotal = ProgressMax - ProgressMin;
            var progressPercentage = Math.Round((decimal)(ProgressValue - ProgressMin) / (decimal)progressTotal * 100, 0);

            string progressBarContent =
$@"<div class='progress-bar' role='progressbar' aria-valuenow='{ProgressValue}' aria-valuemin='{ProgressMin}' aria-valuemax='{ProgressMax}' style='width: {progressPercentage}%;'> 
   <span class='sr-only'>{progressPercentage}% Complete</span>
</div>";

            output.Content.AppendHtml(progressBarContent);

            string classValue;
            if (output.Attributes.ContainsName("class"))
            {
                classValue = $"{output.Attributes["class"]} progress";
            }
            else
            {
                classValue = "progress";
            }

            output.Attributes.Add("class", classValue);

            base.Process(context, output);
        }
    }
}
